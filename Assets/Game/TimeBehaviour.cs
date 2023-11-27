using UnityEngine;

namespace Game
{
	public class TimeBehaviour : MonoBehaviour
	{
		void Update()
		{
			GameTime.DeltaTime = Time.deltaTime;
		}
	}
	public static class GameTime
	{
		public static float DeltaTime;
	}
}