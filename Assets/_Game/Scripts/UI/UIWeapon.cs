using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWeapon : UICanvas
{
    [SerializeField] WeaponData weaponData;
    [SerializeField] ButtonState buttonState;
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TextMeshProUGUI playerCoinTxt;
    [SerializeField] Text coinTxt;
    [SerializeField] Text adsTxt;


    private WeaponType weaponType;

    public override void Setup()
    {
        base.Setup();
        ChangeWeapon(UserDataManager.Ins.userData.playerWeapon);
        playerCoinTxt.SetText(UserDataManager.Ins.userData.coin.ToString());
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();

        LevelManager.Ins.player.TakeOffClothes();
        LevelManager.Ins.player.OnSetFromData();
        LevelManager.Ins.player.WearClothes();

        UIManager.Ins.OpenUI<UIMainMenu>();
    }

    public void NextButton()
    {
        ChangeWeapon(weaponData.NextType(weaponType));
    }

    public void PrevButton()
    {
        ChangeWeapon(weaponData.PrevType(weaponType));
    }

    public void BuyButton()
    {
        //TODO: check tien
        if (true)
        {
            UserDataManager.Ins.SetDataState("weaponStateList", GetWeaponTypeIndex(weaponType), (int)ShopItem.State.Bought);
            ChangeWeapon(weaponType);
        }
    }

    public void AdsButton()
    {
        //TODO: check xem quang cao
        if (true)
        {
            UserDataManager.Ins.SetDataState("weaponStateList", GetWeaponTypeIndex(weaponType), (int)ShopItem.State.Bought);
            ChangeWeapon(weaponType);
        }
    }

    public void EquipButton()
    {
        UserDataManager.Ins.SetDataState("weaponStateList", GetWeaponTypeIndex(UserDataManager.Ins.userData.playerWeapon), (int)ShopItem.State.Bought);
        UserDataManager.Ins.SetDataState("weaponStateList", GetWeaponTypeIndex(weaponType), (int)ShopItem.State.Equipped);
        UserDataManager.Ins.SetEnumData("playerWeapon", weaponType);

        ChangeWeapon(weaponType);
        LevelManager.Ins.player.TryCloth(UIShop.ShopType.Weapon, weaponType);
    }

    public void ChangeWeapon(WeaponType weaponType)
    {
        this.weaponType = weaponType;

        LevelManager.Ins.player.TryCloth(UIShop.ShopType.Weapon, weaponType);

        ButtonState.State state = (ButtonState.State)UserDataManager.Ins.GetDataState("weaponStateList", GetWeaponTypeIndex(weaponType));
        Debug.Log(GetWeaponTypeIndex(weaponType));
        buttonState.SetState(state);

        WeaponItem item = weaponData.GetWeaponItem(weaponType);
        nameTxt.SetText(item.name);
        coinTxt.text = item.cost.ToString();
        adsTxt.text = item.ads.ToString();
    }

    public int GetWeaponTypeIndex(WeaponType weapon)
    {
        int poolTypeIndex = (int)weapon;
        int offset = (int)PoolType.Wp_Arrow;
        return poolTypeIndex - offset;
    }
}
