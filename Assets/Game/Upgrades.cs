using System.Collections.Generic;
using Game.Events;
using Game.Player;

namespace Game
{
	public class Upgrades
	{
		public static Upgrades Instance;

		public static void Reset()
		{
			Instance = new Upgrades();
		}

		Dictionary<UpgradeType, UpgradeState> _dictionary;

		Upgrades()
		{
			_dictionary = new Dictionary<UpgradeType, UpgradeState>();
			_dictionary[UpgradeType.Damage] = new UpgradeState(10, 0.2f, 10 * 0);
			_dictionary[UpgradeType.Health] = new UpgradeState(10, 1f, 5 * 0);
			_dictionary[UpgradeType.Speed] = new UpgradeState(10, 0.05f, 8 * 0);
			_dictionary[UpgradeType.AttackSpeed] = new UpgradeState(10, 0.1f, 3 * 0);
		}

		public float GetValue(UpgradeType type)
		{
			return _dictionary[type].CurrentValue;
		}

		public bool CanBuy(UpgradeType type)
		{
			return _dictionary[type].CanUpgrade();
		}

		public void Buy(UpgradeType type)
		{
			_dictionary[type].Upgrade();
		}

		public float GetRatio(UpgradeType type)
		{
			return _dictionary[type].GetRatio();
		}
	}
	public class UpgradeState
	{
		public int MaxUpgradeCount;
		public float UpgradeIncrement;
		public int UpgradeCost;
		
		public float CurrentValue;
		public int CurrentUpgradeIndex;

		public UpgradeState(int maxUpgradeCount, float upgradeIncrement, int upgradeCost)
		{
			MaxUpgradeCount = maxUpgradeCount;
			UpgradeIncrement = upgradeIncrement;
			UpgradeCost = upgradeCost;

			CurrentValue = 0f;
			CurrentUpgradeIndex = 0;
		}

		public bool CanUpgrade()
		{
			return CanAfford() && CurrentUpgradeIndex < MaxUpgradeCount;
		}

		public bool CanAfford()
		{
			return GlobalVariables.Money >= UpgradeCost;
		}

		public void Upgrade()
		{
			if (CanUpgrade())
			{
				CurrentUpgradeIndex++;
				CurrentValue += UpgradeIncrement;

				GlobalVariables.Money -= UpgradeCost;
				GameEvents.MoneySpend.Dispatch();
			}
		}

		public float GetRatio()
		{
			return (float)CurrentUpgradeIndex / MaxUpgradeCount;
		}
	}

	public enum UpgradeType
	{
		Speed,
		Health,
		AttackSpeed,
		Damage,
	}
}