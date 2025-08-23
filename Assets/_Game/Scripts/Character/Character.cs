using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Character : GameUnit
{
    public const float ATTACK_RANGE = 5f;
    public const float TIME_ON_DEATH = 1f;
    public const float TIME_ON_COOLDOWN = 1f;
    public const float MAX_SIZE = 2.5f;
    public const float MIN_SIZE = 1f;
    public const float TIME_DELAY_THROW = 0.2f;

    public Character target;

    [SerializeField] public Collider coll;
    [SerializeField] GameObject insightMasker;
    [SerializeField] GameObject rangeAttackOutline;
    [SerializeField] protected Skin currentSkin;

    private string animName;
    private int score;
    private Vector3 targetPoint;
    protected bool isCanAttack = true;
    protected float size = 1;


    public int Score => score;
    public bool IsCanAttack { get; set; }
    public bool IsDead { get; protected set; }

    public virtual void OnInit()
    {
        IsDead = false;
        score = 0;
        isCanAttack = true;
        IsCanAttack = true;

        WearClothes();
    }

    public void ChangeAnim(string animName)
    {
        if (this.animName != animName)
        {
            currentSkin.Anim.ResetTrigger(this.animName);
            this.animName = animName;
            currentSkin.Anim.SetTrigger(this.animName);
        }
    }

    public virtual void OnAttack()
    {
        target = GetTargetInRange();

        if (target != null && !target.IsDead)
        {
            targetPoint = target.TF.position;
            TF.LookAt(targetPoint + (TF.position.y - targetPoint.y) * Vector3.up);
            ChangeAnim(Constant.ANIM_ATTACK);
        }

    }



    public Character GetTargetInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.TF.position, ATTACK_RANGE);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(Constant.TAG_CHARACTER) && hitCollider != coll)
            {
                return hitCollider.GetComponent<Character>();
            }
        }

        return null;
    }

    public virtual void Throw()
    {
        currentSkin.Weapon.Throw(this, targetPoint, size);
    }

    public void OnHit(UnityAction hitAction)
    {
        if (!IsDead)
        {
            IsDead = true;
            hitAction.Invoke();
            OnDeath();
        }
    }
    public void ResetAnim()
    {
        animName = Constant.ANIM_IDLE;
        ChangeAnim(animName);
    }

    public virtual void OnDespawn()
    {
        TakeOffClothes();
        SimplePool.Despawn(this);
    }
    public virtual void OnDeath()
    {
        // Debug.Log("Is Checked");
        ChangeAnim(Constant.ANIM_DIE);
        LevelManager.Ins.CharecterDeath(this);
    }

    public virtual void OnMoveStop() { }


    public virtual void WearClothes()
    {

    }

    public void ActiveMaker()
    {
        insightMasker.SetActive(true);
    }

    public void DeactiveMaker()
    {
        insightMasker.SetActive(false);
    }

    public void AddScore(int amount = 1)
    {
        SetScore(score + amount);
    }

    public void SetScore(int score)
    {
        this.score = score > 0 ? score : 0;
        // indicator.SetScore(this.score);
        SetSize(1 + this.score * 0.1f);
    }

    protected virtual void SetSize(float size)
    {
        size = Mathf.Clamp(size, MIN_SIZE, MAX_SIZE);
        this.size = size;

        rangeAttackOutline.transform.parent = null;
        TF.localScale = size * Vector3.one;
        rangeAttackOutline.transform.parent = TF;
    }


    public virtual void TakeOffClothes()
    {
        currentSkin?.OnDespawn();
        SimplePool.Despawn(currentSkin);
    }
    public void ChangeWeapon(WeaponType weaponType)
    {
        currentSkin.ChangeWeapon(weaponType);
    }

    public void ChangeSkin(SkinType skinType)
    {
        currentSkin = SimplePool.Spawn<Skin>((PoolType)skinType, TF);
    }

    public void ChangeAccessory(AccessoryType accessoryType)
    {
        currentSkin.ChangeAccessory(accessoryType);
    }

    public void ChangeHat(HatType hatType)
    {
        currentSkin.ChangeHat(hatType);
    }

    public void ChangePant(PantType pantType)
    {
        currentSkin.ChangePant(pantType);
    }

}
