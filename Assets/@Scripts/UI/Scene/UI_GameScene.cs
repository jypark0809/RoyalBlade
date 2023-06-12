using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    #region Enum
    enum Buttons
    {
        JumpButton,
        GuardButton,
        AttackButton,
    }
    #endregion

    #region Action
    // public event Action<Vector2> OnMoveDirChanged;
    public Action OnPlayerJump;
    public Action OnPlayerGuard;
    public Action OnPlayerAttack;
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Bind
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.JumpButton).gameObject.BindEvent(OnJumpButtonClicked);
        GetButton((int)Buttons.GuardButton).gameObject.BindEvent(OnGuardButtonClicked);
        GetButton((int)Buttons.AttackButton).gameObject.BindEvent(OnAttackButtonClicked);
        #endregion

        return true;
    }

    #region EventHandler
    void OnJumpButtonClicked()
    {
        OnPlayerJump?.Invoke();
    }

    void OnGuardButtonClicked()
    {
        OnPlayerGuard?.Invoke();
    }

    void OnAttackButtonClicked()
    {
        OnPlayerAttack?.Invoke();
    }
    #endregion
}
