using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    int _hp = 3;
    public int Hp
    {
        get { return _hp; }
        set
        {
            _hp = value;

            if (_hp <= 0)
            {
                _hp = 0;
                Managers.Game.GameOver();
            }

            (Managers.UI.SceneUI as UI_GameScene).SetPlayerHp(_hp);
        }
    }
    public int MaxHp { get; set; } = 3;
    public float CriRate { get; set; } = 0.2f;
    float _jumpPower = 20f;
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigid;
    Sword _sword;

    public enum PlayerState
    {
        Idle,
        Jump,
        Skill,
    }
    PlayerState _state = PlayerState.Idle;
    public PlayerState State { get { return _state; } }

    public override void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        _sword = GetComponentInChildren<Sword>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _state = PlayerState.Idle;
            return;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        MonsterController mc = collision.gameObject.GetComponent<MonsterController>();
        if (mc != null && _state == PlayerState.Skill)
        {
            if (!mc.IsValid())
                return;

            mc.OnCollideBySkill(this);
        }
    }

    public void Jump()
    {
        if (_state == PlayerState.Idle)
        {
            Managers.Sound.Play(Define.Sound.Effect, "Jump");
            _rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            _state = PlayerState.Jump;
            Managers.Game.JumpCount++;
        }
    }

    public void Guard()
    {
        MonsterController mc = Managers.Object.Monsters;
        if (mc != null)
        {
            float dist = mc.BottomMonster.transform.position.y - transform.position.y;
            if (Mathf.Abs(dist) <= 2f)
            {
                mc.Rigidbody.velocity = Vector2.up * 5;
                _rigid.AddForce(Vector2.down * 4f, ForceMode2D.Impulse);
                Managers.Sound.Play(Define.Sound.Effect, "Guard");
                Managers.Game.ComboCount = 0;
            }
        }
    }

    public void Attack()
    {
        _sword.Slash();
    }

    Coroutine _coSkill;
    public void SpecialSkill()
    {
        _state = PlayerState.Skill;
        
        if (_coSkill != null)
            StopCoroutine(_coSkill);
        _coSkill = StartCoroutine(CoSpecialSkill());
    }

    IEnumerator CoSpecialSkill()
    {
        Managers.Game.JumpCount = 0;
        _rigid.bodyType = RigidbodyType2D.Kinematic;
        _rigid.velocity = new Vector2(0, 30f);

        yield return new WaitForSeconds(0.5f);

        _state = PlayerState.Jump;
        _rigid.bodyType = RigidbodyType2D.Dynamic;
        _rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rigid.velocity = Vector2.zero;
        _coSkill = null;
    }

    Coroutine _coOnDamaged;
    public void OnDamaged(MonsterController attacker)
    {
        Hp--;

        if (_coOnDamaged != null)
            StopCoroutine(_coOnDamaged);
        _coOnDamaged = StartCoroutine(CoOnDamaged());
    }
    IEnumerator CoOnDamaged()
    {
        _spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);

        yield return new WaitForSeconds(1f);

        _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}
