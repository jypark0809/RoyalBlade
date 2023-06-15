using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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
    public Weapon Weapon;

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
    }

    public void SetWeapon()
    {
        GameObject go = null;

        switch (Managers.Game.PlayerWeapon)
        {
            case WeaponType.Sword:
                go = Managers.Resource.Instantiate("Sword");
                Weapon = go.GetComponent<Sword>();
                break;
            case WeaponType.Spear:
                go = Managers.Resource.Instantiate("Spear");
                Weapon = go.GetComponent<Spear>();
                break;
            case WeaponType.Wand:
                go = Managers.Resource.Instantiate("Wand");
                Weapon = go.GetComponent<Wand>();
                break;
        }

        go.transform.position = transform.position;
        go.transform.SetParent(transform);
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
            Managers.Sound.Play(Sound.Effect, "Jump");
            _rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            _state = PlayerState.Jump;
            Managers.Game.JumpCount++;
        }
    }

    #region Guard
    public float GuardCooltime = GUARD_COOLTIME;
    Coroutine _coGuard;
    public void Guard()
    {
        // 쿨타임 중일 때
        if (_coGuard != null)
            return;
        
        // 몬스터가 없을 때
        MonsterController mc = Managers.Object.Monsters;
        if (mc == null)
            return;

        float dist = mc.BottomMonster.transform.position.y - transform.position.y;
        if (Mathf.Abs(dist) <= 2f)
        {
            mc.Rigidbody.velocity = Vector2.up * 5;
            _rigid.AddForce(Vector2.down * 4f, ForceMode2D.Impulse);
            Managers.Sound.Play(Sound.Effect, "Guard");
            Managers.Game.ComboCount = 0;

            _coGuard = StartCoroutine(CoGuard());
        }
    }

    IEnumerator CoGuard()
    {
        while (GuardCooltime > 0)
        {
            GuardCooltime -= Time.deltaTime;
            (Managers.UI.SceneUI as UI_GameScene).FillGuardCoolTime(GuardCooltime);
            yield return null;
        }

        GuardCooltime = GUARD_COOLTIME;
        _coGuard = null;
    }
    #endregion

    public void Attack()
    {
        Weapon.Slash();
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
