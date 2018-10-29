
#if UNITY_EDITOR

using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
//using System.Reflection;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(MonoScript))]
public sealed class MonoScriptInspector : Editor
{
	private MonoScript monoScript;

	void OnEnable()
	{
		monoScript = (MonoScript)target;
	}

	public override void OnInspectorGUI()
	{
		MonoScript ms = target as MonoScript;
		System.Type type = ms.GetClass();
		if (type != null && type.IsSubclassOf(typeof(ScriptableObject)) && !type.IsSubclassOf(typeof(Editor)) && !type.IsSubclassOf(typeof(EditorWindow)))
		{
			if (GUILayout.Button("Create Instance"))
			{
				ScriptableObject asset = ScriptableObject.CreateInstance(type);
				string path = AssetDatabase.GetAssetPath(monoScript);
				path = path.Remove(path.Length - 3) + ".asset";
				AssetDatabase.CreateAsset(asset, path);
				EditorGUIUtility.PingObject(asset);
			}
		}
		else
		{
			DrawDefaultInspector();
		}
	}
}
#endif
