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
		public GameEventListenerFloat[] EventListenersFloat;

		public void OnEnable()
	    {
		    foreach (GameEventListener eventListener in EventListeners)
		    {
			    eventListener.Register();
		    }
		    foreach (GameEventListenerInt eventListener in EventListenersInt)
		    {
			    eventListener.Register();
		    }
			foreach (GameEventListenerFloat eventListener in EventListenersFloat)
			{
				eventListener.Register();
			}
		}

	    public void OnDisable()
	    {
		    foreach (GameEventListener eventListener in EventListeners)
		    {
			    eventListener.UnRegister();
		    }
		    foreach (GameEventListenerInt eventListener in EventListenersInt)
		    {
			    eventListener.UnRegister();
		    }
			foreach (GameEventListenerFloat eventListener in EventListenersFloat)
			{
				eventListener.UnRegister();
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
	public class UnityEventInt : UnityEvent<int>
	{

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
	public class UnityEventFloat : UnityEvent<float>
	{

	}

	[System.Serializable]
	public class GameEventListenerFloat
	{
		public GameEventFloat Event;
		public UnityEventFloat Response;

		public void OnEventRaised(float param)
		{
			Response.Invoke(param);
		}

		public void Register()
		{
			Event.RegisterListener(this);
		}

		public void UnRegister()
		{
			Event.UnregisterListener(this);
		}
	}
}