using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.EditorCoroutines;
using Unity.EditorCoroutines.Editor;

namespace WD40
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScriptableObject), true)]
    public class ScriptableObjectEditor : Editor
    {
        public Object nestedObject = new Object ();
        public string nestedName = "newNestedObject";
        public ScriptableObject nestedObjectToDestroy;
       
        Object newObject;

        static Object lastNestedOBject;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (!AssetDatabase.IsMainAsset(target))
                return;

            EditorGUILayout.Space();
           
            nestedObject = EditorGUILayout.ObjectField(nestedObject, typeof(MonoScript), false);

            if (nestedObject == null && lastNestedOBject != null)
                nestedObject = lastNestedOBject;

            else if (nestedObject != null && lastNestedOBject == null)
                lastNestedOBject = nestedObject;

            nestedName = EditorGUILayout.TextField("new nested object name: ", nestedName);

            if (GUILayout.Button("Create nested"))
            {
                System.Type nestedType = (nestedObject as MonoScript).GetClass();

                ScriptableObject nestedAssetObj = ScriptableObject.CreateInstance(nestedType);

                nestedAssetObj.name = nestedName + Time.time;

                AssetDatabase.AddObjectToAsset(nestedAssetObj, target);

                // string dir = Path.GetDirectoryName(mp);

                // Debug.Log(dir);
                string newAssetPath = AssetDatabase.GetAssetPath(nestedAssetObj);
                AssetDatabase.ImportAsset(newAssetPath);
                AssetDatabase.Refresh();
                EditorCoroutineUtility.StartCoroutineOwnerless(ExposeNewNestedAsset());

                Object[] objs = new Object[0];
                objs = AssetDatabase.LoadAllAssetsAtPath(newAssetPath);
               
                if(objs.Length <= 0)
                {
                    Debug.LogErrorFormat ("Assets not found at path {0}", newAssetPath);
                    return;
                }
                // last created is a first object in array
                newObject = objs[0];
             
                if(newObject != null)
                Debug.LogFormat("Nested asset {0} was added to {1} successfully", newObject.name, target.name);
            }

            nestedObjectToDestroy = (ScriptableObject) EditorGUILayout.ObjectField(nestedObjectToDestroy, typeof(ScriptableObject), false);         

            if (GUILayout.Button("Remove nested"))
            {
                if((nestedObjectToDestroy != null)&&(AssetDatabase.IsSubAsset(nestedObjectToDestroy)))
                {
                   Debug.Log(AssetDatabase.GetAssetPath(nestedObjectToDestroy));
                   DestroyImmediate(nestedObjectToDestroy, true);
                }

                newObject = target;

                string p = AssetDatabase.GetAssetPath(target);
                AssetDatabase.ImportAsset(p);
                AssetDatabase.Refresh();

                Object[] objs = new Object[0];
                objs = AssetDatabase.LoadAllAssetsAtPath(p);

                if (objs.Length > 0)
                {
                    newObject = objs[objs.Length-1];
                }

                EditorCoroutineUtility.StartCoroutineOwnerless(ExposeDeletedAsset());
            }
        }

        IEnumerator ExposeNewNestedAsset()
        {
            yield return null;

            Object initSelection = Selection.activeObject;
            ColapseFolders();

            yield return new WaitForSecondsRealtime(.2f);

            Selection.activeObject = initSelection;
            EditorGUIUtility.PingObject(newObject);
        }

        IEnumerator ExposeDeletedAsset()
        {
            yield return null;

            Object initSelection = Selection.activeObject;
            ColapseFolders();

            yield return new WaitForSecondsRealtime(.2f);

            Selection.activeObject = initSelection;
            EditorGUIUtility.PingObject(newObject);
        }

        void ColapseFolders ()
        {
            FileSystemInfo[] rootItems = new DirectoryInfo(Application.dataPath).GetFileSystemInfos();
            List<Object> rootItemsList = new List<Object>(rootItems.Length);

            for (int i = 0; i < rootItems.Length; i++)
            {
                Object asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/" + rootItems[i].Name);

                if (asset != null)
                {
                    rootItemsList.Add(asset);
                }
            }

            if (rootItemsList.Count > 0)
            {
                EditorUtility.FocusProjectWindow();
                Selection.objects = rootItemsList.ToArray();
            }

            EditorWindow focusedWindow = EditorWindow.focusedWindow;
            if (focusedWindow != null)
            {
                focusedWindow.SendEvent(new Event { keyCode = KeyCode.LeftArrow, type = EventType.KeyDown, alt = true });
            }
        }
    }
}