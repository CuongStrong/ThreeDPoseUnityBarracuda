using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionVector2 
{
    public static Vector3[] ConvertArrayVector3(this Vector2[] v2)
    {
        Vector3[] v3 = new Vector3[v2.Length];
        for (int i = 0; i < v2.Length; i++)
        {
            Vector2 tempV2 = v2[i];
            v3[i] = new Vector3(tempV2.x, tempV2.y, 0);
        }
        return v3;
    }
}
