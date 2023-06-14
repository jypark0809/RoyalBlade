using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
{
    const float MAX_SPEED = 10f;

    int _monsterCount = 8;
    Stack<Monster> _monsterStack = new Stack<Monster>();
    public Monster BottomMonster { get; set; }
    public Rigidbody2D Rigidbody { get; set; }

    public override void Init()
    {
        base.Init();
        for (int i = _monsterCount; i > 0; i--)
        {
            GameObject go = Managers.Resource.Instantiate("Monster", transform);
            go.transform.localPosition = new Vector2(0, (i-1) * 2);
            go.name = $"Monster_{i}";
        }

        Monster[] monsters = GetComponentsInChildren<Monster>();
        foreach (Monster monster in monsters)
        {
            int waveIndex = Managers.Game.CurrentWaveIndex;
            _monsterStack.Push(monster); // 맨 위에 있는 블럭부터 Push
        }

        BottomMonster = _monsterStack.Peek();
        (Managers.UI.SceneUI as UI_GameScene).SetHpSlider(BottomMonster.Hp, BottomMonster.MaxHp);

        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // 최대 속도 제한
        if (Rigidbody.velocity.magnitude > MAX_SPEED)
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, (-1) * MAX_SPEED);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc != null && pc.State == PlayerController.PlayerState.Idle)
        {
            pc.OnDamaged(this);
            Rigidbody.velocity = Vector2.up * 5;
        }
    }

    public void OnCollideBySkill(PlayerController attacker)
    {
        BottomMonster.OnCollideBySkill(attacker);
        OnDead();
    }

    public virtual void OnDamaged(PlayerController attacker, int damage)
    {
        if (_monsterStack.Count == 0)
            return;

        Managers.Game.ComboCount++;

        // 몬스터가 죽었으면
        if (!BottomMonster.OnDamaged(attacker, damage))
            OnDead();
    }

    protected void OnDead()
    {
        _monsterStack.Pop();

        if (_monsterStack.Count == 0)
        {
            Managers.Object.Despawn(this);
            Managers.Game.CurrentWaveIndex++;
            return;
        }

        BottomMonster = _monsterStack.Peek();
        (Managers.UI.SceneUI as UI_GameScene).SetHpSlider(BottomMonster.Hp, BottomMonster.MaxHp);
    }
}
