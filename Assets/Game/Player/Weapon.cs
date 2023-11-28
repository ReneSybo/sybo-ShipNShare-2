﻿using Game.Enemies;
using Game.Events;
using UnityEngine;

namespace Game.Player
{
	public class Weapon : MonoBehaviour
	{
		public ProjectilePool Pool;
		public Transform ShotPoint;

		public float ShotSpeed = 0.5f;
		float _timeToShoot;

		void Awake()
		{
			GameEvents.ProjectileDespawned.AddListener(OnProjectileDespawn);
		}

		void OnProjectileDespawn(Projectile projectile)
		{
			Pool.ReturnEntity(projectile);
		}

		void Update()
		{
			_timeToShoot -= GameTime.DeltaTime;
			if (_timeToShoot <= 0)
			{
				TryShoot();
				_timeToShoot = ShotSpeed;
			}
		}

		void TryShoot()
		{
			Vector3 playerPos = GlobalVariables.PlayerPos;

			EnemyController closestEnemy = null;
			float closestDistance = float.MaxValue;
			foreach (EnemyController enemy in GlobalVariables.Enemies)
			{
				float distance = (enemy.CurrentPosition - playerPos).sqrMagnitude;
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestEnemy = enemy;
				}
			}

			if (closestEnemy != null)
			{
				ShootAt(closestEnemy.CurrentPosition);
			}
		}

		void ShootAt(Vector3 target)
		{
			Projectile projectile = Pool.SpawnItem();
			projectile.SetTarget(ShotPoint.position, target);
		}
	}
}