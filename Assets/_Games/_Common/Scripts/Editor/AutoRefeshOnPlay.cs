#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Utility
{
    [InitializeOnLoad]
    public static class AutoRefeshOnPlay
    {
        static AutoRefeshOnPlay()
        {
            EditorApplication.playModeStateChanged += AutoRefeshOnPlay.OnEditorApplicationPlayModeStateChanged;
        }

        private static void OnEditorApplicationPlayModeStateChanged(PlayModeStateChange playingState)
        {
            switch (playingState)
            {
                // Called the moment after the user presses the Play button.
                case PlayModeStateChange.ExitingEditMode:
                    if (!EditorPrefs.GetBool("kAutoRefresh")) AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
                    break;
            }
        }
    }
}
#endif //#if UNITY_EDITOR
