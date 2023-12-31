﻿using System;
using System.Collections;
using System.Collections.Generic;
using Game.Difficulty;
using Game.Events;
using Game.Money;
using Game.Player;
using UnityEngine;

namespace Game.Enemies
{
	public class EnemyManager : MonoBehaviour
	{
		public DifficultyConfig[] Configs;
		
		public EnemySpawnPool EnemyPoolAgile;
		public EnemySpawnPool EnemyPoolBrute;
		public EnemySpawnPool EnemyPoolRival;
		public EnemyDeathSpawnPool DeathPoolAgile;
		public EnemyDeathSpawnPool DeathPoolBrute;
		public MoneySpawnPool MoneyPool;

		int _currentConfigIndex;
		EnemyRound _currentRound;
		
		List<MoneyEntity> _activeMoney = new List<MoneyEntity>(100);
		
		void Awake()
		{
			GameEvents.EnemyDied.AddListener(OnEnemyDied);
			GameEvents.EnemySpawned.AddListener(OnEnemySpawned);
			GameEvents.GameEnded.AddListener(OnGameEnded);
			GameEvents.RoundStarted.AddListener(OnRoundStarted);
			GameEvents.CastleTrashed.AddListener(OnRoundStarted);
			GameEvents.EnemyDeathDone.AddListener(OnEnemyDeathDone);
			GameEvents.MoneyGained.AddListener(OnMoneyGained);

			enabled = false;
			_currentConfigIndex = -1;
		}

		void OnMoneyGained(MoneyEntity money)
		{
			if (_activeMoney.Remove(money))
			{
				GlobalVariables.Score += GlobalVariables.ScorePerCoinPickup;
				MoneyPool.ReturnEntity(money);
			}
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
				GetEnemyPool(enemyController.Type).ReturnEntity(enemyController);
			}

			foreach (MoneyEntity entity in _activeMoney)
			{
				MoneyPool.ReturnEntity(entity);
			}

			_currentConfigIndex = -1;
		}

		EnemySpawnPool GetEnemyPool(EnemyType type)
		{
			switch (type)
			{
				case EnemyType.Agile: 
					return EnemyPoolAgile;
				case EnemyType.Brute:
					return EnemyPoolBrute;
				case EnemyType.Rival:
					return EnemyPoolRival;
			}
			
			return EnemyPoolAgile;
		}
		EnemyDeathSpawnPool GetEnemyDeathPool(EnemyType type)
		{
			switch (type)
			{
				case EnemyType.Agile: 
				case EnemyType.Rival: 
					return DeathPoolAgile;
				case EnemyType.Brute:
					return DeathPoolBrute;
			}
			
			return DeathPoolAgile;
		}

		void OnEnemySpawned(EnemyConfig config)
		{
			EnemyController enemy = GetEnemyPool(config.Type).SpawnItem();
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
					_currentRound = null;
					StartCoroutine(DelayEnd());
					GameObject.Find("DelayPanel").GetComponent<Animation>().Play();
				}
			}
		}

		private IEnumerator DelayEnd()
		{
			yield return new WaitForSeconds(5.0f);
			GameEvents.RoundOver.Dispatch();
		}


		void OnEnemyDied(EnemyController enemy)
		{
			GlobalVariables.Score += GlobalVariables.ScorePerKill;
			GetEnemyPool(enemy.Type).ReturnEntity(enemy);

			EnemyDeath enemyDeath = GetEnemyDeathPool(enemy.Type).SpawnItem();
			enemyDeath.CopyFrom(enemy);

			if (enemy.MoneyOnDeath > 0)
			{
				MoneyEntity moneyEntity = MoneyPool.SpawnItem();
				moneyEntity.Setup(enemy);
				_activeMoney.Add(moneyEntity);
			}
		}
		
		void OnEnemyDeathDone(EnemyDeath death)
		{
			GetEnemyDeathPool(death.Type).ReturnEntity(death);
		}
	}

	public class EnemyRound
	{
		EnemyRoundInfo[] _spawnInfo;

		public EnemyRound(DifficultyConfig currentConfig)
		{
			GlobalVariables.TotalDamageScale += currentConfig.DamageScale;
			GlobalVariables.TotalHealthScale += currentConfig.HealthScale;
			GlobalVariables.TotalSpeedScale += currentConfig.SpeedScale;
			GlobalVariables.TotalAttackSpeedScale += currentConfig.AttackSpeedScale;
			
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
				Completed = comp.Count <= 0;
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