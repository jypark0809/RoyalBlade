using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Spear : Weapon
{
    [SerializeField]
    ParticleSystem[] _swingParticles;

    const float SPEAR_SKILL_DURATION = 8;

    public enum SwingType
    {
        First,
        Second,
    }

    SwingType swingType = SwingType.First;

    void Start()
    {
        Owner = Managers.Game.Player;
        WeaponType = Define.WeaponType.Spear;
        Damage = 100;
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

    Coroutine _coSpearSkill;
    public override void WeaponSkill()
    {
        Managers.Game.AttackCount = 0;

        if (_coSpearSkill != null)
            StopCoroutine(_coSpearSkill);
        _coSpearSkill = StartCoroutine(CoSpearSkill());
    }

    IEnumerator CoSpearSkill()
    {
        Managers.Game.AttackCount = 0;

        foreach (ParticleSystem particle in _swingParticles)
        {
            particle.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); 
        }

        yield return new WaitForSeconds(SPEAR_SKILL_DURATION);

        foreach (ParticleSystem particle in _swingParticles)
        {
            particle.gameObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
    }
}
