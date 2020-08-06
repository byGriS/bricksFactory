<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
if (!empty($_POST)) {
	$command = "insert into bricktype (title, countBricks) values ('".$_POST['title']."', '".$_POST['countBricks']."')";
	SqlDB::SQLExecute($command);	
	echo "1";
}
