using System.Collections.Generic;
using Game.Enemies;
using Game.Events;
using UnityEngine;

namespace Game.Player
{
	public class Projectile : MonoBehaviour, ISpawnable
	{
		public float ProjectileDamage = 1f;
		public float ProjectileSpeed = 1f;
		public float Size = 1f;
		public float LifeTime = 3f;
		
		Transform _projectileTransform;
		float _remainingLife;
		Vector3 _forward;
		Vector3 _position;

		void Awake()
		{
			_projectileTransform = transform;
		}

		public void SetTarget(Vector3 start, Vector3 target)
		{
			_projectileTransform.position = start;
			_projectileTransform.LookAt(new Vector3(target.x, start.y, target.z));
			_forward = _projectileTransform.forward;
			_position = _projectileTransform.localPosition;
		}

		void Update()
		{
			_position += _forward * ProjectileSpeed * GameTime.DeltaTime;
			_projectileTransform.localPosition = _position;

			TryHitEnemy();

			_remainingLife -= GameTime.DeltaTime;
			if (_remainingLife <= 0)
			{
				GameEvents.ProjectileDespawned.Dispatch(this);
			}
		}

		void TryHitEnemy()
		{
			float squareSize = Size * Size;
			
			List<EnemyController> enemies = GlobalVariables.Enemies;
			foreach (EnemyController enemy in enemies)
			{
				float distance = new Vector2(enemy.CurrentPosition.x - _position.x, enemy.CurrentPosition.z - _position.z).sqrMagnitude;
				if (distance < squareSize)
				{
					enemy.Hit(ProjectileDamage);
					GameEvents.ProjectileDespawned.Dispatch(this);
					break;
				}
			}
		}

		public void Spawn()
		{
			_remainingLife = LifeTime;
		}

		public void Despawn()
		{
		}
	}
}