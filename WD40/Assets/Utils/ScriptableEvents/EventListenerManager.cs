using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WD40
{
    public class EventListenerManager : MonoBehaviour 
    {
	    public GameEventListener[] EventListeners;
	    public GameEventListenerInt[] EventListenersInt;

	    public void OnEnable()
	    {
		    foreach (GameEventListener eventListener in EventListeners)
		    {
			    eventListener.Register();
		    }
		    foreach (GameEventListenerInt eventListenerInt in EventListenersInt)
		    {
			    eventListenerInt.Register();
		    }
	    }

	    public void OnDisable()
	    {
		    foreach (GameEventListener eventListener in EventListeners)
		    {
			    eventListener.UnRegister();
		    }
		    foreach (GameEventListenerInt eventListenerInt in EventListenersInt)
		    {
			    eventListenerInt.UnRegister();
		    }
	    }	
    }

    [System.Serializable]
    public class GameEventListener
    {
	    public GameEvent Event;
	    public UnityEvent Response;

	    public void OnEventRaised()
	    {
		    Response.Invoke ();
	    }

	    public void Register ()
	    {
		    Event.RegisterListener (this);
	    }

	    public void UnRegister ()
	    {
		    Event.UnregisterListener (this);
	    }
    }

    [System.Serializable]
    public class GameEventListenerInt
    {
	    public GameEventInt Event;
	    public UnityEventInt Response;

	    public void OnEventRaised(int param)
	    {
		    Response.Invoke (param);
	    }

	    public void Register ()
	    {
		    Event.RegisterListener (this);
	    }

	    public void UnRegister ()
	    {
		    Event.UnregisterListener (this);
	    }
    }

    [System.Serializable] 
    public class UnityEventInt: UnityEvent <int>
    {
	
    }
}