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
			_dictionary[UpgradeType.Damage] = new UpgradeState(10, 0.2f, 10 * 1);
			_dictionary[UpgradeType.Health] = new UpgradeState(10, 1f, 5 * 1);
			_dictionary[UpgradeType.Speed] = new UpgradeState(10, 0.05f, 8 * 1);
			_dictionary[UpgradeType.AttackSpeed] = new UpgradeState(10, 0.1f, 3 * 1);
		}

		public float GetValue(UpgradeType type)
		{
			return _dictionary[type].CurrentValue;
		}

		public bool CanBuy(UpgradeType type)
		{
			return _dictionary[type].CanUpgrade();
		}

		public bool Buy(UpgradeType type)
		{
			return _dictionary[type].Upgrade();
		}

		public float GetRatio(UpgradeType type)
		{
			return _dictionary[type].GetRatio();
		}
		
		public float GetIncrement(UpgradeType type)
		{
			return _dictionary[type].UpgradeIncrement;
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

		public bool Upgrade()
		{
			if (CanUpgrade())
			{
				CurrentUpgradeIndex++;
				CurrentValue += UpgradeIncrement;

				GlobalVariables.Money -= UpgradeCost;
				GameEvents.MoneySpend.Dispatch();

				GlobalVariables.Score += GlobalVariables.ScorePerUpgrade;
				
				return true;
			}

			return false;
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