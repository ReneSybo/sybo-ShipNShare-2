﻿using System;
using Game.Events;
using Game.Money;
using Game.Player;
using TMPro;
using UnityEngine;

namespace Game
{
	public class UiController : MonoBehaviour
	{
		public GameObject MainMenuRoot;
		public GameObject ShopRoot;
		public GameObject GameOverRoot;
		public GameObject GameStartRoot;
		public GameObject HudRoot;
		
		public TMP_Text MoneyText;
		public TMP_Text ScoreText;
		
		public GameObject ShopScene3D;
		public GameObject IntroScene3D;
		public GameObject MenuScene3D;
		public GameObject EndScene3D;
		
		void Awake()
		{
			GameEvents.GameEnded.AddListener(OnGameEnded);
			GameEvents.GameStarted.AddListener(OnGameStarted);
			GameEvents.RoundOver.AddListener(OnRoundEnded);
			GameEvents.CutsceneState.AddListener(OnCutsceneState);
			GameEvents.MoneyGained.AddListener(OnMoneyGained);
			GameEvents.MoneySpend.AddListener(UpdateMoneyText);
			
			OnResetGame();
		}

		void OnGameStarted()
		{
			GlobalVariables.ScoreLostToTime = 0;
		}

		void OnMoneyGained(MoneyEntity entity)
		{
			UpdateMoneyText();
		}

		void Update()
		{
			GlobalVariables.ScoreLostToTime += GameTime.DeltaTime * GlobalVariables.ScoreLostPerSecond;
			int totalScore = (int)(GlobalVariables.Score - GlobalVariables.ScoreLostToTime);
			ScoreText.text = totalScore.ToString();
		}

		void UpdateMoneyText()
		{
			MoneyText.text = GlobalVariables.Money.ToString();
		}

		void OnCutsceneState(bool state)
		{
			if (!state)
			{
				HudRoot.SetActive(true);
			}
		}

		public void OnStartGame()
		{
			HideAll();
			GameStartRoot.SetActive(true);
			IntroScene3D.SetActive(true);
			GameEvents.GameStarted.Dispatch();
			Upgrades.Reset();
		}

		public void OnSkipCutscene()
		{
			HideAll();
			GameEvents.GameStarted.Dispatch();
			Upgrades.Reset();
			GameEvents.CutsceneState.Dispatch(false);
		}

		public void OnLeaveShop()
		{
			HideAll();
			GameEvents.RoundStarted.Dispatch();
			HudRoot.SetActive(true);
		}
		
		public void OnResetGame()
		{
			HideAll();
			MainMenuRoot.SetActive(true);
			UpdateMoneyText();
			MenuScene3D.SetActive(true);
		}

		void HideAll()
		{
			MainMenuRoot.SetActive(false);
			ShopRoot.SetActive(false);
			GameOverRoot.SetActive(false);
			ShopScene3D.SetActive(false);
			GameStartRoot.SetActive(false);
			IntroScene3D.SetActive(false);
			HudRoot.SetActive(false);
			MenuScene3D.SetActive(false);
			EndScene3D.SetActive(false);
		}

		void OnRoundEnded()
		{
			HideAll();
			ShopRoot.SetActive(true);
			ShopScene3D.SetActive(true);
			HudRoot.SetActive(true);
		}

		void OnGameEnded()
		{
			HideAll();
			GameOverRoot.SetActive(true);
			EndScene3D.SetActive(true);
		}
	}
}