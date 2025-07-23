using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consts : MonoBehaviour
{
    //Anim trigger value
    public static readonly int ANIM_IDLE = Animator.StringToHash("IsIdle");
    public static readonly int ANIM_RUN = Animator.StringToHash("IsRun");
    public static readonly int ANIM_ATTACK= Animator.StringToHash("IsAttack");
    public static readonly int ANIM_WIN = Animator.StringToHash("IsWin");
    public static readonly int ANIM_DANCE = Animator.StringToHash("IsDance");
    public static readonly int ANIM_ULTI = Animator.StringToHash("IsUlti");
    public static readonly int ANIM_DEAD = Animator.StringToHash("IsDead");
    
    //Tag value
    public const string TAG_CHARACTER = "Character";
}
