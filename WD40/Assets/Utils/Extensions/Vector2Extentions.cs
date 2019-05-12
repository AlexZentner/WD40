using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extentions
{
    public static int GetRandom (this Vector2Int range)
    {
        return Random.Range(range.x, range.y);
    }

    public static float GetRandom (this Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }
}
