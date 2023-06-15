using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager
{
    public PlayerController Player { get; set; }
    public Define.WeaponType PlayerWeapon = Define.WeaponType.None;

    public Action OnWaveIndexChanged;
    public Action<int> OnComboCountChanged;
    public Action OnJumpCountChanged;
    public Action OnAttackCountChanged;

    int _currentWaveIndex = 1;
    public int CurrentWaveIndex
    {
        get { return _currentWaveIndex; }
        set
        {
            _currentWaveIndex = value;
            OnWaveIndexChanged?.Invoke();
        }
    }
    int _comboCount;
    public int ComboCount
    {
        get { return _comboCount; }
        set
        {
            _comboCount = value;
            OnComboCountChanged?.Invoke(_comboCount);
        }
    }
    int _jumpCount;
    public int JumpCount
    {
        get { return _jumpCount; }
        set
        {
            _jumpCount = value;

            if (_jumpCount >= Define.MAX_JUMP_COUNT)
                _jumpCount = Define.MAX_JUMP_COUNT;

            OnJumpCountChanged?.Invoke();
        }
    }
    int _attackCount;
    public int AttackCount
    {
        get { return _attackCount; }
        set
        {
            _attackCount = value;

            if (_attackCount >= Define.MAX_ATTACK_COUNT)
                _attackCount = Define.MAX_ATTACK_COUNT;

            OnAttackCountChanged?.Invoke();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        Managers.UI.ShowPopupUI<UI_GameOver>();
    }

    public void Clear()
    {
        _currentWaveIndex = 1;
        _comboCount = 0;
        _jumpCount = 0;
        _attackCount = 0;

        OnWaveIndexChanged = null;
        OnComboCountChanged = null;
        OnJumpCountChanged = null;
        OnAttackCountChanged = null;
    }
}
