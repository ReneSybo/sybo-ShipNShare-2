using Game.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class ShopUi : MonoBehaviour
	{
		public Button SpeedButton;
		public Button HealthButton;
		public Button AttackSpeedButton;
		public Button DamageButton;
		
		public Slider SpeedSlider;
		public Slider HealthSlider;
		public Slider AttackSpeedSlider;
		public Slider DamageSlider;
		
		public TMP_Text SpeedText;
		public TMP_Text HealthText;
		public TMP_Text AttackSpeedText;
		public TMP_Text DamageText;

		void OnEnable()
		{
			SpeedButton.enabled = Upgrades.Instance.CanBuy(UpgradeType.Speed);
			HealthButton.enabled = Upgrades.Instance.CanBuy(UpgradeType.Health);
			AttackSpeedButton.enabled = Upgrades.Instance.CanBuy(UpgradeType.AttackSpeed);
			DamageButton.enabled = Upgrades.Instance.CanBuy(UpgradeType.Damage);

			SpeedSlider.value = Upgrades.Instance.GetRatio(UpgradeType.Speed);
			HealthSlider.value = Upgrades.Instance.GetRatio(UpgradeType.Health);
			AttackSpeedSlider.value = Upgrades.Instance.GetRatio(UpgradeType.AttackSpeed);
			DamageSlider.value = Upgrades.Instance.GetRatio(UpgradeType.Damage);

			SpeedText.text = "-" + Upgrades.Instance.GetUpgradeCost(UpgradeType.Speed);
			HealthText.text = "-" + Upgrades.Instance.GetUpgradeCost(UpgradeType.Health);
			AttackSpeedText.text = "-" + Upgrades.Instance.GetUpgradeCost(UpgradeType.AttackSpeed);
			DamageText.text = "-" + Upgrades.Instance.GetUpgradeCost(UpgradeType.Damage);
		}

		void UpdateButtonState(Button button, Slider slider, UpgradeType state)
		{
			button.enabled = Upgrades.Instance.CanBuy(state);
			slider.value = Upgrades.Instance.GetRatio(state);
		}
		
		public void OnTryBuySpeed()
		{
			Upgrades.Instance.Buy(UpgradeType.Speed);
			UpdateButtonState(SpeedButton, SpeedSlider, UpgradeType.Speed);
		}

		public void OnTryBuyHealth()
		{
			bool bought = Upgrades.Instance.Buy(UpgradeType.Health);
			UpdateButtonState(HealthButton, HealthSlider, UpgradeType.Health);

			if (bought)
			{
				float healthIncrement = Upgrades.Instance.GetIncrement(UpgradeType.Health);
				GlobalVariables.PlayerHealth += healthIncrement;
				GlobalVariables.PlayerMaxHealth += healthIncrement;
			}
		}
		public void OnTryBuyAttackSpeed()
		{
			Upgrades.Instance.Buy(UpgradeType.AttackSpeed);
			UpdateButtonState(AttackSpeedButton, AttackSpeedSlider, UpgradeType.AttackSpeed);
		}
		public void OnTryBuyDamage()
		{
			Upgrades.Instance.Buy(UpgradeType.Damage);
			UpdateButtonState(DamageButton, DamageSlider, UpgradeType.Damage);
		}
	}
}