<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
if (!empty($_POST)) {
  $command = "update prices set 
  mercuriyAPrice = '".$_POST['mercuriyAPrice']."',
  pulsarPrice = '".$_POST['pulsarPrice']."',
  cementPrice = '".$_POST['cementPrice']."',
  clayPrice = '".$_POST['clayPrice']."',
  sandPrice = '".$_POST['sandPrice']."' where id = 1";
	SqlDB::SQLExecute($command);	
	echo "1";
}
