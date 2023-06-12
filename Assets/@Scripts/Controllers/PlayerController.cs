using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Hp { get; set; } = 3;
    public int MaxHp { get; set; } = 3;
    Rigidbody2D _rigid;
    float _jumpPower = 20;
    int _jumpCount = 0;
    Sword _sword;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _sword = GetComponentInChildren<Sword>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            _jumpCount++;
    }

    public void Jump()
    {
        if (_jumpCount > 0)
        {
            Debug.Log("Jump");
            _rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            _jumpCount--;
        }
    }

    public void Guard()
    {
        Debug.Log("Guard");
    }

    public void Attack()
    {
        Debug.Log("Attack");
        _sword.Slash();
    }
}
