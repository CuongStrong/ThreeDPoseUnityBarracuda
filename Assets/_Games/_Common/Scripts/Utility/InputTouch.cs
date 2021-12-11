using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class InputTouch : MonoBehaviour
{
    public static Action<Vector3> OnTouchDown = delegate { };
    public static event Action<Vector3> OnTouchUp = delegate { };
    public static event Action<Vector3, Vector3> OnTouchHold = delegate { };

    private EventSystem _eventSystem;
    protected EventSystem EventSystem
    {
        get
        {
            if (_eventSystem == null) _eventSystem = EventSystem.current;
            return _eventSystem;
        }
    }

    private Vector3 _starPos;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Application.isMobilePlatform)
        {
            int nbTouches = Input.touchCount;

            if (nbTouches > 0)
            {
                for (int i = 0; i < nbTouches; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    TouchPhase phase = touch.phase;

                    int id = touch.fingerId;

                    if (!EventSystem.IsPointerOverGameObject(id))
                    {
                        //var pos = cameraMain.ScreenToWorldPoint(touch.position);
                        var pos = touch.position;

                        if (phase == TouchPhase.Began)
                        {
                            _starPos = pos;
                            OnTouchDown.Invoke(pos);
                            break;
                        }
                        else if (phase == TouchPhase.Ended)
                        {
                            OnTouchUp.Invoke(pos);
                            break;
                        }
                        else if (phase == TouchPhase.Stationary || phase == TouchPhase.Moved)
                        {
                            var delta = (Vector3)pos - _starPos;
                            OnTouchHold.Invoke(pos, new Vector3(delta.x, 0, delta.y));
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            if (!EventSystem.IsPointerOverGameObject())
            {
                //var pos = cameraMain.ScreenToWorldPoint(Input.mousePosition);
                var pos = Input.mousePosition;

                if (Input.GetMouseButtonDown(0))
                {
                    _starPos = pos;
                    OnTouchDown.Invoke(pos);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    OnTouchUp.Invoke(pos);
                }
                else if (Input.GetMouseButton(0))
                {
                    var delta = pos - _starPos;
                    OnTouchHold.Invoke(pos, new Vector3(delta.x, 0, delta.y));
                    _starPos = pos;
                }
            }
        }
    }
}
