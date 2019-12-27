<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
if (!empty($_POST)) {
	$result = SqlDB::SQLGetValuesC("select count(*) from bricks where dt BETWEEN '".$_POST['dt1']." 00:00:00' and '".$_POST['dt2']." 23:59:59'");
	echo $result[0];
}