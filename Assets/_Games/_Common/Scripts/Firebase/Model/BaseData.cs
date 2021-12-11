#if FIRESTORE
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FirestoreData]
public abstract class BaseData : EventArgs
{
    public abstract void SetDefaults();
    public abstract void UpgradeDefaults();

    public void SetValue(string key, object value)
    {
        var prop = this.GetType().GetProperty(key);
        if (prop != null)
        {
            prop.SetValue(this, value);
        }        
    }
}
#endif
