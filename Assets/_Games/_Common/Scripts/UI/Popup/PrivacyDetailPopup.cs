using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public partial class PrivacyDetailPopup : BasePopup
{
    public void ButtonOkOnclick()
    {
        PlayButtonSfx();
        Hide();
    }
}
