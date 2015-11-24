library(WaterML)
server <- "http://hydrodata.info/CHMI-D/cuahsi_1_1.asmx"
sites <- GetSites(server)
siteInfos <- data.frame()
siteCodes <- sites$FullSiteCode
for (sc in siteCodes) {
  print(sc)
  si <- GetSiteInfo(server, sc)
  siteInfos <- rbind(siteInfos, si)
}
write.csv(siteInfos, "siteinfos_daily.csv")