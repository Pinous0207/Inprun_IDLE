using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BackButton : UI_Base
{
    public void YesButton()
    {
        // Application.Quit() => 어플리케이션을 종료하는 함수
        Application.Quit();
    }
}
