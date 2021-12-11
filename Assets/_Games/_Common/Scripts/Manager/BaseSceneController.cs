using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseSceneController : MonoBehaviour
{
    protected float checkNetworkTime;

    void LateUpdate()
    {
        checkNetworkTime += Time.deltaTime;
        if (checkNetworkTime > 2)
        {
            if (!Utility.isInternetAvailable)
                ShowNoConnectionPopup();

            checkNetworkTime = 0;
        }
    }

    void ShowNoConnectionPopup()
    {
        UIManager.Instance.GetPopup(ResourcesPath.PREFAB_NO_CONNECTION_POPUP).Show();
    }
}
