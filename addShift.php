<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
if (!empty($_POST)) {
	$command = "insert into shifts (starthour, startminute, endhour, endminute) values ('".$_POST['shiftStartHour']."', '".$_POST['shiftStartMinute']."', '".$_POST['shiftEndHour']."', '".$_POST['shiftEndMinute']."')";
	SqlDB::SQLExecute($command);	
	echo "1";
}
