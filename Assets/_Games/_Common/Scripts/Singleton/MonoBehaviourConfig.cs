using System.Linq;
using System.Reflection;
using UnityEngine;

public class MonoBehaviourConfig<T> : MonoBehaviour where T : MonoBehaviour
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
                    string path = $"Config/{typeof(T)}";
                    GameObject prefab = Resources.Load<GameObject>(path);

                    if (prefab == null)
                    {
                        Debug.LogError(string.Format("Resources/{0} cannot found ", path));
                        return null;
                    }

                    go = Instantiate(prefab);
                    go.name = name;
                    DontDestroyOnLoad(go);
                }

                _instance = go.GetComponent<T>();
            }

            return _instance;
        }
    }

    public virtual void Init() { }
}