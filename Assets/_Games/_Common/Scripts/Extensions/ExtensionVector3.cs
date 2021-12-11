using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionVector3 
{
	public static Vector3 GetDirection(this Vector3 self, Vector3 goal) 
	{
		Vector3 heading   = goal - self;
		float   distance  = heading.magnitude;
		Vector3 direction = heading / distance; 

		return direction;
	}

    public static Vector2 GetVector2(this Vector3 self)
    {
        return new Vector2(self.x, self.y);
    }

    public static Vector2[] ConvertArrayVector2(this Vector3[] v3)
    {
        Vector2[] v2 = new Vector2[v3.Length];
        for (int i = 0; i < v3.Length; i++)
        {
            Vector3 tempV3 = v3[i];
            v2[i] = new Vector2(tempV3.x, tempV3.y);
        }
        return v2;
    }
}
