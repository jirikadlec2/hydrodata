server <- 'http://localhost:49200/'
sites_url <- paste(server, 'sites?var=teplota',sep='')
sites <- read.table(sites_url, sep='\t', header=TRUE)

site_chmu = 4 #destne
site_povodi = 338 #sedlonov
od = '2012-06-10'
do = '2013-04-15'
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

chmu <- read.table(url_chmu, sep='\t', header=TRUE)
povodi <- read.table(url_povodi, sep='\t', header=TRUE)

#prevod datumu
for (i in 1:length(chmu$datum)) {
  dt <- as.POSIXct(chmu$datum[i])
  chmu$datetime[i] <- dt
  chmu$hour[i] <- (as.POSIXlt(dt))$hour
}

for (i in 1:length(povodi$datum)) {
  dt <- as.POSIXct(povodi$datum[i])
  povodi$datetime[i] <- dt
  povodi$hour[i] <- (as.POSIXlt(dt))$hour
}

diff = 0
offset = 0 #can be 5, 10, 15, 20, 25, 30..
maxdiff = 3.5
maxdiff2 = 7.0

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
  val01 <- chmu$fixed[startindex-1]
  val02 <- chmu$teplota[startindex]
  if ((!is.na(val01)) & (!is.na(val02)) {
    offset = val01 - val02
    if (offset > maxdiff2) {
      offset = 10.0
    } else if (offset > maxdiff) {
      offset = 5.0
    } else if (offset < -maxdiff) {
      offset = -5.0
    } else if (offset < -maxdiff2) {
      offset = -10.0
    }
    
    for(j in startindex:endindex) {
      chmu$fixed[j] = chmu$teplota[j] - offset
    }
  }
  offset = 0
}

  
  chmu_tep <- chmu$fixed[startindex:endindex]
  povodi_tep <- povodi$teplota[startindex:endindex]
  chmu_ix <- which(!is.na(chmu_tep))
  povodi_ix <- which(!is.na(povodi_tep))
  if ((length(chmu_ix) > 0) & (length(povodi_ix) > 0)) {
    chmu_mean = mean(chmu_tep[chmu_ix])      
    povodi_mean = mean(povodi_tep[povodi_ix])
    pov_offset = chmu_mean - povodi_mean
    sign = 1
    if (pov_offset < 0){
      sign = -1
    }
    pov_offset_5 <- 5*floor((pov_offset*sign) / 5)
    offset = pov_offset_5 * sign
    for(j in startindex:endindex) {
      chmu$fixed[j] = chmu$fixed[j] -offset
    }
  }
}


plotmin = as.POSIXct('2012-07-20')
plotmax = as.POSIXct('2012-07-30')
subset = which(chmu$datetime > plotmin & chmu$datetime < plotmax)
subset2 = which(povodi$datetime > plotmin & povodi$datetime < plotmax)
plot(chmu$datetime[subset], chmu$teplota[subset], type="l",ylim=c(-20,25))
lines(chmu$datetime[subset], chmu$fixed[subset],col="red")
lines(povodi$datetime[subset2], povodi$teplota[subset2],col="green")
abline(a=0,b=0)
