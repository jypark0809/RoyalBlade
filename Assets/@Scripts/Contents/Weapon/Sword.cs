using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField]
    ParticleSystem[] _swingParticles;

    public enum SwingType
    {
        First,
        Second,
        WeaponSkill,
    }

    SwingType swingType = SwingType.First;

    void Start()
    {
        Owner = Managers.Game.Player;
        WeaponType = Define.WeaponType.Sword;
        Damage = 50;
    }

    public override void Slash()
    {
        int currentSwingType = (int)swingType;
        SetParticles(swingType);
        _swingParticles[currentSwingType].gameObject.SetActive(true);
        currentSwingType = (currentSwingType + 1) % 2; // 좌,우 번갈아 가면서 베기
        swingType = (SwingType)currentSwingType;
        Util.PlaySlashSound();
    }

    void SetParticles(SwingType swingType)
    {
        if (Managers.Game.Player == null)
            return;

        transform.position = Managers.Game.Player.transform.position;
        var main = _swingParticles[(int)swingType].main;
    }

    public override void WeaponSkill()
    {
        swingType = SwingType.WeaponSkill;
        SetParticles(swingType);
        _swingParticles[(int)swingType].gameObject.SetActive(true);
        swingType = SwingType.First;
        Managers.Game.AttackCount = 0;
    }
}
