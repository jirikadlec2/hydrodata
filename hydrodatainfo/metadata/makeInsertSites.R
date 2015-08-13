#process the sites (from HISCentral)
sites$insert <- paste(
  "INSERT INTO plaveninycz.stations(st_id, st_seq, st_name, lat, lon) VALUES (",
                      sites$SiteCode, ",", sites$SiteCode, ",'", sites$SiteName, "',",
  sites$Latitude, ", ", sites$Longitude, ");",
  sep="")
write.table(sites$insert, "insert_sites.txt", row.names=FALSE, 
            col.names=FALSE, quote=FALSE)
