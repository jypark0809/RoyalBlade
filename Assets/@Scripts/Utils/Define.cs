using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum Scene
    {
        Unknown,
        GameScene,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
    }

    public enum WeaponType
    {
        None,
        Sword,
        Spear,
        Wand,
    }

    public enum AttackType
    {
        Basic,
        Skill,
    }

    public const int MAX_JUMP_COUNT = 3;
    public const int MAX_ATTACK_COUNT = 10;
    public const float GUARD_COOLTIME = 3;
}
