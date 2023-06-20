using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : BaseController
{
    int _hp;
    public int Hp
    {
        get { return _hp; }
        set
        {
            _hp = value;

            if (IsBottom)
                (Managers.UI.SceneUI as UI_GameScene).SetHpSlider(Hp, MaxHp);
        }
    }
    public int MaxHp { get; set; }
    public bool IsBottom { get; set; } = false;
    MonsterController _mc;

    public override void Init()
    {
        
    }

    public void SetInfo(int hp, MonsterController mc)
    {
        MaxHp = hp;
        Hp = MaxHp;
        _mc = mc;
    }

    public void OnCollideBySkill(PlayerController attacker)
    {
        OnDead();
    }

    public bool OnDamaged(PlayerController attacker, int damage)
    {
        #region Critical
        //bool isCritical = false;
        //if (attacker != null)
        //{
        //    //크리티컬 적용
        //    if (UnityEngine.Random.value <= attacker.CriRate)
        //    {
        //        damage = damage * 2;
        //        isCritical = true;
        //    }

        //}
        #endregion

        Managers.Game.ComboCount++;
        Managers.Object.ShowDamageText(transform.position, damage);

        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            Managers.Game.AttackCount++;
            OnDead();
            return false;
        }

        return true;
    }

    protected void OnDead()
    {
        Managers.Object.ShowDestroyEffect(transform.position);
        Managers.Sound.Play(Define.Sound.Effect, "Boom");
        Managers.Resource.Destroy(gameObject);
        _mc.IsMonsterStackEmpty();
    }
}
