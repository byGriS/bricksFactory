<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");

$result["prices"] = SqlDB::SQLGetValues("select * from prices");
echo json_encode($result, JSON_UNESCAPED_UNICODE);
