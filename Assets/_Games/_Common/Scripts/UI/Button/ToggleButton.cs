using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToggleButton : Button
{
    [SerializeField] public GameObject offImage, onImage;

    public void SetStatus(bool on)
    {
        offImage.SetActive(!on);
        onImage.SetActive(on);
    }
}
