<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
if (!empty($_POST)) {
  $result = SqlDB::SQLGetValuesC("select password from admindata where id = 1")[0];
  if ($_POST['password'] == $result)
    echo 1;
  else
    echo 0;
}
