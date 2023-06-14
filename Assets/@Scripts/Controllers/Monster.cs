using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    int _hp;
    public int Hp
    {
        get { return _hp; }
        set
        {
            _hp = value;
            (Managers.UI.SceneUI as UI_GameScene).SetHpSlider(Hp, MaxHp);
        }
    }
    public int MaxHp { get; set; } = 100;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        Hp = MaxHp;
    }

    public void SetInfo(int hp)
    {
        Hp = hp;
    }

    public void OnCollideBySkill(PlayerController attacker)
    {
        Managers.Sound.Play(Define.Sound.Effect, "Boom");
        Managers.Resource.Destroy(gameObject);
    }

    public bool OnDamaged(PlayerController attacker, int damage)
    {
        bool isCritical = false;
        if (attacker != null)
        {
            //크리티컬 적용
            if (UnityEngine.Random.value <= attacker.CriRate)
            {
                damage = damage * 2;
                isCritical = true;
            }

        }

        Managers.Object.ShowDamageText(transform.position, damage, this.transform, isCritical);

        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead();
            return false;
        }

        return true;
    }

    protected void OnDead()
    {
        Managers.Game.AttackCount++;
        Managers.Sound.Play(Define.Sound.Effect, "Boom");
        Managers.Resource.Destroy(gameObject);
    }
}
