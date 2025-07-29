using UnityEngine;

public class Consts : MonoBehaviour
{
    //Anim trigger value
    public static readonly string ANIM_IDLE = ("IsIdle");
    public static readonly string ANIM_RUN = ("IsRun");
    public static readonly string ANIM_ATTACK = ("IsAttack");
    public static readonly string ANIM_WIN = ("IsWin");
    public static readonly string ANIM_DANCE = ("IsDance");
    public static readonly string ANIM_ULTI = ("IsUlti");
    public static readonly string ANIM_DEAD = ("IsDead");

    //Tag value
    public const string TAG_CHARACTER = "Character";
}


public enum ETargetType
{
    None = 0,
    Nearest = 1,
    Farthest = 2
}

// public enum PoolType
// {
//     None = 0,
//     Character = 1,
//     Bot, 
// }