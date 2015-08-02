use plaveninycz1;
SELECT s.st_id
FROM plaveninycz.stations s
INNER JOIN plaveninycz.stationsvariables p1 ON s.st_id = p1.st_id
WHERE p1.var_id IN (1,2,8,16)
GROUP BY s.st_id
HAVING COUNT(*) = 4