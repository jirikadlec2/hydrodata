update stations set division_name=1, operator_id=3 where st_id in (
49,485,236,487,215,486,199,488,741,292,95,499,489);

update stations set division_name=2, operator_id=3 where st_id in (
490,491,237,311,305,307,306,308,87);

update stations set division_name=3, operator_id=3 where st_id in (
492,131,524,103,500,496,494,495,811,497,240,498);

update stations set division_name=1, meteo_code='1001' where st_id=289;
update stations set division_name=1, meteo_code='1405' where st_id=297;
update stations set division_name=1, meteo_code='1002' where st_id=290;
update stations set division_name=1, meteo_code='1022' where st_id=294;
update stations set division_name=1, meteo_code='1091' where st_id=236;
update stations set division_name=1, meteo_code='1480' where st_id=299;
update stations set division_name=1, meteo_code='1410' where st_id=298;
update stations set division_name=1, meteo_code='1011' where st_id=291;
update stations set division_name=1, meteo_code='1211' where st_id=488;
update stations set division_name=1, meteo_code='1021' where st_id=293;
update stations set division_name=1, meteo_code='1012' where st_id=292;
update stations set division_name=1, meteo_code='1482' where st_id=301;

update stations set division_name=2, meteo_code='2001' where st_id=302;
update stations set division_name=2, meteo_code='2002' where st_id=303;
update stations set division_name=2, meteo_code='2408' where st_id=311;
update stations set division_name=2, meteo_code='2231' where st_id=310;
update stations set division_name=2, meteo_code='2031' where st_id=237;
update stations set division_name=2, meteo_code='2033' where st_id=238;
update stations set division_name=2, meteo_code='2034' where st_id=305;
update stations set division_name=2, meteo_code='2043' where st_id=307;
update stations set division_name=2, meteo_code='2141' where st_id=309;
update stations set division_name=2, meteo_code='2418' where st_id=312;
update stations set division_name=2, meteo_code='2036' where st_id=306;
update stations set division_name=2, meteo_code='2037' where st_id=239;
update stations set division_name=2, meteo_code='2091' where st_id=308;

update stations set division_name=3, meteo_code='3401' where st_id=315;
update stations set division_name=3, meteo_code='3104' where st_id=314;
update stations set division_name=3, meteo_code='3481' where st_id=316;
update stations set division_name=3, meteo_code='3091' where st_id=313;
update stations set division_name=3, meteo_code='3092' where st_id=240;

update stations set st_name2='poh_1409', tok='Libocký potok', st_uri='libocky_potok/horka', riv_id=140390000100 where st_id=236;
insert into stationsvariables(st_id, var_id, is_public) values (236, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (236, 5, 1);

update stations set st_name2='poh_1426', tok='Lomnický potok', st_uri='lomnicky_potok/stanovice', riv_id=141480000100 where st_id=292;
insert into stationsvariables(st_id, var_id, is_public) values (292, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (292, 5, 1);

update stations set st_name2='poh_2433', tok='Přísečnice', st_uri='prisecnice/prisecnice', riv_id=147500000100 where st_id=237;
insert into stationsvariables(st_id, var_id, is_public) values (237, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (237, 5, 1);

update stations set st_name2='poh_2407', tok='Hačka', st_uri='hacka/hacka', riv_id=143480000100 where st_id=311;
insert into stationsvariables(st_id, var_id, is_public) values (311, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (311, 5, 1);

update stations set st_name2='poh_2443', tok='Bílina', st_uri='bilina/jirkov', riv_id=144190000100 where st_id=305;
insert into stationsvariables(st_id, var_id, is_public) values (305, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (305, 5, 1);

update stations set st_name2='poh_2414', tok='Bílina', st_uri='bilina/ujezd', riv_id=144190000100 where st_id=307;
insert into stationsvariables(st_id, var_id, is_public) values (307, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (307, 5, 1);

update stations set st_name2='poh_2446', tok='Loupnice', st_uri='loupnice/janov', riv_id=144221300100 where st_id=306;
insert into stationsvariables(st_id, var_id, is_public) values (306 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (306, 5, 1);

update stations set st_name2='poh_2423', tok='Bouřlivec', st_uri='bourlivec/vsechlapy', riv_id=144470000100 where st_id=308;
insert into stationsvariables(st_id, var_id, is_public) values (308 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (308, 5, 1);

update stations set st_name2='poh_3412', tok='Chřibská Kamenice', st_uri='chribska_kamenice/vd_chribska', riv_id=146390000100 where st_id=240;
insert into stationsvariables(st_id, var_id, is_public) values (240 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (240, 5, 1);

update stations set meteo_code='1001' where st_id=148;
update stationsvariables set st_id=148 where st_id=289;
update rain_daily set station_id=148 where station_id=289;
update rain_hourly set station_id=148 where station_id=289;
update temperature set station_id=148 where station_id=289;
delete from stations where st_id=289;

update stations set meteo_code='1002' where st_id=485;
update stationsvariables set st_id=485 where st_id=290;
update rain_daily set station_id=485 where station_id=290;
update rain_hourly set station_id=485 where station_id=290;
update temperature set station_id=485 where station_id=290;
delete from stations where st_id=290;

update stations set meteo_code='1410' where st_id=487;
update stationsvariables set st_id=487 where st_id=298;
update rain_daily set station_id=487 where station_id=298;
update rain_hourly set station_id=487 where station_id=298;
update temperature set station_id=487 where station_id=298;
delete from stations where st_id=298;

update stations set meteo_code='1011' where st_id=199;
update stationsvariables set st_id=199 where st_id=291;
update rain_daily set station_id=199 where station_id=291;
update rain_hourly set station_id=199 where station_id=291;
update temperature set station_id=199 where station_id=291;
delete from stations where st_id=291;

update stations set meteo_code='1021' where st_id=741;
update stationsvariables set st_id=741 where st_id=293;
update rain_daily set station_id=741 where station_id=293;
update rain_hourly set station_id=741 where station_id=293;
update temperature set station_id=741 where station_id=293;
delete from stations where st_id=293;

update stations set meteo_code='3401' where st_id=492;
update stationsvariables set st_id=492 where st_id=315;
update rain_daily set station_id=492 where station_id=315;
update rain_hourly set station_id=492 where station_id=315;
update temperature set station_id=492 where station_id=315;
delete from stations where st_id=315;

update stations set meteo_code='3481' where st_id=500;
update stationsvariables set st_id=500 where st_id=316;
update rain_daily set station_id=500 where station_id=316;
update rain_hourly set station_id=500 where station_id=316;
update temperature set station_id=500 where station_id=316;
delete from stations where st_id=316;

update stations set meteo_code='34091' where st_id=495;
update stationsvariables set st_id=495 where st_id=313;
update rain_daily set station_id=495 where station_id=313;
update rain_hourly set station_id=495 where station_id=313;
update temperature set station_id=495 where station_id=313;
delete from stations where st_id=313;


insert into stations(st_id, st_seq, st_name, tok, st_name2, st_uri, riv_id, operator_id, division_name, lat, lon, altitude) values (
1294, 99991294, 'Jánský most', 'Teplá', 'poh_1427', 'tepla/jansky_most', 141270000100, 3, 1, 50.2222792, 12.8826122, 375);
insert into stationsvariables(st_id, var_id, is_public) values (1294, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1294, 5, 1);

insert into stations(st_id, st_seq, st_name, tok, st_name2, st_uri, riv_id, operator_id, division_name, lat, lon, altitude) values (
1295, 99991295, 'Chomutov', 'Chomutovka', 'poh_2475', 'chomutovka/chomutov', 143390000100, 3, 2, 50.4711539, 13.3926861, 368);
insert into stationsvariables(st_id, var_id, is_public) values (1295, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1295, 5, 1);

insert into stations(st_id, st_seq, st_name, tok, st_name2, st_uri, riv_id, operator_id, division_name, lat, lon, altitude) values (
1296, 99991296, 'Záluží', 'Bílý potok', 'poh_2449', 'bily_potok/zaluzi', 144230000100, 3, 2, 50.5753731, 13.6015744, NULL);
insert into stationsvariables(st_id, var_id, is_public) values (1296, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1296, 5, 1);

insert into stations(st_id, st_seq, st_name, tok, st_name2, st_uri, riv_id, operator_id, division_name, lat, lon, altitude) values (
1297, 99991297, 'Český Jiřetín', 'Flájský potok', 'poh_2448', 'flajsky_potok/cesky_jiretin', 147620000100, 3, 2, 50.7073575, 13.5431375, 605);
insert into stationsvariables(st_id, var_id, is_public) values (1297, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1297, 5, 1);

insert into stations(st_id, st_seq, st_name, tok, st_name2, st_uri, riv_id, operator_id, division_name, lat, lon, altitude) values (
1298, 99991298, 'Bílina', 'Bílina', 'poh_2424', 'bilina/bilina', 144190000100, 3, 2, 50.5538156, 13.7731475, 203);
insert into stationsvariables(st_id, var_id, is_public) values (1298, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1298, 5, 1);

insert into stations(st_id, st_seq, st_name, tok, st_name2, st_uri, riv_id, operator_id, division_name, lat, lon, altitude) values (
1299, 99991299, 'Vilémov', 'Liboc', 'poh_3431', 'liboc/vilemov', 142400000100, 3, 3, 50.3022431, 13.3107606, 300);
insert into stationsvariables(st_id, var_id, is_public) values (1299, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1299, 5, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1299, 1, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1299, 16, 1);

insert into stations(st_id, st_seq, st_name, tok, st_name2, st_uri, riv_id, operator_id, division_name, lat, lon, altitude) values (
1300, 99991210, 'Kryry', 'Blšanka', 'poh_3432', 'blsanka/kryry', 142780000100, 3, 3, 50.1715389, 13.4228572, 304);
insert into stationsvariables(st_id, var_id, is_public) values (1300, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1300, 5, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1300, 1, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1300, 16, 1);

insert into stations(st_id, st_seq, st_name, tok, st_name2, st_uri, riv_id, operator_id, division_name, lat, lon, altitude) values (
1301, 99991211, 'Zahrádky', 'Robečský potok', 'poh_3428', 'robecsky_potok/zahradky', 145730000100, 3, 3, 50.6352678, 14.5299367, 250);
insert into stationsvariables(st_id, var_id, is_public) values (1301, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1301, 5, 1);

insert into stations(st_id, st_seq, st_name, tok, st_name2, st_uri, riv_id, operator_id, division_name, lat, lon, altitude) values (
1302, 99991212, 'Brozany', 'Ohře', 'poh_3403', 'ohre/brozany', 139660000100, 3, 3, 50.4571428, 14.1562694, 150);
insert into stationsvariables(st_id, var_id, is_public) values (1302, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1302, 5, 1);


update stations set meteo_code='3431' where st_id=1299;
update stations set meteo_code='3432' where st_id=1300;

update stations set meteo_code='1211' where st_id=488;
insert into stationsvariables(st_id, var_id, is_public) values (488,1 1);

insert into stations(st_id, st_seq, st_name, st_uri, operator_id, division_name, lat, lon, altitude) values (
1303, 99991303, 'Hazlov', 'hazlov', 3, 1, 50.1591061, 12.2689289, 550);
insert into stationsvariables(st_id, var_id, is_public) values (1303, 1, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1303, 16, 1);

insert into stations(st_id, st_seq, st_name, st_uri, operator_id, division_name, lat, lon, altitude) values (
1304, 99991304, 'Dolní Žandov', 'dolni_zandov', 3, 1, 50.0159172, 12.5587822, 557);
insert into stationsvariables(st_id, var_id, is_public) values (1304, 1, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1304, 16, 1);

insert into stations(st_id, st_seq, st_name, st_uri, operator_id, division_name, lat, lon, altitude) values (
1305, 99991305, 'Rovná', 'rovna', 3, 1, 50.1055836, 12.6681567, 720);
insert into stationsvariables(st_id, var_id, is_public) values (1305, 1, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1305, 16, 1);

insert into stations(st_id, st_seq, st_name, st_uri, operator_id, division_name, lat, lon, altitude) values (
1306, 99991306, 'Mnichov', 'mnichov', 3, 1, 50.0384789, 12.7856558, 745);
insert into stationsvariables(st_id, var_id, is_public) values (1306, 1, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1306, 16, 1);

insert into stations(st_id, st_seq, st_name, st_uri, operator_id, division_name, lat, lon, altitude) values (
1307, 99991307, 'Prachomety', 'prachomety', 3, 1, 50.0081597, 12.9487469, 700);
insert into stationsvariables(st_id, var_id, is_public) values (1307, 1, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1307, 16, 1);

insert into stations(st_id, st_seq, st_name, st_uri, operator_id, division_name, lat, lon, altitude) values (
1308, 99991308, 'Dlouhá Lomnice', 'dlouha_lomnice', 3, 1, 50.1583931, 12.9857403, 604);
insert into stationsvariables(st_id, var_id, is_public) values (1308, 1, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1308, 16, 1);

insert into stations(st_id, st_seq, st_name, st_uri, operator_id, division_name, lat, lon, altitude) values (
1309, 99991309, 'Abertamy', 'abertamy', 3, 1, 50.3723264, 12.8175442, 890);
insert into stationsvariables(st_id, var_id, is_public) values (1309, 1, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1309, 16, 1);

insert into stations(st_id, st_seq, st_name, st_uri, operator_id, division_name, lat, lon, altitude) values (
1310, 99991310, 'Stráž nad Ohří', 'straz_nad_ohri', 3, 1, 50.3415186, 13.0583397, 325);
insert into stationsvariables(st_id, var_id, is_public) values (1310, 1, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1310, 16, 1);

insert into stations(st_id, st_seq, st_name, st_uri, operator_id, division_name, lat, lon, altitude) values (
1311, 99991311, 'Hora Svatého Šebestiána', 'hora_sv_sebestiana', 3, 2, 50.5103006, 13.2472242, 845);
insert into stationsvariables(st_id, var_id, is_public) values (1311, 1, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1311, 16, 1);

insert into stations(st_id, st_seq, st_name, st_uri, operator_id, division_name, lat, lon, altitude) values (
1312, 99991312, 'Žichov', 'zichov', 3, 2, 50.4801869, 13.7888044, 371);
insert into stationsvariables(st_id, var_id, is_public) values (1312, 1, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1312, 16, 1);

insert into stations(st_id, st_seq, st_name, st_uri, operator_id, division_name, lat, lon, altitude) values (
1313, 99991313, 'Lukov', 'lukov', 3, 2, 50.5294747, 13.8854064, 520);
insert into stationsvariables(st_id, var_id, is_public) values (1313, 1, 1);
insert into stationsvariables(st_id, var_id, is_public) values (1313, 16, 1);






