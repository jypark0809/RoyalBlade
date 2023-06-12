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
        Second,
        First,
        Ult,
    }

    public Define.WeaponType WeaponType { get; set; } = Define.WeaponType.Sword;
    SwingType swingType = SwingType.First;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.transform.GetComponent<MonsterController>();
        
        if (mc == null || mc.isActiveAndEnabled == false)
            return;

        mc.OnDamaged(Owner, Damage);
    }

    public void Slash()
    {
        int currentSwingType = (int)swingType;
        SetParticles(swingType);
        _swingParticles[currentSwingType].gameObject.SetActive(true);
        currentSwingType = (currentSwingType + 1) % 2;
        swingType = (SwingType)currentSwingType;
    }

    void SetParticles(SwingType swingType)
    {
        if (Managers.Game.Player == null)
            return;

        transform.position = Managers.Game.Player.transform.position;

        var main = _swingParticles[(int)swingType].main;
    }
}
