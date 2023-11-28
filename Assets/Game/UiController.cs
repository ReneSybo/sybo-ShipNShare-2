using System;
using Game.Events;
using UnityEngine;

namespace Game
{
	public class UiController : MonoBehaviour
	{
		public GameObject MainMenuRoot;
		public GameObject ShopRoot;
		public GameObject GameOverRoot;
		
		void Awake()
		{
			GameEvents.GameEnded.AddListener(OnGameEnded);
			GameEvents.RoundOver.AddListener(OnRoundEnded);
			
			OnResetGame();
		}

		public void OnStartGame()
		{
			HideAll();
			GameEvents.GameStarted.Dispatch();
		}

		public void OnLeaveShop()
		{
			HideAll();
			GameEvents.RoundStarted.Dispatch();
		}
		
		public void OnResetGame()
		{
			HideAll();
			MainMenuRoot.SetActive(true);
		}

		void HideAll()
		{
			MainMenuRoot.SetActive(false);
			ShopRoot.SetActive(false);
			GameOverRoot.SetActive(false);
		}

		void OnRoundEnded()
		{
			HideAll();
			ShopRoot.SetActive(true);
		}

		void OnGameEnded()
		{
			HideAll();
			GameOverRoot.SetActive(true);
		}
	}
}