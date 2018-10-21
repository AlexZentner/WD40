using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WD40
{
	public class AppNavigation : MonoBehaviour 
	{
		public static void Exit()
		{		
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit ();
			#endif	
		}
	}
}
