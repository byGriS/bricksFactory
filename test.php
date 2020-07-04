<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
$postData = file_get_contents('php://input');
$data = json_decode($postData, true);
if (!empty($data)) {
  if (isset($data['test'])) {
    $command = "update watchdog set test='" . $data['test'] . "' where id = 1";
    SqlDB::SQLExecute($command);
  }
  echo "1";
}
