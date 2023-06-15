using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SelectWeapon : UI_Popup
{
    enum Buttons
    {
        SwordButton,
        SpearButton,
        WandButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Bind
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.SwordButton).gameObject.BindEvent(OnSwordButtonClicked);
        GetButton((int)Buttons.SpearButton).gameObject.BindEvent(OnSpearButtonClicked);
        GetButton((int)Buttons.WandButton).gameObject.BindEvent(OnWandButtonClicked);
        #endregion

        Time.timeScale = 0;

        return true;
    }

    void OnSwordButtonClicked()
    {
        Managers.Game.PlayerWeapon = Define.WeaponType.Sword;
        Managers.Game.Player.SetWeapon();
        Time.timeScale = 1;
        ClosePopupUI();
    }

    void OnSpearButtonClicked()
    {
        Managers.Game.PlayerWeapon = Define.WeaponType.Spear;
        Managers.Game.Player.SetWeapon();
        Time.timeScale = 1;
        ClosePopupUI();
    }

    void OnWandButtonClicked()
    {
        Managers.Game.PlayerWeapon = Define.WeaponType.Wand;
        Managers.Game.Player.SetWeapon();
        Time.timeScale = 1;
        ClosePopupUI();
    }
}
