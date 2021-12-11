using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public partial class TermDetailPopup : BasePopup
{
    public void ButtonOkOnclick()
    {
        PlayButtonSfx();
        Hide();
    }
}
