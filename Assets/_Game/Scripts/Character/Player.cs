using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public const float TIME_TO_RELOAD = 5f;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Rigidbody rb;
    [SerializeField] SkinType skinType = SkinType.SKIN_Normal;
    [SerializeField] PantType pantType;
    [SerializeField] HatType hatType = HatType.HAT_Cap;
    [SerializeField] AccessoryType accessoryType = AccessoryType.ACC_Shield;
    [SerializeField] WeaponType weaponType;


    private bool isMoving = false;
    Character lastTarget;

    public Character Target => target;
    private bool IsCanUpdate => GameManager.Ins.IsState(GameState.GamePlay) || GameManager.Ins.IsState(GameState.Setting);
    public int Coin => Score * 10;


    public override void OnInit()
    {
        OnSetFromData();
        base.OnInit();

        TF.rotation = Quaternion.Euler(Vector3.up * 180);
        SetSize(MIN_SIZE);
        ResetAnim();
        lastTarget = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCanUpdate && !IsDead)
        {
            CheckTarget();

            if (Input.GetMouseButton(0) && JoyStick.direction != Vector3.zero)
            {
                rb.MovePosition(rb.position + JoyStick.direction * moveSpeed * Time.deltaTime);
                TF.position = rb.position;

                TF.forward = JoyStick.direction;

                ChangeAnim(Constant.ANIM_RUN);
                isMoving = true;
            }


            if (Input.GetMouseButtonUp(0))
            {
                isMoving = false;
                OnMoveStop();
                OnAttack();
            }
        }

    }

    private void CheckTarget()
    {
        target = GetTargetInRange();

        if (target != null)
        {
            if (target != lastTarget)
            {
                target.ActiveMaker();
                if (lastTarget != null)
                {
                    lastTarget.DeactiveMaker();
                }
                lastTarget = target;
            }
        }
        else
        {
            if (lastTarget != null)
            {
                lastTarget.DeactiveMaker();
                lastTarget = target;
            }
        }

    }
    public override void OnAttack()
    {
        base.OnAttack();
        if (target != null && isCanAttack)
        {
            Throw();
            ResetAnim();
            isCanAttack = false;
            StartCoroutine(CoolDownAttack(TIME_ON_COOLDOWN));
        }
    }
    public IEnumerator CoolDownAttack(float time)
    {
        yield return new WaitForSeconds(time);

        isCanAttack = true;
    }
    public override void OnMoveStop()
    {
        base.OnMoveStop();
        rb.velocity = Vector3.zero;
        ChangeAnim(Constant.ANIM_IDLE);
    }

    internal void OnRevive()
    {
        ChangeAnim(Constant.ANIM_IDLE);
        IsDead = false;
    }

    internal void OnSetFromData()
    {

        skinType = UserDataManager.Ins.userData.playerSkin;
        weaponType = UserDataManager.Ins.userData.playerWeapon;
        pantType = UserDataManager.Ins.userData.playerPant;
        accessoryType = UserDataManager.Ins.userData.playerAccessory;
        hatType = UserDataManager.Ins.userData.playerHat;
    }

    public override void WearClothes()
    {
        base.WearClothes();

        ChangeSkin(skinType);
        ChangeWeapon(weaponType);
        ChangePant(pantType);
        ChangeAccessory(accessoryType);
        ChangeHat(hatType);

    }

    public void TryCloth(UIShop.ShopType shopType, Enum type)
    {
        switch (shopType)
        {
            case UIShop.ShopType.Hat:
                currentSkin.DespawnHat();
                ChangeHat((HatType)type);
                break;

            case UIShop.ShopType.Accessory:
                currentSkin.DespawnAccessory();
                ChangeAccessory((AccessoryType)type);
                break;

            case UIShop.ShopType.Skin:
                TakeOffClothes();
                skinType = (SkinType)type;
                WearClothes();
                break;

            case UIShop.ShopType.Pant:
                ChangePant((PantType)type);
                break;

            case UIShop.ShopType.Weapon:
                currentSkin.DespawnWeapon();
                ChangeWeapon((WeaponType)type);
                break;
            default:
                break;
        }

    }
}
