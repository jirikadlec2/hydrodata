update stations set division_name=3, operator_id=2 where st_id in (
750,802,4249,94,458,906,129,560,752,433,751,123,283,286,559,558,709,457,196,708,282,556,589,908,909,166,285,755,714,456,753,907,128,195,593,455,707,170,921,
711,919,920,918,754,917,139,1287,916,1288,278,712,591,592,915,432,198,912,212,710,203,914,913,804,147,144,197,706,431,910,705,911,280,557,590);

update stations set division_name=2, operator_id=2 where st_id in (
130,716,965,964,963,1014,89,960,961,803,904,174,966,160,967,968,715,1284,452,459,769,451,565,594,566,970,453,1286,768,972,971,454,969,567,742,973,92,
460,568,974,151,1285,450,217,976,826,975,713);

update stations set division_name=2, operator_id=2 where st_id in (1012,209,268,434,959,564,267,266);

update stations set division_name=1, operator_id=2 where st_id in (435,681,138,757,756,945,90,116,947,436,684,948,1289,758,437,438,262,183,563,683,682,91,805,
446,824,145,956,447,1290,821,957,825,955,823,954,163,822,763,1291,764,1292,1293,448,765,766,261,1261,1260,181,158,949,951,441,953,762,952,213,1081,561,141,760,
442,685,443,686,759,761,444,107,125,445,950,169,994,127,562,819,159,946,173,439,440,820,142);

update stations set division_name=1, operator_id=2 where st_id in (146, 958);
update stations set st_name='Koloměřice', st_uri='bilinsky/kolomerice' where st_id=958;
update stations set division_name=2, operator_id=2 where st_id=1249;

update stations set tok='Kocába', st_name2='pvl_kodd', riv_id=124430000100 where st_id=1012;
update stations set tok='Dobřejovický potok', st_name2='pvl_dppr', riv_id=137660000100 where st_id=1014;
update stations set tok='Malše', st_name2='pvl_mapo', riv_id=115500000100 where st_id=1081;
update stations set tok='Loděnice', st_name2='pvl_ldmk', riv_id=137070000100 where st_id=1249;
update stations set tok='Bezdrevský potok', st_name2='pvl_bpne', riv_id=116380000100 where st_id=1260;
update stations set tok='Bezdrevský potok', st_name2='pvl_bpll', riv_id=116380000100 where st_id=1261;
update stations set tok='Břevnický potok', st_name2='pvl_bvbv', riv_id=125100000100 where st_id=1285;
update stations set tok='Chotýšanka', st_name2='pvl_chsr', riv_id=127970000100 where st_id=1284;
update stations set tok='Trnava', st_name2='pvl_trho', riv_id=126470000100 where st_id=1286;
update stations set tok='Poleňka', st_name2='pvl_posl', riv_id=132620000100 where st_id=1287;
update stations set tok='Chodská Úhlava', st_name2='pvl_cuha', riv_id=132260000100 where st_id=1288;
update stations set tok='Blanice', st_name2='pvl_blhs', riv_id=121890000100 where st_id=1289;
update stations set tok='Kamenice', st_name2='pvl_kaka', riv_id=117550000100 where st_id=1290;
update stations set tok='Dračice', st_name2='pvl_drgo', riv_id=117100000100 where st_id=1291;
update stations set tok='Skřemelice', st_name2='pvl_skal', riv_id=116940000100 where st_id=1292;
update stations set tok='Romavský potok', st_name2='pvl_rbhr', riv_id=116960000100 where st_id=1293;

update stations set st_name2='pvl_klnh' where st_id=166;


update stations set st_name2='pvl_lilz', tok='Litavka', st_uri='litavka/laz', riv_id=136510000100 where st_id=283;
insert into stationsvariables(st_id, var_id, is_public) values (283, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (283, 5, 1);

update stations set st_name2='pvl_kckl', tok='Klíčava', st_uri='klicava/vd_klicava', riv_id=136310000100 where st_id=286;
insert into stationsvariables(st_id, var_id, is_public) values (286, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (286, 5, 1);

update stations set st_name2='pvl_stzc', tok='Střela', st_uri='strela/vd_zlutice', riv_id=134330000100 where st_id=282;
insert into stationsvariables(st_id, var_id, is_public) values (282, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (282, 5, 1);

update stations set st_name2='pvl_klkb', tok='Klabava', st_uri='klabava/vd_klabava', riv_id=133740000100 where st_id=285;
insert into stationsvariables(st_id, var_id, is_public) values (285, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (285, 5, 1);

update stations set st_name2='pvl_uhsl', tok='Úhlava', st_uri='uhlava/vd_nyrsko', riv_id=132140000100 where st_id=278;
insert into stationsvariables(st_id, var_id, is_public) values (278, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (278, 5, 1);

update stations set st_name2='pvl_mzlc', tok='Mže', st_uri='mze/vd_lucina', riv_id=129120000100 where st_id=280;
insert into stationsvariables(st_id, var_id, is_public) values (280, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (280, 5, 1);

update stations set st_name2='pvl_vlsy', tok='Vltava', st_uri='vltava/vd_slapy', riv_id=113900000100 where st_id=268;
insert into stationsvariables(st_id, var_id, is_public) values (268, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (268, 5, 1);

update stations set st_name2='pvl_vlol', tok='Vltava', st_uri='vltava/vd_orlik', riv_id=113900000100 where st_id=266;
insert into stationsvariables(st_id, var_id, is_public) values (266, 4, 1);
insert into stationsvariables(st_id, var_id, is_public) values (266, 5, 1);


update stations set st_name2=NULL, division_name=3, meteo_code='BES6' where st_id=691;
update stations set st_name2=NULL, division_name=3, meteo_code='CPZA' where st_id=288;
update stations set st_name2=NULL, division_name=3, meteo_code='OPOB' where st_id=287;
update stations set st_name2=NULL, division_name=3, meteo_code='PPPI' where st_id=284;
update stations set st_name2=NULL, division_name=3, meteo_code='STRA' where st_id=687;
update stations set st_name2=NULL, division_name=3, meteo_code='UHNY' where st_id=278;
update stations set st_name2=NULL, division_name=3, meteo_code='BESP' where st_id=689;
update stations set st_name2=NULL, division_name=3, meteo_code='RACD' where st_id=279;
update stations set st_name2=NULL, division_name=3, meteo_code='LPLE' where st_id=1092;

update stations set st_name2=NULL, division_name=2, meteo_code='ZESV' where st_id=277;
update stations set st_name2=NULL, division_name=2, meteo_code='SVSV' where st_id=271;
update stations set st_name2=NULL, division_name=2, meteo_code='SPNE' where st_id=276;
update stations set st_name2=NULL, division_name=2, meteo_code='KPKS' where st_id=704;
update stations set st_name2=NULL, division_name=2, meteo_code='LPLU' where st_id=698;
update stations set st_name2=NULL, division_name=2, meteo_code='TRTR' where st_id=275;
update stations set st_name2=NULL, division_name=2, meteo_code='KPSA' where st_id=1089;
update stations set st_name2=NULL, division_name=2, meteo_code='TRVO' where st_id=699;
update stations set st_name2=NULL, division_name=2, meteo_code='VPVI' where st_id=700;
update stations set st_name2=NULL, division_name=2, meteo_code='HESE' where st_id=274;
update stations set st_name2=NULL, division_name=2, meteo_code='HPUS' where st_id=1090;
update stations set st_name2=NULL, division_name=2, meteo_code='DVKR' where st_id=1088;
update stations set st_name2=NULL, division_name=2, meteo_code='HEBO' where st_id=701;

update stations set st_name2=NULL, division_name=1, meteo_code='METR' where st_id=1086;
update stations set st_name2=NULL, division_name=1, meteo_code='HVSH' where st_id=1084;
update stations set st_name2=NULL, division_name=1, meteo_code='BLSP' where st_id=1083;
update stations set st_name2=NULL, division_name=1, meteo_code='BLAR' where st_id=1072;
update stations set st_name2=NULL, division_name=1, meteo_code='DRHA' where st_id=1074;
update stations set st_name2=NULL, division_name=1, meteo_code='DYLI' where st_id=1077;
update stations set st_name2=NULL, division_name=1, meteo_code='MARI' where st_id=263;
update stations set st_name2=NULL, division_name=1, meteo_code='CRSO' where st_id=695;
update stations set st_name2=NULL, division_name=1, meteo_code='HVDD' where st_id=1073;
update stations set st_name2=NULL, division_name=1, meteo_code='LUKS' where st_id=1075;


update stations set meteo_code='LILA' where st_id=283;
update stations set meteo_code='STZC' where st_id=282;
update stations set meteo_code='KCKL' where st_id=286;
update stations set meteo_code='KLKB' where st_id=285;
update stations set meteo_code='KPST' where st_id=431;
insert into stationsvariables(st_id, var_id, is_public) values (431,1,1);
update stations set meteo_code='MZLC' where st_id=280;
update stations set meteo_code='VLSL', division_name=2 where st_id=268;
update stations set meteo_code='VLOR', division_name=2 where st_id=266;
update stations set meteo_code='BLRA', division_name=2 where st_id=459;
insert into stationsvariables(st_id, var_id, is_public) values (459,1,1);
update stations set meteo_code='SAZR', division_name=2 where st_id=92;
insert into stationsvariables(st_id, var_id, is_public) values (92,1,1);


update stations set meteo_code='BELI' where st_id=457;
update stationsvariables set st_id=457 where st_id=663;
update rain_daily set station_id=457 where station_id=663;
update rain_hourly set station_id=457 where station_id=663;
update temperature set station_id=457 where station_id=663;
delete from stations where st_id=663;

update stations set meteo_code='STPL' where st_id=196;
update stationsvariables set st_id=196 where st_id=659;
update rain_daily set station_id=196 where station_id=659;
update rain_hourly set station_id=196 where station_id=659;
update temperature set station_id=196 where station_id=659;
delete from stations where st_id=659;

update stations set meteo_code='STCI' where st_id=708;
update stationsvariables set st_id=708 where st_id=743;
update rain_daily set station_id=708 where station_id=743;
update rain_hourly set station_id=708 where station_id=743;
update temperature set station_id=708 where station_id=743;
delete from stations where st_id=743;

update stations set meteo_code='KLST' where st_id=753;
update stationsvariables set st_id=753 where st_id=1091;
update rain_daily set station_id=753 where station_id=1091;
update rain_hourly set station_id=753 where station_id=1091;
update temperature set station_id=753 where station_id=1091;
delete from stations where st_id=1091;

update stations set meteo_code='UHJI' where st_id=711;
update stationsvariables set st_id=711 where st_id=690;
update rain_daily set station_id=711 where station_id=690;
update rain_hourly set station_id=711 where station_id=690;
update temperature set station_id=711 where station_id=690;
delete from stations where st_id=690;

update stations set meteo_code='UHKL' where st_id=139;
update stationsvariables set st_id=139 where st_id=654;
update rain_daily set station_id=139 where station_id=654;
update rain_hourly set station_id=139 where station_id=654;
update temperature set station_id=139 where station_id=654;
delete from stations where st_id=654;

update stations set meteo_code='RALH' where st_id=198;
update stationsvariables set st_id=198 where st_id=661;
update rain_daily set station_id=198 where station_id=661;
update rain_hourly set station_id=198 where station_id=661;
update temperature set station_id=198 where station_id=661;
delete from stations where st_id=661;

update stations set meteo_code='ZUDO' where st_id=710;
update stationsvariables set st_id=710 where st_id=688;
update rain_daily set station_id=710 where station_id=688;
update rain_hourly set station_id=710 where station_id=688;
update temperature set station_id=710 where station_id=688;
delete from stations where st_id=688;

update stations set meteo_code='RATA' where st_id=203;
update stationsvariables set st_id=203 where st_id=646;
update rain_daily set station_id=203 where station_id=646;
update rain_hourly set station_id=203 where station_id=646;
update temperature set station_id=203 where station_id=646;
delete from stations where st_id=646;

update stations set meteo_code='MZHR' where st_id=147;
update stationsvariables set st_id=147 where st_id=281;
update rain_daily set station_id=147 where station_id=281;
update rain_hourly set station_id=147 where station_id=281;
update temperature set station_id=147 where station_id=281;
delete from stations where st_id=281;

update stations set meteo_code='UPTR' where st_id=144;
update stationsvariables set st_id=144 where st_id=658;
update rain_daily set station_id=144 where station_id=658;
update rain_hourly set station_id=144 where station_id=658;
update temperature set station_id=144 where station_id=658;
delete from stations where st_id=658;

update stations set meteo_code='MZST' where st_id=197;
update stationsvariables set st_id=197 where st_id=692;
update rain_daily set station_id=197 where station_id=692;
update rain_hourly set station_id=197 where station_id=692;
update temperature set station_id=197 where station_id=692;
delete from stations where st_id=692;

update stations set meteo_code='BPBL' where st_id=594;
update stationsvariables set st_id=594 where st_id=703;
update rain_daily set station_id=594 where station_id=703;
update rain_hourly set station_id=594 where station_id=703;
update temperature set station_id=594 where station_id=703;
delete from stations where st_id=703;

update stations set meteo_code='BERA' where st_id=567;
update stationsvariables set st_id=567 where st_id=702;
update rain_daily set station_id=567 where station_id=702;
update rain_hourly set station_id=567 where station_id=702;
update temperature set station_id=567 where station_id=702;
delete from stations where st_id=702;

update stations set meteo_code='SKVA' where st_id=435;
update stationsvariables set st_id=435 where st_id=735;
update rain_daily set station_id=435 where station_id=735;
update rain_hourly set station_id=435 where station_id=735;
update temperature set station_id=435 where station_id=735;
delete from stations where st_id=735;

update stations set meteo_code='OTPI' where st_id=90;
update stationsvariables set st_id=90 where st_id=1080;
update rain_daily set station_id=90 where station_id=1080;
update rain_hourly set station_id=90 where station_id=1080;
update temperature set station_id=90 where station_id=1080;
delete from stations where st_id=1080;

update stations set meteo_code='BLHE' where st_id=116;
update stationsvariables set st_id=116 where st_id=734;
update rain_daily set station_id=116 where station_id=734;
update rain_hourly set station_id=116 where station_id=734;
update temperature set station_id=116 where station_id=734;
delete from stations where st_id=734;

update stations set meteo_code='BLHU' where st_id=1289;
update stationsvariables set st_id=1289 where st_id=265;
update rain_daily set station_id=1289 where station_id=265;
update rain_hourly set station_id=1289 where station_id=265;
update temperature set station_id=1289 where station_id=265;
delete from stations where st_id=265;

update stations set meteo_code='BLBM' where st_id=438;
update stationsvariables set st_id=438 where st_id=733;
update rain_daily set station_id=438 where station_id=733;
update rain_hourly set station_id=438 where station_id=733;
update temperature set station_id=438 where station_id=733;
delete from stations where st_id=733;

update stations set meteo_code='VONE' where st_id=127;
update stationsvariables set st_id=127 where st_id=732;
update rain_daily set station_id=127 where station_id=732;
update rain_hourly set station_id=127 where station_id=732;
update temperature set station_id=127 where station_id=732;
delete from stations where st_id=732;

update stations set meteo_code='OTKA' where st_id=159;
update stationsvariables set st_id=159 where st_id=731;
update rain_daily set station_id=159 where station_id=731;
update rain_hourly set station_id=159 where station_id=731;
update temperature set station_id=159 where station_id=731;
delete from stations where st_id=731;

update stations set meteo_code='OSKO' where st_id=173;
update stationsvariables set st_id=173 where st_id=728;
update rain_daily set station_id=173 where station_id=728;
update rain_hourly set station_id=173 where station_id=728;
update temperature set station_id=173 where station_id=728;
delete from stations where st_id=728;

update stations set meteo_code='KRST' where st_id=440;
update stationsvariables set st_id=440 where st_id=729;
update rain_daily set station_id=440 where station_id=729;
update rain_hourly set station_id=440 where station_id=729;
update temperature set station_id=440 where station_id=729;
delete from stations where st_id=729;

update stations set meteo_code='VYMO' where st_id=142;
update stationsvariables set st_id=142 where st_id=730;
update rain_daily set station_id=142 where station_id=730;
update rain_hourly set station_id=142 where station_id=730;
update temperature set station_id=142 where station_id=730;
delete from stations where st_id=730;

update stations set meteo_code='LUBE' where st_id=183;
update stationsvariables set st_id=183 where st_id=736;
update rain_daily set station_id=183 where station_id=736;
update rain_hourly set station_id=183 where station_id=736;
update temperature set station_id=183 where station_id=736;
delete from stations where st_id=736;

update stations set meteo_code='LUKL' where st_id=91;
update stationsvariables set st_id=91 where st_id=737;
update rain_daily set station_id=91 where station_id=737;
update rain_hourly set station_id=91 where station_id=737;
update temperature set station_id=91 where station_id=737;
delete from stations where st_id=737;

update stations set meteo_code='CPTU' where st_id=805;
update stationsvariables set st_id=805 where st_id=1085;
update rain_daily set station_id=805 where station_id=1085;
update rain_hourly set station_id=805 where station_id=1085;
update temperature set station_id=805 where station_id=1085;
delete from stations where st_id=1085;

update stations set meteo_code='NERO' where st_id=447;
update stationsvariables set st_id=447 where st_id=738;
update rain_daily set station_id=447 where station_id=738;
update rain_hourly set station_id=447 where station_id=738;
update temperature set station_id=447 where station_id=738;
delete from stations where st_id=738;

update stations set meteo_code='SPHP' where st_id=957;
update stationsvariables set st_id=957 where st_id=739;
update rain_daily set station_id=957 where station_id=739;
update rain_hourly set station_id=957 where station_id=739;
update temperature set station_id=957 where station_id=739;
delete from stations where st_id=739;

update stations set meteo_code='ZSPI' where st_id=954;
update stationsvariables set st_id=954 where st_id=1079;
update rain_daily set station_id=954 where station_id=1079;
update rain_hourly set station_id=954 where station_id=1079;
update temperature set station_id=954 where station_id=1079;
delete from stations where st_id=1079;

update stations set meteo_code='LUNV' where st_id=448;
update stationsvariables set st_id=448 where st_id=697;
update rain_daily set station_id=448 where station_id=697;
update rain_hourly set station_id=448 where station_id=697;
update temperature set station_id=448 where station_id=697;
delete from stations where st_id=697;

update stations set meteo_code='SCHU' where st_id=953;
update stationsvariables set st_id=953 where st_id=264;
update rain_daily set station_id=953 where station_id=264;
update rain_hourly set station_id=953 where station_id=264;
update temperature set station_id=953 where station_id=264;
delete from stations where st_id=264;

update stations set meteo_code='SCHS' where st_id=952;
update stationsvariables set st_id=952 where st_id=696;
update rain_daily set station_id=952 where station_id=696;
update rain_hourly set station_id=952 where station_id=696;
update temperature set station_id=952 where station_id=696;
delete from stations where st_id=696;

update stations set meteo_code='MAPO' where st_id=1081;
update stationsvariables set st_id=1081 where st_id=1082;
update rain_daily set station_id=1081 where station_id=1082;
update rain_hourly set station_id=1081 where station_id=1082;
update temperature set station_id=1081 where station_id=1082;
delete from stations where st_id=1082;

update stations set meteo_code='CRLC' where st_id=141;
update stationsvariables set st_id=141 where st_id=1076;
update rain_daily set station_id=141 where station_id=1076;
update rain_hourly set station_id=141 where station_id=1076;
update temperature set station_id=141 where station_id=1076;
delete from stations where st_id=1076;

update stations set meteo_code='KPBR' where st_id=685;
update stationsvariables set st_id=685 where st_id=694;
update rain_daily set station_id=685 where station_id=694;
update rain_hourly set station_id=685 where station_id=694;
update temperature set station_id=685 where station_id=694;
delete from stations where st_id=694;

update stations set meteo_code='PONO' where st_id=759;
update stationsvariables set st_id=759 where st_id=1078;
update rain_daily set station_id=759 where station_id=1078;
update rain_hourly set station_id=759 where station_id=1078;
update temperature set station_id=759 where station_id=1078;
delete from stations where st_id=1078;

update stations set meteo_code='VLZA' where st_id=444;
update stationsvariables set st_id=444 where st_id=1087;
update rain_daily set station_id=444 where station_id=1087;
update rain_hourly set station_id=444 where station_id=1087;
update temperature set station_id=444 where station_id=1087;
delete from stations where st_id=1087;

update stations set meteo_code='SVCK' where st_id=445;
update stationsvariables set st_id=445 where st_id=693;
update rain_daily set station_id=445 where station_id=693;
update rain_hourly set station_id=445 where station_id=693;
update temperature set station_id=445 where station_id=693;
delete from stations where st_id=693;

update stations set meteo_code='TVLE' where st_id=169;
update stationsvariables set st_id=169 where st_id=740;
update rain_daily set station_id=169 where station_id=740;
update rain_hourly set station_id=169 where station_id=740;
update temperature set station_id=169 where station_id=740;
delete from stations where st_id=740;

update stations set operator_id=1 where st_id=994;








