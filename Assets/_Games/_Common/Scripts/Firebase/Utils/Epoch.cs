using UnityEngine;
using System;

public static class Epoch
{
    public static long ONE_DAY = 24 * 3600;
    public static long ONE_HOUR = 3600;

    public static long Current()
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        long currentEpochTime = (long)(DateTime.UtcNow - epochStart).TotalSeconds;

        return currentEpochTime;
    }

    public static long SecondsElapsed(long t1)
    {
        long difference = Current() - t1;

        return Math.Abs(difference);
    }

    public static long SecondsElapsed(long t1, long t2)
    {
        long difference = t1 - t2;

        return Math.Abs(difference);
    }

}
