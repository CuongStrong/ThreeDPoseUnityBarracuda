using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class MainMenu : BaseMenu
{
    [SerializeField] Text coinText, levelText;
    [SerializeField] Button pauseButton;

    void Start()
    {
        //Buttons   
        pauseButton.onClick.AddListener(() =>
        {
            UIManager.Instance.GetPopup(ResourcesPath.PREFAB_SETTING_POPUP).Show();
            PlayButtonSfx();
        });
    }

    private void OnEnable()
    {
        DataSave.OnCoinChanged += OnCoinChanged;

        OnCoinChanged(DataSave.Instance.coin);
        levelText.text = string.Format("Level {0}", DataSave.Instance.level);
    }
    private void OnDisable()
    {
        DataSave.OnCoinChanged -= OnCoinChanged;
    }

    void OnCoinChanged(int coin)
    {
        coinText.text = coin.ToString();
    }

}
