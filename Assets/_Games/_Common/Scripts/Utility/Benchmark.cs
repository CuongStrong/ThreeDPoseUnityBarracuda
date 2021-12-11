using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Benchmark
{
    static System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

    public static void Start()
    {
        sw.Reset();
        sw.Start();
    }

    public static void Stop(string action = "")
    {
        sw.Stop();

        Debug.Log ( string.Format("Benchmark {0} got {1} ms ~ {2} sticks", action , sw.ElapsedMilliseconds, sw.ElapsedTicks));
    }
}
