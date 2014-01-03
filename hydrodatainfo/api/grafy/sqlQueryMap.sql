select st.lon, st.lat, st.altitude, avg(sn.snow_cm) from plaveninycz.snow sn
inner join plaveninycz.stations st
on sn.station_id = st.st_id
where sn.time_utc between '2013-01-20' and '2013-01-21'
group by st.lon, st.lat, st.altitude