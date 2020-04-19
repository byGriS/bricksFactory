<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
if (!empty($_POST)) {
  $command = "update shifts set starthour = '".$_POST['shiftStartHour']."',
   startminute = '".$_POST['shiftStartMinute']."', 
   endhour = '".$_POST['shiftEndHour']."', 
   endminute = '".$_POST['shiftEndMinute']."' where id = '".$_POST['id']."'";
	SqlDB::SQLExecute($command);	
	echo "1";
}
