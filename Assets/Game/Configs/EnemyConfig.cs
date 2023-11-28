using UnityEngine;

namespace Game.Enemies
{
	[CreateAssetMenu(menuName = "EnemyConfig")]
	public class EnemyConfig : ScriptableObject
	{
		[Header("Visuals")]
		public Mesh Mesh;
		public Material Material;
		
		[Header("Stats")]
		public float Health;
		public float Speed;
		public float Damage;
		public float TimeBetweenAttacks;
	}
}