using System;
using Game.Enemies;
using Game.Events;
using Game.Player;
using UnityEngine;
using AudioType = Game.Audio.AudioType;

namespace Game.Money
{
	public class MoneyEntity : MonoBehaviour
	{
		public float PickupDistance;

		int _value;
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
				GlobalVariables.Money += _value;
				GameEvents.PlayAudio.Dispatch(AudioType.PickupCoin);
				GameEvents.MoneyGained.Dispatch(this);
			}
		}

		public void Setup(EnemyController enemy)
		{
			_transform.position = enemy.CurrentPosition;
			_value = enemy.MoneyOnDeath;
		}
	}
}