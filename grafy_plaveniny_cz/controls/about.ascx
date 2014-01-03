<%@ Control Language="C#" AutoEventWireup="true" CodeFile="about.ascx.cs" Inherits="controls_about" %>

<p>
This website displays <b>graphs</b> of <a href="../stations/snow/">snow cover</a>, daily
<a href="../stations/precip/">precipitation</a> and <a href="../stations/water-stage/">water stages</a> 
of rivers and streams from hundreds of locations in the whole Czech Republic.
This information can be useful for skiers, rafters, mushroom pickers and all friends of nature interested in 
remarkable meteorological events from the recent past.
</p>

<h2>Data sources</h2>
<p>Most recent observations of snow cover, precipitation and water stages are published every day on
the <a href="http://www.voda.gov.cz/portal/en/">water management information portal</a> and the
<a href="http://www.chmi.cz/meteo/opss/pocasi">CHMI</a> website. Every 24 hours, a special program is
launched automatically on our server. This program reads the published observation results and saves them
in our database. When a visitor of this web selects a station and an observation period, 
a query is sent to the database and the results are displayed.
</p>