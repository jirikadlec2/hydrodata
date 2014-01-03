server <- 'http://hydrodata.info/api/'
sites_url <- paste(server, 'sites?var=teplota',sep='')
sites <- read.table(sites_url, sep='\t', header=TRUE)
print(sites$name[25])
download.file(sites_url, "sites.txt")

sites <- read.table("sites.txt", sep='\t', header=TRUE)

#This workaround is necessary for correctly reading hours!
Sys.setenv(TZ="GMT")
od = '2012-06-10'
do = '2013-08-01'

for (i in 1:length(sites$id)){
  siteid <- sites$id[i]
  sitename <- sites$name[i]
  url <- paste(server, 'values?var=teplota&step=h&site=',
               siteid, '&od=', od, 
               '&do=', do, sep='')
  filename <- paste("site_", siteid, '.txt', sep='')
  download.file(url, filename)
}