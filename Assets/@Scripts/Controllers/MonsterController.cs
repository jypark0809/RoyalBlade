using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
{
    const float MAX_SPEED = 10f;
    const int MONSTER_SPAWN_COUNT = 8;

    public Stack<Monster> MonsterStack = new Stack<Monster>();
    public Monster BottomMonster { get; set; }
    public Rigidbody2D Rigidbody { get; set; }

    public override void Init()
    {
        base.Init();
        Rigidbody = GetComponent<Rigidbody2D>();

        for (int i = MONSTER_SPAWN_COUNT; i > 0; i--)
        {
            GameObject go = Managers.Resource.Instantiate("Monster", transform);
            go.transform.localPosition = new Vector2(0, (i-1) * 2);
            go.name = $"Monster_{i}";
        }

        Monster[] monsters = GetComponentsInChildren<Monster>();
        foreach (Monster monster in monsters)
        {
            int curWave = Managers.Game.CurrentWaveIndex;
            int monsterHp = (int)Mathf.Pow(2, curWave); 
            monster.SetInfo(monsterHp, this);
            MonsterStack.Push(monster); // 맨 위에 있는 블럭부터 스택에 Push
        }

        BottomMonster = MonsterStack.Peek();
        BottomMonster.IsBottom = true;
        (Managers.UI.SceneUI as UI_GameScene).SetHpSlider(BottomMonster.Hp, BottomMonster.MaxHp);
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
            Managers.Sound.Play(Define.Sound.Effect, "OnDamaged");
        }
    }

    public void OnCollideBySkill(PlayerController attacker)
    {
        BottomMonster.OnCollideBySkill(attacker);
    }

    public void IsMonsterStackEmpty()
    {
        MonsterStack.Pop();

        if (MonsterStack.Count == 0)
        {
            Managers.Object.Despawn(this);
            Managers.Game.CurrentWaveIndex++;
            return;
        }

        BottomMonster = MonsterStack.Peek();
        BottomMonster.IsBottom = true;
        (Managers.UI.SceneUI as UI_GameScene).SetHpSlider(BottomMonster.Hp, BottomMonster.MaxHp);
    }
}
