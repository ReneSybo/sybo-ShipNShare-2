using System;
using Game.Enemies;
using Game.Player;

namespace Game.Events
{
	public static class GameEvents
	{
		public static readonly GameEvent PlayerHurt = new GameEvent();
		public static readonly GameEvent GameEnded = new GameEvent(); //Player died, show full screen "Game Over"
		public static readonly GameEvent GameStarted = new GameEvent(); //Click "Let's do it!"
		public static readonly GameEvent CastleTrashed = new GameEvent(); //The sand castle is ruined!
		public static readonly GameEvent RoundOver = new GameEvent(); //All enemies dead
		public static readonly GameEvent RoundStarted = new GameEvent(); //Left the shop screen
		public static readonly GameEvent<EnemyDeath> EnemyDeathDone = new GameEvent<EnemyDeath>();
		public static readonly GameEvent<EnemyController> EnemyDied = new GameEvent<EnemyController>();
		public static readonly GameEvent<EnemyConfig> EnemySpawned = new GameEvent<EnemyConfig>();
		public static readonly GameEvent<Projectile> ProjectileDespawned = new GameEvent<Projectile>();
		public static readonly GameEvent<bool> ProjectileSpawned = new GameEvent<bool>();
	}
	
	public class GameEvent
	{
		event Action _listeners;
		
		public GameEvent()
		{
		}

		public void AddListener(Action action)
		{
			_listeners += action;
		}

		public void RemoveListener(Action action)
		{
			_listeners -= action;
		}

		public void Dispatch()
		{
			_listeners?.Invoke();
		}
	}
	public class GameEvent<TData>
	{
		event Action<TData> _listeners;

		public void AddListener(Action<TData> action)
		{
			_listeners += action;
		}

		public void RemoveListener(Action<TData> action)
		{
			_listeners -= action;
		}

		public void Dispatch(TData data)
		{
			_listeners?.Invoke(data);
		}
	}
}