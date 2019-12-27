<?php
class SqlDB
{
	private static $dbHost = 'localhost';
	private static $dbDataBase = 'c98744oh_brick';
	private static $dbUserName = 'c98744oh_brick';
	private static $password = 'c98744oh_brick_2019';
	/*private static $dbDataBase = 'bricks';
	private static $dbUserName = 'root';
	private static $password = '';*/
	private static $pdo;

	public static function SQLExecute($query)
	{
		if (SqlDB::$pdo === null) {
			$pdo = SqlDB::Connect();
		}
		$result = $pdo->query($query);
		return $result;
	}
	public static function SQLGetValues($query)
	{
		if (SqlDB::$pdo === null) {
			$pdo = SqlDB::Connect();
		}
		$result = $pdo->query($query)->fetchAll();
		return $result;
	}
	public static function SQLGetValuesC($query)
	{
		if (SqlDB::$pdo === null) {
			$pdo = SqlDB::Connect();
		}
		$result = $pdo->query($query)->fetchAll(PDO::FETCH_COLUMN);
		return $result;
	}

	private static function Connect()
	{
		$opt = array(
			PDO::ATTR_ERRMODE							=> PDO::ERRMODE_EXCEPTION,
			PDO::ATTR_DEFAULT_FETCH_MODE	=> PDO::FETCH_ASSOC
		);
		$pdo = new PDO(
			"mysql:host=" . SqlDB::$dbHost . ";dbname=" . SqlDB::$dbDataBase,
			SqlDB::$dbUserName,
			SqlDB::$password,
			$opt
		);
		return $pdo;
	}
}
