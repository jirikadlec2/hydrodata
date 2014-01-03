#This workaround is necessary for correctly reading hours!
Sys.setenv(TZ="GMT")

site_chmu = 7
site_povodi = 348
od = '2012-06-10'
do = '2013-08-01'
datatype <- c("POSIXct")
names(datatype) <- c("datum")

file_chmu <- paste("site_", site_chmu, ".txt",sep='')
file_povodi <- paste("site_", site_povodi, ".txt",sep='')

chmu <- read.table(file_chmu, sep='\t', header=TRUE, colClasses=datatype)
povodi <- read.table(file_povodi, sep='\t', header=TRUE, colClasses=datatype)

#prevod datumu, pridani sloupce pro hodinu
chmu$hour <- (as.POSIXlt(chmu$datum))$hour
povodi$hour <- (as.POSIXlt(povodi$datum))$hour

diff = 0
offset = 0 #can be 5, 10, 15, 20, 25, 30..
maxdiff = 3.5
maxdiff2 = 7.0

#prvni den, teplotu nemenime..
startindex = 2
endindex = 25

chmu$fixed = chmu$teplota
chmu$povodi = povodi$teplota

maxi <- length(chmu$teplota)
numdays <- floor(maxi/24)

for(day in 1:numdays) {
  startindex <- day * 24 + 2
  endindex <- startindex + 23
  
  #first check the rapid change offset
  val_yesterday <- chmu$fixed[startindex-1]
  val_today <- chmu$teplota[startindex]
  if ((!is.na(val_yesterday)) & (!is.na(val_today))) {
    offset = 5 * round((val_today-val_yesterday) / 5)    
    chmu$fixed[startindex:endindex] = chmu$fixed[startindex:endindex] - offset
  }
}

na_indexes = which(is.na(chmu$teplota))
chmu$fixed[na_indexes] = NA

#correction by comparing to povodi
for(day in 1:numdays) {
  startindex <- day * 24 + 2
  endindex <- startindex + 23
  
  chmu_tep <- chmu$fixed[startindex:endindex]
  povodi_tep <- povodi$teplota[startindex:endindex]
  tep_ix <- which(!is.na(chmu_tep) & !is.na(povodi_tep))
  if (length(tep_ix) > 0) {
    chmu_mean = mean(chmu_tep[tep_ix])      
    povodi_mean = mean(povodi_tep[tep_ix])
    pov_offset = chmu_mean - povodi_mean
    offset = 5 * round((chmu_mean - povodi_mean) / 5)
    chmu$fixed[startindex:endindex] = chmu$fixed[startindex:endindex] - offset   
  }
}

#correction 3: remove jumps with nodata
for(day in 1:numdays) {
  startindex <- day * 24 + 1
  previndex <- startindex - 1
  postindex <- startindex + 1
  tep1 = chmu$fixed[previndex]
  if (is.na(tep1)) {
    tep2 <- chmu$fixed[startindex]
    tep3 <- chmu$fixed[postindex]
    if (!is.na(tep2) & !is.na(tep3)) {
      offset = 5 * round((tep3-tep2) / 5)
      chmu$fixed[startindex] = tep2 + offset
    }
  }
}

#correction 4 for shifts..
for(day in 1:numdays) {
  startindex <- day * 24 + 2
  endindex <- startindex + 23
  
  #again check the rapid change offset
  val_yesterday <- chmu$fixed[startindex-1]
  val_today <- chmu$fixed[startindex]
  val_povodi <- povodi$teplota[startindex-1]
  if (!is.na(val_yesterday) & !is.na(val_today) & !is.na(val_povodi)) {
    offset = 5 * round((val_today-val_yesterday) / 5)
    offset2 = 5 * round((val_today-val_povodi) / 5)
    if (abs(offset) > 4 & abs(offset2) > 4) {
      chmu$fixed[startindex:endindex] = chmu$fixed[startindex:endindex] - offset
    }
  }
  if (is.na(val_povodi) & !is.na(val_yesterday) & !is.na(val_today)) {
    offset = 5 * round((val_today-val_yesterday) / 5)
    if (abs(offset) > 4){
      chmu$fixed[startindex:endindex] = chmu$fixed[startindex:endindex] - offset
    }
  }
}
fixed_data <- data.frame(datum=chmu$datum, teplota_fixed=chmu$fixed)
file_fixed <- paste("fixed_", site_chmu, ".txt",sep='')
write.table(fixed_data, file_fixed, row.names=FALSE, sep='\t')

plotmin = as.POSIXct('2013-07-01')
plotmax = plotmin + 86400 * 31
subset = which(chmu$datum > plotmin & chmu$datum < plotmax)
subset2 = which(povodi$datum > plotmin & povodi$datum < plotmax)
ymin = round(min(chmu$fixed[subset],na.rm=TRUE))
ymax = round(max(chmu$fixed[subset],na.rm=TRUE))
plot(chmu$datum[subset], chmu$datum[subset], type="l",ylim=c(-20,30))
lines(chmu$datum[subset], chmu$fixed[subset],col="red")
lines(povodi$datum[subset2], povodi$teplota[subset2],col="green")
abline(a=0,b=0)
chmu_subset <- chmu[subset,]