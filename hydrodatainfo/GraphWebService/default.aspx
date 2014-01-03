<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Graph Web Service</title>
<style type="text/css">
label {
width:100px;    /*Or however much space you need for the form’s labels*/
    float:left;
}
input {
	margin-bottom: 5px;
}
</style>
</head>
<body>
<form action="Handler2.ashx" method="get" enctype="application/x-www-form-urlencoded" name="test_form">
<label for="siteCode">siteCode</label>
<input name="siteCode" type="text" value="CHMI-H:123" /><br />
<label for="variableCode">variableCode</label>
<input name="variableCode" type="text" value="CHMI-H:PRUTOK" /><br />
<label for="startDate">startDate</label>
<input name="startDate" type="text" value="2013-05-24" /><br />
<label for="endDate">endDate</label>
<input name="endDate" type="text" value="2013-06-14" /><br />
<label for="serviceURL">serviceURL</label>
<input name="serviceURL" type="text" value="http://hydrodata.info/chmi-h/cuahsi_1_1.asmx" size="100" /><br />
<label for="width">width</label>
<input name="width" type="text" value="600" /><br />
<label for="height">height</label>
<input name="height" type="text" value="400" /><br />
<input name="Submit" type="submit" value="Submit" />
</form>
</body>
</html>
