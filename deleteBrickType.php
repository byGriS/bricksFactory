<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
if (!empty($_POST)) {
  $command = "delete from bricktype where id = '".$_POST['id']."'";
	SqlDB::SQLExecute($command);	
	echo "1";
}
