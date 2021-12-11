using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoodooTracking : MonoBehaviourPersistence<VoodooTracking>
{
    public void OnGameStarted()
    {
#if VOODOO
        TinySauce.OnGameStarted();
#endif
    }

    public void OnGameStarted(int levelNumber)
    {
#if VOODOO
        TinySauce.OnGameStarted(levelNumber.ToString());
#endif
    }

    public void OnGameFinished(int score)
    {
#if VOODOO
        TinySauce.OnGameFinished(score);
#endif
    }

    void OnGameFinished(bool isUserCompleteLevel, int score)
    {
#if VOODOO
        TinySauce.OnGameFinished(isUserCompleteLevel, score);
#endif
    }

    public void OnGameFinished(bool isUserCompleteLevel, int score, int levelNumber)
    {
#if VOODOO
        TinySauce.OnGameFinished(isUserCompleteLevel, score, levelNumber.ToString());
#endif
    }
}
