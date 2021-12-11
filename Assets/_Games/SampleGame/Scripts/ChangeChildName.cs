using UnityEngine;

public class ChangeChildName : MonoBehaviour
{
    private void Start()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).name = i.ToString();
        }
    }
}
