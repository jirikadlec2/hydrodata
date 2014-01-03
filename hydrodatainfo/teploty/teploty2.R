server <- 'http://localhost/api/'
sites_url <- paste(server, 'sites?var=teplota',sep='')
sites <- read.table(sites_url, sep='\t', header=TRUE)

#This workaround is necessary for correctly reading hours!
Sys.setenv(TZ="GMT")

site_chmu = 996 #strojetice
site_povodi = 315 #zatec
od = '2012-06-10'
do = '2013-08-01'
datatype <- c("POSIXct")
names(datatype) <- c("datum")
url_chmu <- paste(server, 'values?var=teplota&step=h&site=',
                  site_chmu, '&od=', od, 
                  '&do=', do, sep='')
url_povodi <- paste(server, 'values?var=teplota&step=h&site=',
                    site_povodi, 
                    '&od=', od,
                    '&do=', do,
                    sep='') 
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
chmu$fixed[1] = chmu$teplota[1]
chmu$fixed[2] = chmu$teplota[2]
startindex = 2
endindex = 25
for(j in startindex:endindex) {
  chmu$fixed[j] = chmu$teplota[j]
}

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
    
    for(j in startindex:endindex) {
      if (!is.na(chmu$teplota[j])) {
        chmu$fixed[j] = chmu$teplota[j] - offset
      }
    }
  }
  offset = 0
}

for(day in 1:1) {
  
  startindex <- day * 24 + 2
  endindex <- startindex + 23
  
  #second correction by neighbour station
  chmu_tep <- chmu$fixed[startindex:endindex]
  povodi_tep <- povodi$teplota[startindex:endindex]
  tep_ix <- which(!is.na(chmu_tep) & !is.na(povodi_tep))
  if (length(tep_ix) > 0) {
    chmu_mean = mean(chmu_tep[tep_ix])      
    povodi_mean = mean(povodi_tep[tep_ix])
    pov_offset = chmu_mean - povodi_mean
    rounded_offset = 5 * round(pov_offset / 5)
    
    for(j in startindex:endindex) {
      if (!is.na(chmu$teplota[j])) {
        chmu$fixed[j] = chmu$fixed[j] - rounded_offset
      }
    }
  }  
}


plotmin = as.POSIXct('2013-02-01')
plotmax = as.POSIXct('2013-03-01')
subset = which(chmu$datum > plotmin & chmu$datum < plotmax)
subset2 = which(povodi$datum > plotmin & povodi$datum < plotmax)
plot(chmu$datum[subset], chmu$datum[subset], type="l",ylim=c(-25,30))
lines(chmu$datum[subset], chmu$fixed[subset],col="red")
lines(povodi$datum[subset2], povodi$teplota[subset2],col="green")
abline(a=0,b=0)
