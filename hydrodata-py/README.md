# Czechia Precipitation and Streamflow Download - Python
This commandline tool can be used to download last week's graphs of
streamflow, precipitation and temperature from the voda.gov.cz portal

Dependencies:
* python 3.6 or higher
* requests and requests_html python packages

Recommended installation and setup:
```
git clone https://github.com/jirikadlec2/hydrodata
cd hydrodata/hydrodata-py
python3 -m venv env
source env/bin/activate
pip install requests
pip install requests_html

python voda_gov_cz.py -o /home/jirka/meteodata -a pvl -dt precip
```

Arguments:
* -o: full path to the folder where results are saved
* -a: data provider agency. Use pvl, pla, poh or pod
* -dt: data type. Use streamflow or precip