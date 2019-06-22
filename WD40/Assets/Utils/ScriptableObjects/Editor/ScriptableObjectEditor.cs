using UnityEditor;
using UnityEngine;

namespace WD40
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScriptableObject), true)]
    public class ScriptableObjectEditor : Editor
    {
        public Object nestedObject = new Object ();
        public string nestedName = "newNestedObject";
        public ScriptableObject nestedObjectToDestroy;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (!AssetDatabase.IsMainAsset(target))
                return;

            EditorGUILayout.Space();

            nestedObject = EditorGUILayout.ObjectField(nestedObject, typeof(MonoScript), false); 
            nestedName = EditorGUILayout.TextField("new nested object name: ", nestedName);       

            if (GUILayout.Button("Create nested"))
            {
                System.Type nestedType = (nestedObject as MonoScript).GetClass();
                         
                ScriptableObject nestedAssetObj = ScriptableObject.CreateInstance(nestedType);

                nestedAssetObj.name = nestedName;
                          
                AssetDatabase.AddObjectToAsset(nestedAssetObj, target);

                string p = AssetDatabase.GetAssetPath(nestedAssetObj);
            
                AssetDatabase.ImportAsset(p, ImportAssetOptions.DontDownloadFromCacheServer);

                AssetDatabase.Refresh(ImportAssetOptions.DontDownloadFromCacheServer);
            }

            nestedObjectToDestroy = (ScriptableObject) EditorGUILayout.ObjectField(nestedObjectToDestroy, typeof(ScriptableObject), false);         

            if (GUILayout.Button("Remove nested"))
            {
                if((nestedObjectToDestroy != null)&&(AssetDatabase.IsSubAsset(nestedObjectToDestroy)))
                {
                   DestroyImmediate(nestedObjectToDestroy, true);
                }

                string p = AssetDatabase.GetAssetPath(target);
                AssetDatabase.ImportAsset(p, ImportAssetOptions.DontDownloadFromCacheServer);

                AssetDatabase.Refresh(ImportAssetOptions.DontDownloadFromCacheServer);
            }
        }
    }
}