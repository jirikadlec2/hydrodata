library(sp)
mysiteid <- 1006
pti <- which(sites$id==mysiteid)
pt <- cbind(sites$lon[pti],sites$lat[pti])
pts <- cbind(sites$lon, sites$lat)

sites2 <- sites
sites2$dist = spDistsN1(pts, pt, longlat=TRUE)
sites2ind <- which(sites2$operator != 'PVL')
sitesfilter <- sites2[sites2ind,]
sortedind <- order(sitesfilter$dist)
sites2sorted <- sitesfilter[sortedind,]