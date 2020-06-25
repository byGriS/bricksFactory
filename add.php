<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
$postData = file_get_contents('php://input');
$data = json_decode($postData, true);
if (!empty($data)) {
  if (isset($data['counter'])) {
    $command = "insert into bricks (dt, counter) values ('" . $data['dt'] . "', '" . $data['counter'] . "')";
    SqlDB::SQLExecute($command);
  }
  if (isset($data['pulsar'])) {
    $command = "insert into counters (dt, mercuriyA, mercuriyR, pulsar) values ('" . $data['dt'] . "', '" . $data['mercuriyA'] . "', '" . $data['mercuriyR'] . "', '" . $data['pulsar'] . "')";
    SqlDB::SQLExecute($command);
  }
  if (isset($data['rifey'])) {
    $command = "insert into rifeycounter (dt, rifey) values ('" . $data['dt'] . "', '" . $data['rifey'] . "')";
    SqlDB::SQLExecute($command);
  }
  if (isset($data['tenzo'])) {
    $command = "insert into tenzocounter (dt, tenzo) values ('" . $data['dt'] . "', '" . $data['tenzo'] . "')";
    SqlDB::SQLExecute($command);
  }
  echo "1";
}
