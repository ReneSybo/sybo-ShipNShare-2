using System;
using Game.Events;
using UnityEngine;

namespace Game
{
	public class IntroSequence : MonoBehaviour
	{
		public Animation Animation;

		void OnEnable()
		{
			GameEvents.CutsceneState.Dispatch(true);
		}

		void Update()
		{
			if (!Animation.isPlaying)
			{
				GameEvents.CutsceneState.Dispatch(false);
				gameObject.SetActive(false);
			}
		}
	}
}