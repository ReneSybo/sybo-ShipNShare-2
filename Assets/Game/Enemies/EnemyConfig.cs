using UnityEngine;

namespace Game.Enemies
{
	[CreateAssetMenu(menuName = "EnemyConfig")]
	public class EnemyConfig : ScriptableObject
	{
		public float Health;
		public float Speed;
		public float Damage;
		public float TimeBetweenAttacks;
	}
}