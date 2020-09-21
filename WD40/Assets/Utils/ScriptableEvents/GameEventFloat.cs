using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WD40
{
	public class GameEventFloat : ScriptableObject
	{
		List<GameEventListenerFloat> listeners = new List<GameEventListenerFloat>();
		

		public void Raise(float param)
		{
			for (int i = listeners.Count - 1; i >= 0; i--)
				listeners[i].OnEventRaised(param);
		}

		public void RegisterListener(GameEventListenerFloat listener)
		{
			if (!listeners.Contains(listener))
				listeners.Add(listener);
		}

		public void UnregisterListener(GameEventListenerFloat listener)
		{
			if (listeners.Contains(listener))
				listeners.Remove(listener);
		}
	}
}