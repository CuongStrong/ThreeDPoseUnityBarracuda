using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SimpleFPS : MonoBehaviour
{
    private Text fpsText;
    private float deltaTime, fps;

    private void Awake()
    {
        fpsText = GetComponent<Text>();
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
        fpsText.text = string.Format("{0:0.} fps", fps);
    }

}
