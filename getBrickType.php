<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");

$result["brickType"] = SqlDB::SQLGetValues("select * from bricktype");
echo json_encode($result, JSON_UNESCAPED_UNICODE);
