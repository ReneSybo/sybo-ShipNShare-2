using System.Collections.Generic;
using Game.Events;
using Game.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
	public class EnemyController : MonoBehaviour, ISpawnable
	{
		Transform _enemyTransform;
		Vector3 _currentDirection;
		float _timeToOrientation;
		float _timeToHurt;
		Vector3 _pushDir;

		public MeshRenderer Renderer;
		public MeshFilter Filter;
		
		public Vector3 CurrentPosition;
		public float HurtRadius = 1f;
		public float Speed = 1f;
		public float TimeBetweenOrientation = 0.1f;
		public float TimeBetweenHurting = 0.5f;

		void Awake()
		{
			_enemyTransform = transform;
			
			GameEvents.GameEnded.AddListener(OnGameEnded);
			GameEvents.GameStarted.AddListener(OnGameStarted);
		}

		void OnGameStarted()
		{
			enabled = true;
		}

		void OnGameEnded()
		{
			enabled = false;
		}

		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(CurrentPosition, HurtRadius);
			
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(CurrentPosition, GlobalVariables.AvoidingRange);
		}

		[ContextMenu("Respawn")]
		public void Spawn()
		{
			Vector2 distances = GlobalVariables.EnemySpawnDistances;
			float distance = Random.Range(distances.x, distances.y);
			float angle = Random.value * 360f;

			float x = distance * Mathf.Sin(angle);
			float y = distance * Mathf.Cos(angle);
			
			CurrentPosition = new Vector3(x, 0, y);
			_enemyTransform.localPosition = CurrentPosition;
			_enemyTransform.LookAt(GlobalVariables.PlayerPos);
			_currentDirection = _enemyTransform.forward;
			
			GlobalVariables.Enemies.Add(this);
		}

		public void ApplyConfig(EnemyConfig config)
		{
			Filter.sharedMesh = config.Mesh;
			Renderer.sharedMaterial = config.Material;
		}

		void Update()
		{
			_timeToOrientation -= GameTime.DeltaTime;
			_timeToHurt -= GameTime.DeltaTime;
			if (_timeToOrientation <= 0f)
			{
				_timeToOrientation = TimeBetweenOrientation;
				_enemyTransform.LookAt(GlobalVariables.PlayerPos);
				_currentDirection = _enemyTransform.forward;
				Seperate();
			}

			_pushDir = _pushDir * 0.95f;
			
			CurrentPosition += _currentDirection * Speed * GameTime.DeltaTime;
			CurrentPosition += _pushDir * GameTime.DeltaTime;
			
			_enemyTransform.localPosition = CurrentPosition;

			if (_timeToHurt <= 0f)
			{
				Vector3 distanceToPlayer = GlobalVariables.PlayerPos - CurrentPosition;
				if (distanceToPlayer.sqrMagnitude <= HurtRadius)
				{
					_timeToHurt = TimeBetweenHurting;
					GlobalVariables.PlayerHealth--;
					GameEvents.PlayerHurt.Dispatch();
				}
			}
		}
		
		void Seperate()
		{
			List<EnemyController> nearbyEnemies = GlobalVariables.Enemies;
			Vector3 moveAmount = Vector3.zero;
			
			foreach (EnemyController enemy in nearbyEnemies)
			{
				float distance = Vector3.Distance(CurrentPosition, enemy.CurrentPosition);
				if (distance < GlobalVariables.AvoidingRange)
				{
					moveAmount += CurrentPosition - enemy.CurrentPosition;
				}
			}
			
			_pushDir += moveAmount * GlobalVariables.SeperateFactor;
		}

		void OnDestroy()
		{
			GlobalVariables.Enemies.Remove(this);
		}

		public void Despawn()
		{
			GlobalVariables.Enemies.Remove(this);
		}

		public void Hit()
		{
			GameEvents.EnemyDied.Dispatch(this);
		}
	}
}