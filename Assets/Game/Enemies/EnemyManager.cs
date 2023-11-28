using System;
using Game.Events;
using UnityEngine;

namespace Game.Enemies
{
	public class EnemyManager : MonoBehaviour
	{
		public EnemySpawnPool EnemyPool;
		public int SpawnAmount = 100;
		public bool AutoRespawn = false;
		
		void Awake()
		{
			GameEvents.EnemyDied.AddListener(OnEnemyDied);
		}

		void Start()
		{
			for (int i = 0; i < SpawnAmount; i++)
			{
				EnemyPool.SpawnItem();
			}
		}

		void Update()
		{
			if (Input.GetKey(KeyCode.Space))
			{
				EnemyPool.SpawnItem();
			}
		}

		void OnEnemyDied(EnemyController enemy)
		{
			EnemyPool.ReturnEntity(enemy);
			
			if (AutoRespawn)
			{
				EnemyPool.SpawnItem();
			}
		}
	}
}