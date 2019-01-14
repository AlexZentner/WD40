
using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SpriteGroupBuilder : MonoBehaviour 
{
    //public bool _useCurrentGroup;
    public string _newGroupName = "new mountains group";
    public string _newGroupSortingLayer;
    public bool _setNewParent = true;
    public Transform _newGroupParent;

    public int _maxCount;  

    public GroupComponents [] _groupComponents;
    public GenerationStages _generationStages;

    public Vector2 _xRange;
    public Vector2 _yRange;   

    [ContextMenu("GenerateSpriteGroup")]
    public void GenerateSpriteGroup()
    {
        List<GameObject> currentGroup = new List<GameObject>();

        Transform [] existingGroups = GetComponentsInChildren<Transform>();

        for (int i = 0; i < existingGroups.Length; i++)
        {
            if(existingGroups[i].gameObject != this.gameObject)
            existingGroups[i].gameObject.SetActive(false);
        }

        GameObject groupRoot = new GameObject();
        groupRoot.transform.SetParent(gameObject.transform);
        groupRoot.name = _newGroupName;

        for (int i = 0; i < _maxCount; i++)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(groupRoot.transform);
            go.transform.localPosition = Vector3.zero;
            var renderer = go.AddComponent<SpriteRenderer>();
            int n = Random.Range(0, _groupComponents.Length);
            renderer.sprite = _groupComponents[n]._sprite;
            renderer.sortingLayerName = _newGroupSortingLayer;
            currentGroup.Add(go);
        }

        RandomizePositions(currentGroup, groupRoot);
    }

    void RandomizePositions (List<GameObject> currentGroup, GameObject groupRoot)
    {
       // Debug.Log(currentGroup.Count);
        for (int i = 0; i < currentGroup.Count; i++)
        {
            currentGroup[i].transform.localPosition = new Vector2(Random.Range(_xRange.x, _xRange.y), Random.Range(_yRange.x, _yRange.y));
        }

        if (_setNewParent)
            groupRoot.transform.SetParent(_newGroupParent);
    }

    public enum GenerationStages
    {
        Init = 0, 
        RandomizePositions = 1
    }

    [Serializable]
    public class GroupComponents
    {
        public Sprite _sprite;
       // public int _weight;
    }
}


