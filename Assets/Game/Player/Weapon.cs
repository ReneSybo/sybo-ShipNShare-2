using Game.Enemies;
using Game.Events;
using UnityEngine;
using AudioType = Game.Audio.AudioType;

namespace Game.Player
{
	public class Weapon : MonoBehaviour
	{
		public ProjectilePool Pool;
		public Transform PrimaryShotpoint;
		public Transform SecondaryShotPoint;
		public Vector3 CurrentShotDirection;
		public float ShootingDistance;

		bool _shootingFromPrimary;
		public float ShotSpeed = 0.5f;
		float _timeToShoot;

		void Awake()
		{
			GameEvents.ProjectileDespawned.AddListener(OnProjectileDespawn);
			GameEvents.GameEnded.AddListener(OnGameEnded);
			GameEvents.RoundStarted.AddListener(OnRoundStarted);
			GameEvents.CastleTrashed.AddListener(OnRoundStarted);
			enabled = false;
		}

		void OnRoundStarted()
		{
			enabled = true;
		}

		void OnGameEnded()
		{
			enabled = false;
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, ShootingDistance);
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
				_timeToShoot = ShotSpeed - Upgrades.Instance.GetValue(UpgradeType.AttackSpeed);
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

			if (closestDistance <= ShootingDistance * ShootingDistance)
			{
				if (closestEnemy != null)
				{
					ShootAt(closestEnemy.CurrentPosition);
					GameEvents.PlayAudio.Dispatch(AudioType.WeaponShot);
				}
			}
		}

		void ShootAt(Vector3 target)
		{
			Projectile projectile = Pool.SpawnItem();

			Vector3 startPoint = _shootingFromPrimary ? PrimaryShotpoint.position : SecondaryShotPoint.position;
			CurrentShotDirection = (target - startPoint).normalized;
			projectile.SetTarget(startPoint, target);
			GameEvents.ProjectileSpawned.Dispatch(_shootingFromPrimary);
			
			_shootingFromPrimary = !_shootingFromPrimary;
		}
	}
}