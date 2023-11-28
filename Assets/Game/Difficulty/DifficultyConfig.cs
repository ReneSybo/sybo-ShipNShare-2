using System;
using Game.Enemies;
using UnityEngine;

namespace Game.Difficulty
{
	[CreateAssetMenu(menuName = "DifficultyConfig")]
	public class DifficultyConfig : ScriptableObject
	{
		[Header("Enemy related")] 
		public EnemyComposition[] Compositions = Array.Empty<EnemyComposition>();
	}
	
	[Serializable]
	public class EnemyComposition
	{
		public EnemyConfig Config;
		public int Count;
		public float TimeBetweenSpawn;
	}
}