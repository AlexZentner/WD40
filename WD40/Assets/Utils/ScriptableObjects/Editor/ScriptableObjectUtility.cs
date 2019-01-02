using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Reflection;

namespace WD40
{
    [CustomEditor(typeof(MonoScript))]
    public sealed class ScriptableObjectUtility : Editor
    {
        private MonoScript monoScript;
        int newInstanceCount = 1;

        public Object [] nestedObject = new Object [1];
        string newName = "new Object Instance";

        bool nestedfoldout = false;
        int nestedCount = 0;

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

                nestedfoldout = EditorGUILayout.Foldout(nestedfoldout, new GUIContent("Nested assets: "));

                if(nestedfoldout)
                {
                    EditorGUI.BeginChangeCheck();
                    nestedCount = EditorGUILayout.IntField("nested count", nestedCount);
                    if  (EditorGUI.EndChangeCheck())
                    {
                        Object[] changedNestedObject = new Object[nestedCount];

                        for (int n = 0; n < changedNestedObject.Length; n ++)
                        {
                            if(n < nestedObject.Length)
                            {
                                changedNestedObject[n] = nestedObject[n];
                            }
                        }

                        nestedObject = changedNestedObject;
                    }

                    for (int i = 0; i < nestedCount; i++)
                    {
                        nestedObject[i] = EditorGUILayout.ObjectField(nestedObject[i], typeof(MonoScript), false);
                    }
                }
            
                newInstanceCount = Mathf.Clamp(newInstanceCount, 0, int.MaxValue);

                serializedObject.ApplyModifiedProperties();

                if (GUILayout.Button("Create Asset(s)"))
                {
                    for (int i = 0; i < newInstanceCount; i++)
                    {
                        string indexToName = i > 0 ? i.ToString() : "";

                        ScriptableObject asset = ScriptableObject.CreateInstance(type);                       

                        string path = AssetDatabase.GetAssetPath(monoScript);
                        path = Path.ChangeExtension(path, ".asset");
                        path = path.Replace(monoScript.name, (newName + indexToName));

                        AssetDatabase.CreateAsset(asset, path);                       

                        Object[] objs = new Object [0];

                        for (int y = 0; y < nestedObject.Length; y++)
                        {
                            if (nestedObject[y] == null)
                                continue;

                            System.Type nestedType = (nestedObject[y] as MonoScript).GetClass();
                         
                            ScriptableObject nestedAssetObj = ScriptableObject.CreateInstance(nestedType);

                            nestedAssetObj.name = string.Format("nested asset " + y);
                          
                           AssetDatabase.AddObjectToAsset(nestedAssetObj, asset);
                           
                            objs = AssetDatabase.LoadAllAssetsAtPath(path);                          
                        }

                        foreach (var o in objs)
                        {
                           string p = AssetDatabase.GetAssetPath(o);
                           AssetDatabase.ImportAsset(p, ImportAssetOptions.DontDownloadFromCacheServer);
                        }

                        AssetDatabase.Refresh(ImportAssetOptions.DontDownloadFromCacheServer);
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

