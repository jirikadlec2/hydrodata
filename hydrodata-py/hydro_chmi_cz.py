# downloads ALL GRAPHS from PVL into the specific directory

import os

from datetime import datetime
from pathlib import Path

from requests import get
from requests_html import HTMLSession
from urllib.parse import urljoin


def fetch_hpps_streamflow(dst_dir, url=None):
    """
    Fetch streamflow data from chmi fetch_hpps_data
    """
    session = HTMLSession()
    n_charts = 0

    datatype_prefix = 'streamflow'
    agency = 'chmi'

    pagesize = 50
    n_pages = 20

    for page in range(0, n_pages):
        subpage_url = "http://hydro.chmi.cz/hpps/hpps_oplist.php?startpos={0}&recnum={1}".format(page*pagesize, pagesize)
        print("----------------------------------------------------")
        print(subpage_url)
        print("----------------------------------------------------")
        session = HTMLSession()
        r = session.get(subpage_url)
 
        for lnk in r.html.absolute_links:
            if 'prfdyn' in lnk:
                print(lnk)
                
                station_seq = lnk.split('=')[-1]
                print(station_seq)

                data_dir = dst_dir / datatype_prefix / agency / station_seq
                if not os.path.exists(data_dir):
                    os.makedirs(data_dir)
                utc_timestamp_text = datetime.utcnow().strftime('%Y-%m-%dT%H0000z.html')

                html_filename = "prfdata_" + station_seq + "_" + utc_timestamp_text
                html_path = data_dir / html_filename

                # save the HTML with seven-day table
                lnk_table = lnk.replace('prfdyn', 'prfdata')
                print(lnk_table)
                html_response = get(lnk_table)
                if html_response.status_code == 200:
                    print(html_path)
                    with open(html_path, 'wb') as f:
                        f.write(html_response.content)


def fetch_hpps_precip(dst_dir, url=None):
    """
    Fetch streamflow data from chmi fetch_hpps_data
    """
    session = HTMLSession()
    n_charts = 0

    datatype_prefix = 'precip'
    agency = 'chmi'

    pagesize = 50
    n_pages = 12

    for day_offset in range(-7, -1):
        for page in range(0, n_pages):
            subpage_url = "http://hydro.chmi.cz/hpps/hpps_act_rain.php?day_offset={:d}&startpos={:d}&recnum={:d}".format(day_offset, page*pagesize, pagesize)
            print("----------------------------------------------------")
            print(subpage_url)
            print("----------------------------------------------------")

            data_dir = dst_dir / datatype_prefix / agency / "day-ofset-minus{:d}".format(day_offset)
            if not os.path.exists(data_dir):
                os.makedirs(data_dir)
            utc_timestamp_text = datetime.utcnow().strftime('%Y-%m-%dT%H0000z.html')

            html_filename = "act_rain_dayoffset{:d}_page{:d}_{:s}".format(day_offset, page, utc_timestamp_text)
            html_path = data_dir / html_filename
            print(html_path)

            html_response = get(subpage_url)
            if html_response.status_code == 200:
                with open(html_path, 'wb') as f:
                    f.write(html_response.content)
 

if __name__ == '__main__':

    fetch_hpps_precip(Path('C:/Users/Admin/Dropbox/meteodata'))
    fetch_hpps_streamflow(Path('C:/Users/Admin/Dropbox/meteodata'))
