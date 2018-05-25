# downloads ALL GRAPHS from PVL into the specific directory

import os

from datetime import datetime

from requests import get
from requests_html import HTMLSession
from urllib.parse import urljoin


def fetch_vodagov_charts(dst_dir, agency, base_url, subpages, datatype_prefix):
    """
    Fetch graphs and html tables from voda.gov.cz
    fetch_vodagov_charts(dst_dir='/home/jiri/meteodata',
                         agency_prefix='pod',
                         base_url='http://www.pvl.cz/portal/SaP/pc/?',
                         subpages=['oid=1', 'oid=2'],
                         datatype_prefix='streamflow',
                         agency_prefix='pod')

    :param dst_dir: destination directory where to save the data (subdirs are created automatically)
    :param base_url: the base url [for example http://www.pvl.cz/portal/SaP/pc/? for streamflow,
                                               http://www.pvl.cz/portal/srazky/pc/? for precipitation]
    :param subpages: the list of sub-pages (for example ['oid=1', 'oid=2', 'oid=3'])
    :param datatype_prefix: the data type. use 'streamflow' or 'precip'
    :param agency: the short name of the operating agency. use pla, poh, pod, pvl or pmo
    :return: number of charts and html pages downloaded
    """

    #if datatype_prefix == 'streamflow':
        #pvl_base = 'http://sap.poh.cz/portal/SaP/pc/?'
    #else:
        #pvl_base = 'http://sap.poh.cz/portal/Srazky/PC/?'

    session = HTMLSession()
    n_charts = 0

    for subpage in subpages:

        url = base_url + subpage
        r = session.get(url)

        for lnk in r.html.absolute_links:
            if 'Mereni.aspx?id=' or 'mereni.aspx?id=' in lnk:

                print(lnk)

                try:

                    r_st = session.get(lnk)

                    images = r_st.html.find('img')
                    for img in images:
                        src = img.attrs['src']
                        if ('graf' in src or 'Graf' in src) and ('miniatury' not in src):

                            img_src_absolute = urljoin(lnk, src)
                            print(img_src_absolute)

                            img_response = get(img_src_absolute)
                            if img_response.status_code == 200:

                                img_dir = os.path.join(dst_dir, datatype_prefix, agency, os.path.splitext(os.path.basename(img_src_absolute))[0])
                                if not os.path.exists(img_dir):
                                    os.makedirs(img_dir)
                                utc_timestamp_text = datetime.utcnow().strftime('_%Y-%m-%dT%H0000z.png')

                                img_filename = os.path.basename(img_src_absolute).replace('.png', utc_timestamp_text)

                                img_path = os.path.join(img_dir, img_filename)
                                print(img_path)
                                with open(img_path, 'wb') as f:
                                    f.write(img_response.content)

                                # also save the HTML
                                html_path = img_path.replace('.png', '.html')
                                html_response = get(lnk)
                                if html_response.status_code == 200:
                                    print(html_path)
                                    with open(html_path, 'wb') as f:
                                        f.write(html_response.content)

                            n_charts += 1

                except ValueError:
                    print('ERROR fetching ' + lnk)
    return n_charts


if __name__ == '__main__':

    dst_dir = '/home/jiri/meteodata'

    out_result = []

    agency = 'pod'
    n_streamflow = fetch_vodagov_charts(dst_dir, agency,
                         base_url='http://www.pod.cz/portal/SaP/en/pc/?',
                         subpages=['oid=1', 'oid=2'],
                         datatype_prefix='streamflow')

    n_precip = fetch_vodagov_charts(dst_dir, agency,
                                        base_url='https://www.pod.cz/portal/Srazky/en/pc/?',
                                        subpages=['oid=1', 'oid=2'],
                                        datatype_prefix='precip')

    out_result.append({'agency':agency, 'streamflow_charts':n_streamflow, 'precip_charts': n_precip})


    agency = 'poh'
    n_streamflow = fetch_vodagov_charts(dst_dir, agency,
                                        base_url='http://sap.poh.cz/portal/SaP/en/pc/?',
                                        subpages=['oid=1', 'oid=2', 'oid=3'],
                                        datatype_prefix='streamflow')

    n_precip = fetch_vodagov_charts(dst_dir, agency,
                                    base_url='http://sap.poh.cz/portal/Srazky/en/pc/?',
                                    subpages=['oid=1', 'oid=2', 'oid=3'],
                                    datatype_prefix='precip')

    out_result.append({'agency': agency, 'streamflow_charts': n_streamflow, 'precip_charts': n_precip})

    agency = 'pvl'
    n_streamflow = fetch_vodagov_charts(dst_dir, agency,
                                        base_url='http://www.pvl.cz/portal/SaP/en/pc/?',
                                        subpages=['oid=1', 'oid=2', 'oid=3'],
                                        datatype_prefix='streamflow')

    n_precip = fetch_vodagov_charts(dst_dir, agency,
                                    base_url='http://www.pvl.cz/portal/Srazky/en/pc/?',
                                    subpages=['oid=1', 'oid=2', 'oid=3'],
                                    datatype_prefix='precip')

    out_result.append({'agency': agency, 'streamflow_charts': n_streamflow, 'precip_charts': n_precip})

    agency = 'pla'
    n_streamflow = fetch_vodagov_charts(dst_dir, agency,
                                        base_url='http://www.pla.cz/portal/SaP/en/PC/?',
                                        subpages=['oid=1', 'oid=2'],
                                        datatype_prefix='streamflow')

    n_precip = fetch_vodagov_charts(dst_dir, agency,
                                    base_url='http://www.pla.cz/portal/Srazky/en/PC/?',
                                    subpages=['oid=1', 'oid=2'],
                                    datatype_prefix='precip')

    out_result.append({'agency': agency, 'streamflow_charts': n_streamflow, 'precip_charts': n_precip})

    print(out_result)
