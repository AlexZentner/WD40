using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(MonoScript))]
public sealed class MonoScriptInspector : Editor
{
	private MonoScript monoScript;
    string newName = "new Object Instance";
 
	void OnEnable()
	{        
        monoScript = (MonoScript)target;
        newName = "new "+monoScript.name + " Instance";
    }

	public override void OnInspectorGUI()
	{
		MonoScript ms = target as MonoScript;
		System.Type type = ms.GetClass();
		if (type != null && type.IsSubclassOf(typeof(ScriptableObject)) && !type.IsSubclassOf(typeof(Editor)) && !type.IsSubclassOf(typeof(EditorWindow)))
		{
            newName = EditorGUILayout.TextField("New asset name", newName);
           
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Create Instance"))
			{
				ScriptableObject asset = ScriptableObject.CreateInstance(type);
				string path = AssetDatabase.GetAssetPath(monoScript);
                path = Path.ChangeExtension(path, ".asset");
                path = path.Replace(monoScript.name, newName);             
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

