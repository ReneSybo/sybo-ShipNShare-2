using System;
using System.Collections.Generic;
using Game.Events;
using UnityEngine;

namespace Game.Audio
{
	public class AudioSystem : MonoBehaviour
	{
		public AudioSource Source;
		public AudioEntry[] Entries = Array.Empty<AudioEntry>();

		Dictionary<AudioType, AudioEntry> _entriesMap;
		
		void Awake()
		{
			_entriesMap = new Dictionary<AudioType, AudioEntry>();
			foreach (AudioEntry entry in Entries)
			{
				_entriesMap[entry.Type] = entry;
			}
			
			GameEvents.PlayAudio.AddListener(OnAudio);
		}

		void OnAudio(AudioType type)
		{
			if (_entriesMap.TryGetValue(type, out var entry))
			{
				Play(entry);
			}
		}

		void Play(AudioEntry entry)
		{
			Source.PlayOneShot(entry.Clip);
		}
	}

	[Serializable]
	public class AudioEntry
	{
		public AudioType Type;
		public AudioClip Clip;
	}
}