using System.Collections.Generic;
using Game.Enemies;
using UnityEngine;

namespace Game.Player
{
	public class GlobalVariables : MonoBehaviour
	{
		[SerializeField] float _enemySpawnMin = 10f;
		[SerializeField] float _enemySpawnMax = 20f;
		[SerializeField] float _avoidingRange = 1f;
		[SerializeField] float _seperateFactor = 2f;
		[SerializeField] float _playerAvoidFactor = 2f;
		[SerializeField] float _playerStartHealth = 10;
		
		[Header("Score")]
		[SerializeField] int _scorePerKill = 10;
		[SerializeField] int _scorePerCoinPickup = 10;
		[SerializeField] int _scorePerUpgrade = 10;
		[SerializeField] int _scorePerCastleSmash = 10;
		[SerializeField] float _scoreLostPerSecond = 10;
		
		
		public static Vector3 PlayerPos;
		public static Vector2 EnemySpawnDistances;
		public static List<EnemyController> Enemies = new List<EnemyController>(100);
		public static float AvoidingRange;
		public static float SeperateFactor;
		public static float PlayerAvoidFactor;
		public static float PlayerHealth;
		public static float PlayerMaxHealth;
		public static float PlayerStartHealth;
		public static int Money;
		public static int Score;
		public static float ScoreLostToTime;
		
		public static int ScorePerKill;
		public static int ScorePerCoinPickup;
		public static int ScorePerUpgrade;
		public static int ScorePerCastleSmash;
		public static float ScoreLostPerSecond;
		
		public static float TotalDamageScale = 1f;
		public static float TotalHealthScale = 1f;
		public static float TotalSpeedScale = 1f;
		public static float TotalAttackSpeedScale = 1f;
		public static bool IsFlexing;
		
		bool _playing;

		void Awake()
		{
			PlayerHealth = _playerStartHealth;
			PlayerMaxHealth = _playerStartHealth;
		}
		
		void Update()
		{
			PlayerStartHealth = _playerStartHealth;
			EnemySpawnDistances = new Vector2(_enemySpawnMin, _enemySpawnMax);
			AvoidingRange = _avoidingRange;
			SeperateFactor = _seperateFactor;
			PlayerAvoidFactor = _playerAvoidFactor;

			ScorePerKill = _scorePerKill;
			ScorePerCoinPickup = _scorePerCoinPickup;
			ScorePerUpgrade = _scorePerUpgrade;
			ScorePerCastleSmash = _scorePerCastleSmash;
			ScoreLostPerSecond = _scoreLostPerSecond;
		}
		
		void OnDrawGizmos()
		{
			Vector3 myPos = transform.localPosition;
			
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(myPos, _enemySpawnMin);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(myPos, _enemySpawnMax);
		}
	}
}