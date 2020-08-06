<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
if (!empty($_POST)) {
  $command = "update bricktype set title = '".$_POST['title']."',
   countbricks = '".$_POST['countbricks']."' where id = '".$_POST['id']."'";
	SqlDB::SQLExecute($command);	
	echo "1";
}
