using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public abstract class SpawnPool<TScript> : MonoBehaviour where TScript : MonoBehaviour
	{
		public int InitialSpawnCount = 0;
		public TScript Source;
		
		List<TScript> _availableEntities;

		void Awake()
		{
			_availableEntities = new List<TScript>(InitialSpawnCount);
			for (int i = 0; i < InitialSpawnCount; i++)
			{
				TScript entity = Instantiate(Source, transform);
				_availableEntities.Add(entity);
				entity.gameObject.SetActive(false);
			}
		}

		public TScript SpawnItem()
		{
			TScript entity;
			if (_availableEntities.Count > 0)
			{
				int lastIndex = _availableEntities.Count - 1;
				entity = _availableEntities[lastIndex];
				_availableEntities.RemoveAt(lastIndex);
			}
			else
			{
				entity = Instantiate(Source, transform);
			}

			entity.gameObject.SetActive(true);
			if (entity is ISpawnable spawnable)
			{
				spawnable.Spawn();
			}
			
			return entity;
		}

		public void ReturnEntity(TScript entity)
		{
			entity.gameObject.SetActive(false);
			if (entity is ISpawnable spawnable)
			{
				spawnable.Despawn();
			}
			
			_availableEntities.Add(entity);
		}
	}
}