using System;
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
		public TMP_Text WaveText;
		
		public GameObject ShopScene3D;
		public GameObject IntroScene3D;
		public GameObject MenuScene3D;
		public GameObject EndScene3D;

		public int counter = 0;
		
		bool _isInRound;
		
		void Awake()
		{
			GameEvents.GameEnded.AddListener(OnGameEnded);
			GameEvents.GameStarted.AddListener(OnGameStarted);
			GameEvents.RoundOver.AddListener(OnRoundEnded);
			GameEvents.RoundStarted.AddListener(OnRoundStarted);
			GameEvents.CastleTrashed.AddListener(OnRoundStarted);
			GameEvents.CutsceneState.AddListener(OnCutsceneState);
			GameEvents.MoneyGained.AddListener(OnMoneyGained);
			GameEvents.MoneySpend.AddListener(UpdateMoneyText);
			
			OnResetGame();
		}

		void OnRoundStarted()
		{
			_isInRound = true;
			WaveText.text = "Wave: " + counter;
		}

		void OnGameStarted()
		{
			_isInRound = false;
			GlobalVariables.ScoreLostToTime = 0;
			GlobalVariables.ScoreGainedFromFlex = 0;
		}

		void OnMoneyGained(MoneyEntity entity)
		{
			UpdateMoneyText();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit(0);
				return;
			}
			
			if (GlobalVariables.IsFlexing && _isInRound)
			{
				GlobalVariables.ScoreGainedFromFlex += GameTime.DeltaTime * GlobalVariables.ScorePerSeconfFlex;
			}
			GlobalVariables.ScoreLostToTime += GameTime.DeltaTime * GlobalVariables.ScoreLostPerSecond;
			ScoreText.text = "Score: " + GlobalVariables.TotalScore;
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
			counter = 1;
			WaveText.text = "Wave: " + counter;
		}

		public void OnSkipCutscene()
		{
			HideAll();
			GameEvents.GameStarted.Dispatch();
			Upgrades.Reset();
			GameEvents.CutsceneState.Dispatch(false);
			counter = 1;
			WaveText.text = "Wave: " + counter;
		}

		public void OnLeaveShop()
		{
			HideAll();
			counter = counter + 1;

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
			_isInRound = false;
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