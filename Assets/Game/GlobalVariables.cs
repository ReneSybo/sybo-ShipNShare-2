﻿using System.Collections.Generic;
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
		[SerializeField] float _playerStartHealth = 10;
		
		public static Vector3 PlayerPos;
		public static Vector2 EnemySpawnDistances;
		public static List<EnemyController> Enemies = new List<EnemyController>(100);
		public static float AvoidingRange;
		public static float SeperateFactor;
		public static float PlayerHealth;
		public static float PlayerStartHealth;
		
		bool _playing;

		void Awake()
		{
			PlayerHealth = _playerStartHealth;
		}
		
		void Update()
		{
			PlayerStartHealth = _playerStartHealth;
			EnemySpawnDistances = new Vector2(_enemySpawnMin, _enemySpawnMax);
			AvoidingRange = _avoidingRange;
			SeperateFactor = _seperateFactor;
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