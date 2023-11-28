using System;
using Game.Events;
using UnityEngine;

namespace Game.Player
{
	public class SandCastle : MonoBehaviour
	{
		public float TrashDistance;

		public GameObject TutorialText;
		public GameObject Gun1;
		public GameObject Gun2;
		
		Transform _transform;

		void Awake()
		{
			_transform = transform;
			
			GameEvents.GameStarted.AddListener(OnGameStart);
			enabled = false;
		}

		void OnGameStart()
		{
			TutorialText.SetActive(true);
			Gun1.SetActive(false);
			Gun2.SetActive(false);
			_transform.rotation = Quaternion.Euler(0, 0, 0);
			enabled = true;
		}

		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(transform.position, TrashDistance * TrashDistance);
		}

		void Update()
		{
			float distanceToPlayer = (_transform.position - GlobalVariables.PlayerPos).sqrMagnitude;
			if (distanceToPlayer <= TrashDistance)
			{
				_transform.rotation = Quaternion.Euler(0, 0, -180);
				enabled = false;
				TutorialText.SetActive(false);
				Gun1.SetActive(true);
				Gun2.SetActive(true);
				
				GameEvents.CastleTrashed.Dispatch();
			}
		}
	}
}