using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameOver : UI_Popup
{
    enum Buttons
    {
        RestartButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Bind
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.RestartButton).gameObject.BindEvent(OnRestartButtonClicked);
        #endregion

        return true;
    }

    void OnRestartButtonClicked()
    {
        Managers.Game.Clear();
        Time.timeScale = 1;
        Managers.Scene.LoadScene(Define.Scene.GameScene);
    }
}
