<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
$postData = file_get_contents('php://input');
$data = json_decode($postData, true);
if (!empty($data)) {
	if (isset($data['counter'])){
		$command = "insert into bricks (dt, counter) values ('".$data['dt']."', '".$data['counter']."')";
	}else{
		$command = "insert into bricks (dt) values ('".$data['dt']."')";
	}
	SqlDB::SQLExecute($command);	
echo "1";
}
