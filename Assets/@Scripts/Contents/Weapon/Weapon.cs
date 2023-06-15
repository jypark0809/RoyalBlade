using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : BaseController
{
    Define.WeaponType _weaponType;
    public Define.WeaponType WeaponType
    {
        get { return _weaponType; }
        set { _weaponType = value; }
    }
    public Define.AttackType AttackType = Define.AttackType.Basic;
    public PlayerController Owner { get; set; }
    public int Damage { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Monster mc = collision.transform.GetComponent<Monster>();

        if (!mc.IsValid())
            return;

        if (AttackType == Define.AttackType.Basic)
            mc.OnDamaged(Owner, Damage);            // 데미지
        else if (AttackType == Define.AttackType.Skill)
            mc.OnCollideBySkill(Owner);             // 즉사
    }

    public virtual void Slash()
    {

    }

    public abstract void WeaponSkill();
}
