using System;

namespace Game.Events
{
	public class GameEvent
	{
		public static readonly GameEvent PlayerHurt = new GameEvent();

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
			;
		}
	}
}