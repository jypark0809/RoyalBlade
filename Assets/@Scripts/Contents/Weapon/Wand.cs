using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Wand : Weapon
{
    [SerializeField] ParticleSystem[] _swingParticles;
    [SerializeField] GameObject _Laser;
    const float WAND_SKILL_DURATION = 4;

    public enum SwingType
    {
        First,
        Second,
    }

    SwingType swingType = SwingType.First;

    void Start()
    {
        Owner = Managers.Game.Player;
        WeaponType = WeaponType.Wand;
        AttackType = AttackType.Basic;
        Damage = 200;
    }

    public override void Slash()
    {
        int currentSwingType = (int)swingType;
        // SetParticles(swingType);
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

    Coroutine _coWandSkill;
    public override void WeaponSkill()
    {
        if (_coWandSkill != null)
            StopCoroutine(_coWandSkill);
        _coWandSkill = StartCoroutine(CoWandSkill());
    }

    IEnumerator CoWandSkill()
    {
        Managers.Game.AttackCount = 0;
        AttackType = Define.AttackType.Skill;
        _Laser.SetActive(true);

        yield return new WaitForSeconds(WAND_SKILL_DURATION);

        AttackType = Define.AttackType.Basic;
        _Laser.SetActive(false);
    }
}
