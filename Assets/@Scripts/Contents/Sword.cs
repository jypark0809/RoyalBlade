using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] _swingParticles;

    public PlayerController Owner { get; set; }
    public int Damage { get; set; } = 100;

    enum SwingType
    {
        First,
        Second,
        // Ult,
    }

    public Define.WeaponType WeaponType { get; set; } = Define.WeaponType.Sword;
    SwingType swingType = SwingType.First;

    void Start()
    {
        Owner = Managers.Game.Player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.transform.GetComponent<MonsterController>();
        
        if (!mc.IsValid())
            return;

        mc.Rigidbody.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
        mc.OnDamaged(Owner, Damage);
    }

    public void Slash()
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

    public void WeaponSkill()
    {

    }
}
