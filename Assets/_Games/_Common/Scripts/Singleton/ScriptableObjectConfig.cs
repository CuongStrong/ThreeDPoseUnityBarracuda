using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectConfig<T> : ScriptableObject where T : ScriptableObject
{
    private static T _entity;
    public static T Entity
    {
        get
        {
            if (!_entity)
            {
                string path = $"Config/{typeof(T)}";
                _entity = Resources.Load<T>(path);

                if (_entity == null) Debug.LogError(string.Format("Resources/{0} cannot found ", path));
            }

            return _entity;
        }
    }

    public virtual void Init() { }
}
