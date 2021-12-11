using UnityEngine;

public class MonoBehaviourPersistence<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                string name = typeof(T).Name;
                var go = GameObject.Find(name);
                if (go == null)
                {
                    go = new GameObject(name);
                    DontDestroyOnLoad(go);

                    _instance = go.AddComponent<T>();
                }
                else
                {
                    _instance = go.GetComponent<T>();
                }
            }

            return _instance;
        }
    }

    public virtual void Init() { }
}