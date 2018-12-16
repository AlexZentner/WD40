using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace WD40
{
    [CustomEditor(typeof(MonoScript))]
    public sealed class ScriptableObjectUtility : Editor
    {
        private MonoScript monoScript;
        string newName = "new Object Instance";
        int newInstanceCount = 1;

        void OnEnable()
        {
            monoScript = (MonoScript)target;
            newName = "new " + monoScript.name + " Instance";
        }

        public override void OnInspectorGUI()
        {
            MonoScript ms = target as MonoScript;
            System.Type type = ms.GetClass();
            if (type != null && type.IsSubclassOf(typeof(ScriptableObject)) && !type.IsSubclassOf(typeof(Editor)) && !type.IsSubclassOf(typeof(EditorWindow)))
            {
                newName = EditorGUILayout.TextField("New asset name", newName);
                newInstanceCount = EditorGUILayout.IntField("new assets count", newInstanceCount);

                newInstanceCount = Mathf.Clamp(newInstanceCount, 0, int.MaxValue);

                serializedObject.ApplyModifiedProperties();

                if (GUILayout.Button("Create Instance(s)"))
                {
                    for (int i = 0; i < newInstanceCount; i++)
                    {
                        string indexToName = i > 0 ? i.ToString() : "";

                        ScriptableObject asset = ScriptableObject.CreateInstance(type);
                        string path = AssetDatabase.GetAssetPath(monoScript);
                        path = Path.ChangeExtension(path, ".asset");
                        path = path.Replace(monoScript.name, (newName + indexToName));
                        AssetDatabase.CreateAsset(asset, path);
                        EditorGUIUtility.PingObject(asset);
                    }
                }
            }
            else
            {
                DrawDefaultInspector();
            }
        }
    }
}

