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
		
		public GameObject ShopScene3D;
		
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
			Upgrades.Reset();
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
			ShopScene3D.SetActive(false);
		}

		void OnRoundEnded()
		{
			HideAll();
			ShopRoot.SetActive(true);
			ShopScene3D.SetActive(true);
		}

		void OnGameEnded()
		{
			HideAll();
			GameOverRoot.SetActive(true);
		}
	}
}