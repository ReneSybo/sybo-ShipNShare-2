using System;
using Game.Enemies;
using Game.Events;
using Game.Player;
using UnityEngine;

namespace Game.Money
{
	public class MoneyEntity : MonoBehaviour
	{
		[NonSerialized] public int Value;
		
		public float PickupDistance;

		Transform _transform;

		void Awake()
		{
			_transform = transform;
		}
		
		void Update()
		{
			float distanceToPlayer = (_transform.position - GlobalVariables.PlayerPos).sqrMagnitude;
			if (distanceToPlayer <= PickupDistance)
			{
				_transform.rotation = Quaternion.Euler(0, 0, -180);
				enabled = false;
				
				GameEvents.MoneyGained.Dispatch(this);
			}
		}

		public void Setup(EnemyController enemy)
		{
			_transform.position = enemy.CurrentPosition;
			Value = enemy.MoneyOnDeath;
		}
	}
}