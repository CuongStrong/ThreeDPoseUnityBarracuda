using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public partial class ErrorPopup : BasePopup
{
    [SerializeField] Text tittleText, descText;

    public void Init(string tittle, string desc)
    {
        tittleText.text = tittle;
        descText.text = desc;
    }

    public void ButtonOkOnclick()
    {
        PlayButtonSfx();
        Hide();
    }
}
