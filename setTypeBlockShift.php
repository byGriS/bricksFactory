<?php
include "db.php";
header("Access-Control-Allow-Origin: *");
header("Access-Control-Allow-Methods: POST,GET");
header("Access-Control-Allow-Headers: *");
if (!empty($_POST)) {
  $result = SqlDB::SQLGetValues("select * from shiftsdata where dt = '".$_POST['dt'] ."' and starthour = '".$_POST['shiftStartHour']."';");
  if (count($result) == 0){
    echo "1";
    $command = "insert into shiftsdata (dt, starthour, startminute, endhour, endminute, bricktitle, brickcount) values 
    ('".$_POST['dt']."','".$_POST['shiftStartHour']."', '".$_POST['shiftStartMinute']."', '".$_POST['shiftEndHour']."', '".$_POST['shiftEndMinute']."', '".$_POST['brickTitle']."', '".$_POST['brickCount']."')";
    SqlDB::SQLExecute($command);
  }else{
    echo "2";
    $command = "update shiftsdata set 
    starthour = '".$_POST['shiftStartHour']."',
    startminute = '".$_POST['shiftStartMinute']."',
    endhour = '".$_POST['shiftEndHour']."',
    endminute = '".$_POST['shiftEndMinute']."',
    bricktitle =  '".$_POST['brickTitle']."',
    brickcount = '".$_POST['brickCount']."' where dt = '".$_POST['dt']."' and starthour = '".$_POST['shiftStartHour']."';"; 
    SqlDB::SQLExecute($command);
  }
}
