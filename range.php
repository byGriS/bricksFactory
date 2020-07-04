<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
if (!empty($_POST)) {
	//$result = SqlDB::SQLGetValuesC("select count(*) from bricks where dt BETWEEN '".$_POST['dt1']." 00:00:00' and '".$_POST['dt2']." 23:59:59'");
  $result["bricks"] = SqlDB::SQLGetValues("select * from bricks where dt BETWEEN '".$_POST['dt1']." 00:00:00' and '".$_POST['dt2']." 23:59:59'");
  $result["counters"] = SqlDB::SQLGetValues("select * from counters where dt BETWEEN '".$_POST['dt1']." 00:00:00' and '".$_POST['dt2']." 23:59:59'");
  $result["tenzo"] = SqlDB::SQLGetValues("select * from tenzocounter where dt BETWEEN '".$_POST['dt1']." 00:00:00' and '".$_POST['dt2']." 23:59:59'");
  $result["rifey"] = SqlDB::SQLGetValues("select * from rifeycounter where dt BETWEEN '".$_POST['dt1']." 00:00:00' and '".$_POST['dt2']." 23:59:59'");
  echo json_encode($result, JSON_UNESCAPED_UNICODE );
}