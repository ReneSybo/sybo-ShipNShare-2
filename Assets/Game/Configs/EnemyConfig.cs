using UnityEngine;

namespace Game.Enemies
{
	[CreateAssetMenu(menuName = "EnemyConfig")]
	public class EnemyConfig : ScriptableObject
	{
		[Header("Visuals")]
		public EnemyType Type;
		
		[Header("Stats")]
		public float Health;
		public float Speed;
		public float Damage;
		public float TimeBetweenAttacks;
		
		[Header("Drops")]
		public int MoneyOnDeath;
	}
}