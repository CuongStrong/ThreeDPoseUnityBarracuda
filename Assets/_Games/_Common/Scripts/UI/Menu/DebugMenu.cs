using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public partial class DebugMenu : BaseMenu
{
    [SerializeField] Text stageText;
    [SerializeField] GameObject morePanel;

    private static bool offDebug;

    private void OnEnable()
    {
        // OnEnablePartial();
        ButtonDebugOnClick(false);
        stageText.text = string.Format("Lv.{0}", DataSave.Instance.level);
    }

    public void ButtonStageOnClick(bool next)
    {
        Define.NextStage(next);
        SaveGameManager.Instance.Save();
        Define.ReloadScene();
    }

    public void ButtonResetAllOnClick()
    {
        SaveGameManager.ResetDataSave();
        Define.ReloadScene();
    }

    public void ButtonRetryOnClick()
    {
        Define.ReloadScene();
    }

    public void ButtonMoreOnClick()
    {
        morePanel.gameObject.SetActive(true);
    }

    public void ButtonClosePanelOnClick()
    {
        morePanel.gameObject.SetActive(false);
    }

    public void ButtonDebugOnClick(bool clicked = true)
    {
        if (clicked)
        {
            offDebug = !offDebug;
        }

        canvasGroup.alpha = offDebug ? 0 : 1;
    }
}

