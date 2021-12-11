using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Pool
{
    [SerializeField] public string name;
    [SerializeField] public int size = 1;
    [SerializeField] public GameObject prefab;

    public Queue<GameObject> queue = new Queue<GameObject>();
    public List<GameObject> spawnList { get; set; } = new List<GameObject>();
    public int count => queue.Count;

    private Transform parent, prefabTransform;
    private Vector3 originPos, originScale;
    private Quaternion originRot;

    public void Init(Transform root)
    {
        parent = new GameObject(name).transform;
        parent.SetParent(root);
        prefabTransform = prefab.transform;
        prefab.SetActive(false);

        originScale = prefabTransform.localScale;
        originPos = prefabTransform.localPosition;
        originRot = prefabTransform.localRotation;

        for (int i = 0; i < size; i++)
            queue.Enqueue(ClonePrefab());
    }

    GameObject ClonePrefab()
    {
        var clone = Transform.Instantiate(prefab, prefabTransform.position, prefabTransform.rotation, this.parent);
        clone.transform.localScale = originScale;
        return clone;
    }

    public GameObject Spawn(Vector3 position = default, Quaternion rotation = default, Vector3 scale = default, Transform p = null, float lifeTime = -1)
    {
        GameObject result = queue.Count > 0 ? queue.Dequeue() : ClonePrefab();
        spawnList.Add(result);

        if (p != null) result.transform.SetParent(p);
        if (lifeTime > 0)
        {
            DOTween.Sequence()
                .AppendInterval(lifeTime)
                .AppendCallback(() =>
                {
                    if (result != null) Despawn(result);
                });
        }

        result.transform.localPosition = position.Equals(default) ? originPos : position;
        result.transform.localRotation = rotation.Equals(default) ? originRot : rotation;
        result.transform.localScale = scale.Equals(default) ? originScale : scale;
        result.SetActive(true);
        result.BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);

        return result;
    }

    public GameObject Spawn(Vector3 position = default) => Spawn(position, default, default, null, -1);
    public GameObject Spawn(Transform p) => Spawn(default, default, default, p, -1);
    public GameObject Spawn(float lifeTime) => Spawn(default, default, default, null, lifeTime);
    public GameObject Spawn(Vector3 position = default, Transform p = null) => Spawn(position, default, default, p, -1);
    public GameObject Spawn(Vector3 position = default, Transform p = null, float lifeTime = -1) => Spawn(position, default, default, p, lifeTime);
    public GameObject Spawn(Vector3 position = default, float lifeTime = -1) => Spawn(position, default, default, null, lifeTime);
    public GameObject Spawn(Vector3 position = default, Quaternion rotation = default) => Spawn(position, rotation, default, null, -1);
    public GameObject Spawn(Vector3 position = default, Quaternion rotation = default, Transform p = null) => Spawn(position, rotation, default, p, -1);
    public GameObject Spawn(Vector3 position = default, Quaternion rotation = default, Transform p = null, float lifeTime = -1) => Spawn(position, rotation, default, p, lifeTime);
    public GameObject Spawn(Vector3 position = default, Quaternion rotation = default, float lifeTime = -1) => Spawn(position, rotation, default, null, lifeTime);

    public void Despawn(GameObject go)
    {
        if (go == null)
        {
            Debug.LogWarning("Despawn a null gameObject");
        }
        else
        {
            if (spawnList.Contains(go))
            {
                go.BroadcastMessage("OnDespawned", SendMessageOptions.DontRequireReceiver);
                go.SetActive(false);
                go.transform.SetParent(parent);

                spawnList.Remove(go);
                queue.Enqueue(go);
            }
            else if (queue.Contains(go))
            {
                Debug.Log(string.Format("{0} has already despawned", go.name));
            }
            else
            {
                Debug.LogWarning(string.Format("Despawn wrong pool {0}, So destroy {1}", name, go.name));
                GameObject.Destroy(go);
            }
        }
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            Debug.LogWarning("Destroy a null gameObject");
        }
        else
        {
            if (spawnList.Contains(go))
                spawnList.Remove(go);

            GameObject.Destroy(go);
        }
    }

    public void DespawnAll()
    {
        List<GameObject> list = new List<GameObject>(spawnList);
        foreach (var t in list)
            if (t != null) Despawn(t);

        spawnList.Clear();
    }

    public void DestroyAll()
    {
        foreach (var t in queue)
            if (t != null) GameObject.Destroy(t);

        queue.Clear();

        foreach (var t in spawnList)
            if (t != null) GameObject.Destroy(t);

        spawnList.Clear();
    }
}


public class PoolManager : MonoBehaviourConfig<PoolManager>
{
    [SerializeField] private List<Pool> poolListEditor;

    public Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    private void Awake()
    {
        foreach (var t in poolListEditor)
        {
            if (!IsError(t))
            {
                t.Init(transform);
                pools.Add(t.name, t);
            }
        }
    }

    public void Despawn(GameObject go)
    {
        if (go == null)
        {
            Debug.LogWarning("Despawn a null gameObject");
        }
        else
        {
            bool found = false;
            foreach (var t in pools.Values)
            {
                if (t.spawnList.Contains(go))
                {
                    found = true;
                    t.Despawn(go);
                    break;
                }
            }

            if (!found)
            {
                Debug.LogWarning(string.Format("No found pool to Despawn, So destroy {0}", go.name));
                GameObject.Destroy(go);
            }
        }
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            Debug.LogWarning("Destroy a null gameObject");
        }
        else
        {
            bool found = false;
            foreach (var t in pools.Values)
            {
                if (t.spawnList.Contains(go))
                {
                    found = true;
                    t.Destroy(go);
                    break;
                }
            }

            if (!found) GameObject.Destroy(go);
        }
    }

    public void DespawnAll()
    {
        foreach (var t in pools.Values) t.DespawnAll();
    }

    public void DestroyAll()
    {
        foreach (var t in pools.Values) t.DespawnAll();
    }

    bool IsError(Pool t)
    {
        bool error = false;

        if (string.IsNullOrEmpty(t.name))
        {
            error = true;
            Debug.LogError("PoolManager error : there is empty pool name");
        }
        if (pools.ContainsKey(t.name))
        {
            error = true;
            Debug.LogError("PoolManager error : there are duplicate pool name");
        }
        if (t.size < 1)
        {
            error = true;
            Debug.LogError("PoolManager error : there is pool size < 1");
        }
        if (t.prefab == null)
        {
            error = true;
            Debug.LogError("PoolManager error : there is pool prefab null");
        }

        return error;
    }

#if UNITY_EDITOR
    public Dictionary<string, string> GetPoolNames()
    {
        Dictionary<string, string> kq = new Dictionary<string, string>();

        foreach (var t in poolListEditor)
        {
            if (!IsError(t))
                kq.Add(t.name, t.name);
            else
                return null;
        }

        return kq;
    }
#endif

}
