using System;
using Game.Events;
using UnityEngine;

namespace Game.Enemies
{
	public class EnemyDeath : MonoBehaviour, ISpawnable
	{
		public Animation Animation;
		
		Transform _transform;

		void Awake()
		{
			_transform = transform;
		}

		public void Spawn()
		{
			Animation.Play();
		}

		public void Despawn()
		{
			Animation.Stop();
			Animation.Rewind();
		}

		public void CopyFrom(EnemyController enemy)
		{
			Transform enemyTransform = enemy.transform;
			_transform.position = enemyTransform.position;
			_transform.rotation = enemyTransform.rotation;
		}

		void Update()
		{
			if (!Animation.isPlaying)
			{
				GameEvents.EnemyDeathDone.Dispatch(this);
			}
		}
	}
}