using Game.Events;
using UnityEngine;

namespace Game.Player
{
	public class PlayerHealthBar : MonoBehaviour
	{
		public Transform Display;

		void Awake()
		{
			GameEvents.PlayerHurt.AddListener(OnUpdateHealth);
			GameEvents.RoundStarted.AddListener(OnUpdateHealth);
			GameEvents.GameStarted.AddListener(OnUpdateHealth);
		}

		void OnUpdateHealth()
		{
			float ratio = GlobalVariables.PlayerHealth / GlobalVariables.PlayerMaxHealth;
			Display.localScale = new Vector3(ratio, 1, 1);
		}
	}
}