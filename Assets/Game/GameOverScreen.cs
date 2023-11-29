using System;
using Game.Player;
using TMPro;
using UnityEngine;

namespace Game
{
	public class GameOverScreen : MonoBehaviour
	{
		public TMP_Text MoneyText;
		
		void OnEnable()
		{
			int points = (int)(GlobalVariables.Score - GlobalVariables.ScoreLostPerSecond + GlobalVariables.ScoreGainedFromFlex);
			MoneyText.text = "Score: " + points;
		}
	}
}