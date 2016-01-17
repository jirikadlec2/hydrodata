select st.st_id, st.st_name, st.st_name2, st.st_name3 
from plaveninycz.stations st 
inner join plaveninycz.stationsvariables stv on st.st_id = stv.st_id 
where stv.var_id = 8 
order by st_name

update plaveninycz.stations set st_name3='Brno/Tuřany' WHERE st_id = 82;
update plaveninycz.stations set st_name3='Čáslav' WHERE st_id = 19;
update plaveninycz.stations set st_name3='Červená u Libavé' WHERE st_id = 53;
update plaveninycz.stations set st_name='České Budějovice', st_name3='České Budějovice' WHERE st_id = 41;
update plaveninycz.stations set st_name3='Doksany' WHERE st_id = 22;
update plaveninycz.stations set st_name3='Dukovany' WHERE st_id = 80;
update plaveninycz.stations set st_name3='Holešov' WHERE st_id = 233;
update plaveninycz.stations set st_name3='Cheb' WHERE st_id = 49;
update plaveninycz.stations set st_name3='Churáňov' WHERE st_id = 23;
update plaveninycz.stations set st_name3='Karlovy Vary' WHERE st_id = 48;
update plaveninycz.stations set st_name3='Kocelovice' WHERE st_id = 33;
update plaveninycz.stations set st_name3='Kostelní Myslová' WHERE st_id = 76;
update plaveninycz.stations set st_name3='Košetice' WHERE st_id = 232;
update plaveninycz.stations set st_name3='Kuchařovice' WHERE st_id = 81;
update plaveninycz.stations set st_name3='Liberec' WHERE st_id = 231;
update plaveninycz.stations set st_name3='Luká' WHERE st_id = 63;
update plaveninycz.stations set st_name3='Lysá hora' WHERE st_id = 52;
update plaveninycz.stations set st_name3='Milešovka' WHERE st_id = 47;
update plaveninycz.stations set st_name3='Ostrava/Mošnov' WHERE st_id = 60;
update plaveninycz.stations set st_name3='Pardubice' WHERE st_id = 20;
update plaveninycz.stations set st_name3='Pec pod Sněžkou' WHERE st_id = 2;
update plaveninycz.stations set st_name3='Plzeň-Mikulka' WHERE st_id = 253;
update plaveninycz.stations set st_name3='Praha-Libuš' WHERE st_id = 45;
update plaveninycz.stations set st_name3='Praha/Ruzyně' WHERE st_id = 42;
update plaveninycz.stations set st_name3='Přibyslav' WHERE st_id = 30;
update plaveninycz.stations set st_name3='Přimda' WHERE st_id = 24;
update plaveninycz.stations set st_name3='Svratouch' WHERE st_id = 3;
update plaveninycz.stations set st_name3='Šerák' WHERE st_id = 223;
update plaveninycz.stations set st_name3='Temelín' WHERE st_id = 34;
update plaveninycz.stations set st_name3='Tušimice' WHERE st_id = 51;
update plaveninycz.stations set st_name3='Ústí nad Labem' WHERE st_id = 10;
update plaveninycz.stations set st_name3='Ústí nad Orlicí' WHERE st_id = 9;

insert into plaveninycz.stationsvariables(st_id, var_id) values (528, 8);
update plaveninycz.stations set st_name3='Polom' WHERE st_id = 528;

insert into plaveninycz.stationsvariables(st_id, var_id) values (46, 8);
update plaveninycz.stations set st_name3='Praha/Kbely' WHERE st_id = 46;

insert into plaveninycz.stationsvariables(st_id, var_id) values(849, 8);
update plaveninycz.stations set st_name3='Hošťálková-Maruška' WHERE st_id = 849;
update plaveninycz.stations set st_name2='HOSTALKOVA MARUSKA' WHERE st_id = 849;

insert into plaveninycz.stationsvariables(st_id, var_id) values (17, 8);
update plaveninycz.stations set st_name3='Kopisty' WHERE st_id = 17;

insert into plaveninycz.stationsvariables(st_id, var_id) values (1163, 8);
update plaveninycz.stations set st_name='Luisino Údolí' WHERE st_id = 1163;
update plaveninycz.stations set st_name2='LUISINO UDOLI' WHERE st_id = 1163;

insert into plaveninycz.stationsvariables(st_id, var_id) values (78, 8);
update plaveninycz.stations set st_name3='Náměšť nad Oslavou' WHERE st_id = 78;
update plaveninycz.stations set st_name='Náměšť nad Oslavou - Sedlec' WHERE st_id = 78;

update plaveninycz.stations set st_name='Praha - Ruzyně' WHERE st_id = 42;
update plaveninycz.stations set st_name='Praha - Libuš' WHERE st_id = 45;
update plaveninycz.stations set st_name='Praha - Kbely' WHERE st_id = 46;
update plaveninycz.stations set st_name='Ústí nad Orlicí' WHERE st_id = 9;
update plaveninycz.stations set st_name='Ústí nad Labem - Kočkov' WHERE st_id = 10;
update plaveninycz.stations set st_name='Ústí nad Orlicí' WHERE st_id = 9;
update plaveninycz.stations set st_name='Deštné v Orlických. horách' WHERE st_id = 4;
update plaveninycz.stations set st_name='Rokytnice v Orlických. horách' WHERE st_id = 5;
update plaveninycz.stations set st_name2='SINDELOVA-OBORA' WHERE st_id = 533;

insert into plaveninycz.stations(st_id, st_seq, st_name, altitude, st_name2, operator_id, lat, lon) values (1314, 1314, 'Velké Karlovice - Benešky', 855, 'VELKE KARLOVICE', 1, 49.3977644, 18.3185139);
insert into plaveninycz.stations(st_id, st_seq, st_name, altitude, st_name2, operator_id, lat, lon) values (1315, 1315, 'Plechý', 1344, 'PLECHY', 1, 48.7705383, 13.8523139);
insert into plaveninycz.stations(st_id, st_seq, st_name, altitude, st_name3, operator_id, lat, lon) values (1316, 1316, 'Prostějov', 214, 'Prostějov', 1, 49.4525944, 17.1347800);
insert into plaveninycz.stationsvariables(st_id, var_id) values (1314, 8);
insert into plaveninycz.stationsvariables(st_id, var_id) values (1315, 8);
insert into plaveninycz.stationsvariables(st_id, var_id) values (1316, 8);
