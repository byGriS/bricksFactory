<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
if (!empty($_POST)) {
$result = SqlDB::SQLGetValues("select * from shiftsdata where dt = '" . $_POST['dt'] . "' and starthour = '" . $_POST['shiftStartHour'] . "';");

echo json_encode($result, JSON_UNESCAPED_UNICODE);
}
