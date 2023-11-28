using System;
using Game.Events;
using UnityEngine;

namespace Game.Player
{
	public class SandCastle : MonoBehaviour
	{
		static readonly int FaceDilate = Shader.PropertyToID("_FaceDilate");
		
		public float TrashDistance;
		public Material TextMaterial;

		public GameObject TutorialText;
		public GameObject TutorialArrow;
		
		Transform _transform;
		bool _activeChecking;
		bool _activeFading;
		float _currentFadeValue;

		void Awake()
		{
			_transform = transform;
			
			GameEvents.GameStarted.AddListener(OnGameStart);
			_activeChecking = false;
		}

		void OnGameStart()
		{
			_transform.rotation = Quaternion.Euler(0, 0, 0);
			TextMaterial.SetFloat(FaceDilate, 0);
			TutorialArrow.SetActive(true);
			TutorialText.SetActive(true);
			_activeChecking = true;
			_activeFading = false;
		}

		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(transform.position, TrashDistance * TrashDistance);
		}

		void Update()
		{
			if (_activeChecking)
			{
				float distanceToPlayer = (_transform.position - GlobalVariables.PlayerPos).sqrMagnitude;
				if (distanceToPlayer <= TrashDistance)
				{
					_transform.rotation = Quaternion.Euler(0, 0, -180);
					_activeChecking = false;
					_activeFading = true;
					TutorialArrow.SetActive(false);

					_currentFadeValue = 0f;
					GameEvents.CastleTrashed.Dispatch();
				}
			}
			
			if(_activeFading)
			{
				if (_currentFadeValue > -1)
				{
					_currentFadeValue -= GameTime.DeltaTime;
					TextMaterial.SetFloat(FaceDilate, _currentFadeValue);
					if (_currentFadeValue <= -1)
					{
						_activeFading = false;
						TutorialText.SetActive(false);
						_currentFadeValue = 0;
						TextMaterial.SetFloat(FaceDilate, _currentFadeValue);
					}
				}
			}

		}
	}
}