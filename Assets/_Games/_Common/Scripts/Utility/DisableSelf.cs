using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSelf : MonoBehaviour
{
    [SerializeField] public float delay = 2;
    [SerializeField] public bool destroy;

    private void OnEnable()
    {
        this.Invoke(delay, () =>
        {
            if (destroy)
            {
                Destroy(this.gameObject);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        });
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
