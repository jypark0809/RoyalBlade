using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public int Hp { get; set; } = 100;
    public int MaxHp { get; set; } = 100;

    public virtual void OnDamaged(PlayerController attacker, int damage)
    {
        if (Hp <= 0)
            return;

        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead();
        }
    }

    protected void OnDead()
    {
        // Despawn
    }
}
