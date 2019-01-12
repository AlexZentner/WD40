using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WD40
{
    [CreateAssetMenu(menuName = "ScriptableEvents" ,fileName = "newEvent")]
    public class GameEventInt : ScriptableObject 
    {
	    List<GameEventListenerInt> listeners = new List <GameEventListenerInt>();

	    public void Raise (int param) 
	    {	
		    for (int i = listeners.Count - 1; i >= 0; i--)
			    listeners [i].OnEventRaised (param);
	    }

	    public void RegisterListener (GameEventListenerInt listener) {
		    if (!listeners.Contains (listener))
			    listeners.Add (listener);		
	    }

	    public void UnregisterListener (GameEventListenerInt listener) 
	    {
		    if (listeners.Contains (listener))
			    listeners.Remove (listener);
	    }
    }
}