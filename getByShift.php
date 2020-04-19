<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
if (!empty($_POST)) {
	$result = SqlDB::SQLGetValues("select * from bricks where dt BETWEEN '".$_POST['dt']." ".$_POST['starthour'].":".$_POST['startminute'].":00' and '".$_POST['dt']." ".$_POST['endhour'].":".$_POST['endminute'].":59'");
	echo json_encode($result, JSON_UNESCAPED_UNICODE );
}