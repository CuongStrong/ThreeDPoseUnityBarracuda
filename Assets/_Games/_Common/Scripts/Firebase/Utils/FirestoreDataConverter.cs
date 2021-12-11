#if FIRESTORE
using Firebase.Firestore;
using System;
using UnityEngine;

public class FirestoreStringConverter : FirestoreConverter<string>
{
    public override object ToFirestore(string value) => value;

    public override string FromFirestore(object value)
    {
        switch (value)
        {
            case int v:
                {                    
                    return v.ToString();
                }
            case null: return "";
            default:
                {
                    return Convert.ToString(value);
                }
        }
    }
}

public class FirestoreIntConverter : FirestoreConverter<int>
{
    public override object ToFirestore(int value) => value.ToString();

    public override int FromFirestore(object value)
    {
        switch (value)
        {
            case string v:
                {
                    int result = 0;
                    int.TryParse(v, out result);
                    return result;
                }
            case null: return 0;
            default:
                {
                    return Convert.ToInt32(value);
                }
        }
    }
}

public class FirestoreLongConverter : FirestoreConverter<long>
{
    public override object ToFirestore(long value) => value.ToString();

    public override long FromFirestore(object value)
    {
        switch (value)
        {
            case string v:
                {
                    long result = 0;
                    long.TryParse(v, out result);
                    return result;
                }
            case null: return 0;
            default:
                {
                    return Convert.ToInt64(value);
                }
        }
    }
}

public class FirestoreDoubleConverter : FirestoreConverter<double>
{
    public override object ToFirestore(double value) => value.ToString();

    public override double FromFirestore(object value)
    {
        switch (value)
        {
            case string v:
                {
                    double result = 0;
                    double.TryParse(v, out result);
                    return result;
                }
            case null: return 0;
            default:
                {
                    return Convert.ToDouble(value);
                }
        }
    }
}

public class FirestoreFloatConverter : FirestoreConverter<float>
{
    public override object ToFirestore(float value) => value.ToString();

    public override float FromFirestore(object value)
    {
        switch (value)
        {
            case string v:
                {
                    float result = 0;
                    float.TryParse(v, out result);
                    return result;
                }
            case null: return 0;
            default:
                {
                    return Convert.ToSingle(value);
                }
        }
    }
}

public class FirestoreBoolConverter : FirestoreConverter<bool>
{
    public override object ToFirestore(bool value) => value.ToString();

    public override bool FromFirestore(object value)
    {
        switch (value)
        {
            case string v:
                {
                    return v.CompareTo("Y") == 0 || v.CompareTo("YES") == 0 || v.CompareTo("TRUE") == 0 || v.CompareTo("true") == 0;
                }
            case null: return false;
            default:
                {
                    return Convert.ToBoolean(value);
                }
        }
    }
}
#endif
