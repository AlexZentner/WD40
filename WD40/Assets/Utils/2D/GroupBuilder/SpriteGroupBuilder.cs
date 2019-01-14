
using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SpriteGroupBuilder : MonoBehaviour 
{
    public bool useCurrentGroup;

    public int _maxCount;  

    public GroupComponents _groupComponents;
    public GenerationStages _generationStages;

    public Vector2 _xRange;
    public Vector2 _yRange;   

    [ContextMenu("GenerateSpriteGroup")]
    public void GenerateSpriteGroup()
    {
        List<GameObject> currentGroup = new List<GameObject>();

        for (int i = 0; i < _maxCount; i++)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(gameObject.transform);
            go.transform.localPosition = Vector3.zero;
            go.AddComponent<SpriteRenderer>().sprite = _groupComponents._sprite;
            currentGroup.Add(go);
        }

        RandomizePositions(currentGroup);
    }

    void RandomizePositions (List<GameObject> currentGroup)
    {
        Debug.Log(currentGroup.Count);
        for (int i = 0; i < currentGroup.Count; i++)
        {
            currentGroup[i].transform.localPosition = new Vector2(Random.Range(_xRange.x, _xRange.y), Random.Range(_yRange.x, _yRange.y));
        }
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
        public int _weight;
    }
}


