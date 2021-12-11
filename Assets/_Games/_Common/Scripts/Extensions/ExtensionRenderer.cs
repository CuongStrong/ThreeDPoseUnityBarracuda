using UnityEngine;

public static class ExtensionRenderer
{
    public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    public static void SmoothDampTextureOffset(this Renderer renderer, bool horizontal, ref Vector2 currentOffset, float smoothTime)
    {
        float t = Mathf.Repeat(Time.time * smoothTime, 1f);

        if (horizontal)
        {
            currentOffset.x = t;
            currentOffset.y = 0f;
        }
        else
        {
            currentOffset.x = 0f;
            currentOffset.y = t;
        }

        renderer.sharedMaterial.SetTextureOffset("_MainTex", currentOffset);
    }
}
