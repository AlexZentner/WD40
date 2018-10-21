using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WD40
{
	public class DebugHelper 
	{
		public static void LogWarning(string text)
		{
			#if UNITY_EDITOR
			Debug.LogWarning (text);
			#endif
		}
		
		public static void LogWarning(object obj)
		{
			#if UNITY_EDITOR
			Debug.LogWarning (obj.ToString());
			#endif
		}

		public static void LogError(string text)
		{
			#if UNITY_EDITOR
			Debug.LogWarning (text);
			#endif
		}

		public static void LogError(object obj)
		{
			#if UNITY_EDITOR
			Debug.LogWarning (obj);
			#endif
		}

        public static void DebugBreak ()
        {
			#if UNITY_EDITOR
            Debug.Break();
			#endif
        }
    }
}
