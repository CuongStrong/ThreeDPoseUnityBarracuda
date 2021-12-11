#if FIRESTORE
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[FirestoreData]
public class BaseModel<T> where T:BaseData, new()
{
    [FirestoreProperty]
    public T data { get; set; }

    public bool isReady;

    public string Name;
    public event EventHandler Changed;

    protected DocumentReference docRef;

    public BaseModel()
    {
        data = new T();
        isReady = false;
    }

    public virtual async Task Start(string userId, string envSuffix)
    {
        isReady = false;

        var db = DataManager.Instance.firestoreInstance;
        docRef = db.Collection(Name + "_" + envSuffix).Document(userId);

        Debug.Log(Name + "_" + envSuffix);

        var snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            data = snapshot.ConvertTo<T>();
            data.UpgradeDefaults();
        }
        else
        {
            data.SetDefaults();
            await docRef.SetAsync(data);
        }

        isReady = true;
        InvokeChange();
    }

    public virtual void Stop()
    {
        UnregisterEvents();

        isReady = false;
        docRef = null;
    }

    public virtual void UnregisterEvents()
    {
        Changed = delegate { };
    }

    public void Update(Dictionary<string, object> updates)
    {
        if (docRef == null)
        {
            return;
        }

        docRef.UpdateAsync(updates);

        foreach (var kvp in updates)
        {
            data.SetValue(kvp.Key, kvp.Value);
        }

        InvokeChange();        
    }

    public void Update(string key, object value)
    {
        if (docRef == null)
        {
            return;
        }

        docRef.UpdateAsync(key, value);

        data.SetValue(key, value);

        InvokeChange();
    }

    public void InvokeChange()
    {
        Changed?.Invoke(this, data);
    }
}
#endif
