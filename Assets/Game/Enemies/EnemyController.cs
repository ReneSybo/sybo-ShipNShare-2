using System;
using System.Collections.Generic;
using Game.Events;
using Game.Player;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
	public class EnemyController : MonoBehaviour
	{
		Transform _enemyTransform;
		Vector3 _currentDirection;
		Vector3 _currentPosition;
		float _timeToOrientation;
		float _timeToHurt;
		Vector3 _pushDir;

		public float HurtRadius = 1f;
		public float Speed = 1f;
		public float TimeBetweenOrientation = 0.1f;
		public float TimeBetweenHurting = 0.1f;

		void Awake()
		{
			GlobalVariables.Enemies.Add(this);
			_enemyTransform = transform;
			Respawn();
		}

		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(_currentPosition, HurtRadius);
			
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(_currentPosition, GlobalVariables.AvoidingRange);
		}

		[ContextMenu("Respawn")]
		void Respawn()
		{
			Vector2 distances = GlobalVariables.EnemySpawnDistances;
			float distance = Random.Range(distances.x, distances.y);
			float angle = Random.value * 360f;

			float x = distance * Mathf.Sin(angle);
			float y = distance * Mathf.Cos(angle);
			
			_currentPosition = new Vector3(x, 0, y);
			_enemyTransform.localPosition = _currentPosition;
			_enemyTransform.LookAt(GlobalVariables.PlayerPos);
			_currentDirection = _enemyTransform.forward;
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
			
			_currentPosition += _currentDirection * Speed * GameTime.DeltaTime;
			_currentPosition += _pushDir * GameTime.DeltaTime;
			
			_enemyTransform.localPosition = _currentPosition;

			if (_timeToHurt <= 0f)
			{
				Vector3 distanceToPlayer = GlobalVariables.PlayerPos - _currentPosition;
				if (distanceToPlayer.sqrMagnitude <= HurtRadius)
				{
					GameEvent.PlayerHurt.Dispatch();
					Respawn();
				}
			}
		}
		
		void Seperate()
		{
			List<EnemyController> nearbyEnemies = GlobalVariables.Enemies;
			Vector3 moveAmount = Vector3.zero;
			
			foreach (EnemyController enemy in nearbyEnemies)
			{
				float distance = Vector3.Distance(_currentPosition, enemy._currentPosition);
				if (distance < GlobalVariables.AvoidingRange)
				{
					moveAmount += _currentPosition - enemy._currentPosition;
				}
			}
			
			_pushDir += moveAmount * GlobalVariables.SeperateFactor;
		}

		void OnDestroy()
		{
			GlobalVariables.Enemies.Remove(this);
		}
	}
}