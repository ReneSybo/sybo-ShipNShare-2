using System;
using Game.Difficulty;
using Game.Events;
using Game.Player;
using UnityEngine;

namespace Game.Enemies
{
	public class EnemyManager : MonoBehaviour
	{
		public DifficultyConfig[] Configs;
		
		public EnemySpawnPool EnemyPool;

		int _currentConfigIndex;
		EnemyRound _currentRound;
		
		void Awake()
		{
			GameEvents.EnemyDied.AddListener(OnEnemyDied);
			GameEvents.EnemySpawned.AddListener(OnEnemySpawned);
			GameEvents.GameEnded.AddListener(OnGameEnded);
			GameEvents.RoundStarted.AddListener(OnRoundStarted);
			GameEvents.CastleTrashed.AddListener(OnRoundStarted);

			enabled = false;
			_currentConfigIndex = -1;
		}

		void OnRoundStarted()
		{
			enabled = true;
			StartNextRound();
		}

		void OnGameEnded()
		{
			enabled = false;

			EnemyController[] enemies = GlobalVariables.Enemies.ToArray();
			GlobalVariables.Enemies.Clear();
			
			foreach (EnemyController enemyController in enemies)
			{
				EnemyPool.ReturnEntity(enemyController);
			}

			_currentConfigIndex = -1;
		}

		void OnEnemySpawned(EnemyConfig config)
		{
			EnemyController enemy = EnemyPool.SpawnItem();
			enemy.ApplyConfig(config);
		}

		void StartNextRound()
		{
			if (_currentConfigIndex < Configs.Length - 1)
			{
				_currentConfigIndex++;
			}

			_currentRound = new EnemyRound(Configs[_currentConfigIndex]);
		}

		void Update()
		{
			if (_currentRound != null)
			{
				_currentRound.Update();
				if (GlobalVariables.Enemies.Count == 0 && _currentRound.Completed())
				{
					GameEvents.RoundOver.Dispatch();
					_currentRound = null;
				}
			}
		}

		void OnEnemyDied(EnemyController enemy)
		{
			EnemyPool.ReturnEntity(enemy);
		}
	}

	public class EnemyRound
	{
		EnemyRoundInfo[] _spawnInfo;

		public EnemyRound(DifficultyConfig currentConfig)
		{
			_spawnInfo = new EnemyRoundInfo[currentConfig.Compositions.Length];
			for (int i = 0; i < currentConfig.Compositions.Length; i++)
			{
				EnemyComposition enemyComposition = currentConfig.Compositions[i];
				_spawnInfo[i] = new EnemyRoundInfo(enemyComposition);
			}

		}

		public void Update()
		{
			foreach (EnemyRoundInfo info in _spawnInfo)
			{
				info.Update();
			}
		}
		
		public bool Completed()
		{
			foreach (EnemyRoundInfo info in _spawnInfo)
			{
				if (!info.Completed)
				{
					return false;
				}
			}

			return true;
		}

		class EnemyRoundInfo
		{
			public bool Completed;
			
			EnemyComposition _comp;
			
			int _spawnCount;
			float _timeSinceLastSpawn;

			public EnemyRoundInfo(EnemyComposition comp)
			{
				_comp = comp;
				Completed = false;
			}

			public void Update()
			{
				if (!Completed)
				{
					_timeSinceLastSpawn -= GameTime.DeltaTime;
					if (_timeSinceLastSpawn <= 0f)
					{
						_timeSinceLastSpawn = _comp.TimeBetweenSpawn;
						_spawnCount++;
						
						GameEvents.EnemySpawned.Dispatch(_comp.Config);
						Completed = _spawnCount >= _comp.Count;
					}
				}
			}
		}
	}
}