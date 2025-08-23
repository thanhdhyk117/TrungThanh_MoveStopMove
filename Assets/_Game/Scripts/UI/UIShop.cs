using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UICanvas
{
    public enum ShopType { Hat, Pant, Accessory, Skin, Weapon }

    [SerializeField] ShopData data;
    [SerializeField] ShopItem prefab;
    [SerializeField] Transform content;
    [SerializeField] ShopBar[] shopBars;

    [SerializeField] TextMeshProUGUI playerCoinTxt;

    [SerializeField] ButtonState buttonState;
    [SerializeField] Text coinTxt;
    [SerializeField] Text adsTxt;

    MiniPool<ShopItem> miniPool = new MiniPool<ShopItem>();

    private ShopItem currentItem;
    private ShopBar currentBar;

    private ShopItem itemEquiped;

    public ShopType shopType => currentBar.Type;

    private void Awake()
    {
        miniPool.OnInit(prefab, 10, content);

        for (int i = 0; i < shopBars.Length; i++)
        {
            shopBars[i].SetShop(this);
        }
    }

    public override void Open()
    {
        base.Open();
        SelectBar(shopBars[0]);
        // CameraFollower.Ins.ChangeState(CameraFollower.State.Shop);

        playerCoinTxt.SetText(UserDataManager.Ins.userData.coin.ToString());
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        UIManager.Ins.OpenUI<UIMainMenu>();

        LevelManager.Ins.player.TakeOffClothes();
        LevelManager.Ins.player.OnSetFromData();
        LevelManager.Ins.player.WearClothes();
    }

    internal void SelectBar(ShopBar shopBar)
    {
        if (currentBar != null)
        {
            currentBar.Active(false);
        }

        currentBar = shopBar;
        currentBar.Active(true);

        miniPool.Collect();
        itemEquiped = null;

        switch (currentBar.Type)
        {
            case ShopType.Hat:
                InitShipItems(data.hats.Ts, ref itemEquiped);
                break;
            case ShopType.Pant:
                InitShipItems(data.pants.Ts, ref itemEquiped);
                break;
            case ShopType.Accessory:
                InitShipItems(data.accessories.Ts, ref itemEquiped);
                break;
            case ShopType.Skin:
                InitShipItems(data.skins.Ts, ref itemEquiped);
                break;
            default:
                break;
        }

    }

    private void InitShipItems<T>(List<ShopItemData<T>> items, ref ShopItem itemEquiped) where T : Enum
    {

        string keyNameList = GetKeyEnumType(items[0].type);
        // Debug.Log(keyNameList);

        for (int i = 0; i < items.Count; i++)
        {
            ShopItem.State state = (ShopItem.State)UserDataManager.Ins.GetDataState(keyNameList, i);


            ShopItem item = miniPool.Spawn();
            item.SetData(i, items[i], this);
            item.SetState(state);

            if (state == ShopItem.State.Equipped)
            {
                SelectItem(item);
                itemEquiped = item;
            }

        }
    }

    public ShopItem.State GetState(ShopItem shopItem)
    {
        Debug.Log("=================");
        Debug.Log(GetKeyEnumType(shopItem));
        Debug.Log(shopItem.id);
        return (ShopItem.State)UserDataManager.Ins.GetDataState(GetKeyEnumType(shopItem), shopItem.id);
    }

    public string GetKeyEnumType(ShopItem shopItem)
    {
        switch (shopItem.GetEnumType())
        {
            case Type t when t == typeof(HatType):
                return "hatStateList";
            case Type t when t == typeof(PantType):
                return "pantStateList";
            case Type t when t == typeof(SkinType):
                return "skinStateList";
            case Type t when t == typeof(AccessoryType):
                return "accessoryStateList";
        }
        return "";
    }


    internal void SelectItem(ShopItem item)
    {
        if (currentItem != null)
        {
            currentItem.SetState(GetState(currentItem));
        }

        currentItem = item;

        switch (currentItem.state)
        {
            case ShopItem.State.Buy:
                buttonState.SetState(ButtonState.State.Buy);
                break;
            case ShopItem.State.Bought:
                buttonState.SetState(ButtonState.State.Equip);
                break;
            case ShopItem.State.Equipped:
                buttonState.SetState(ButtonState.State.Equiped);
                break;
            case ShopItem.State.Selecting:
                break;
            default:
                break;
        }

        LevelManager.Ins.player.TryCloth(shopType, currentItem.Type);
        currentItem.SetState(ShopItem.State.Selecting);

        //check data
        coinTxt.text = item.data.cost.ToString();
        adsTxt.text = item.data.ads.ToString();
    }

    public void BuyButton()
    {
        //TODO: check so tien
        if (true)
        {
            UserDataManager.Ins.SetDataState(GetKeyEnumType(currentItem), currentItem.id, (int)ShopItem.State.Bought);
            SelectItem(currentItem);
        }

    }

    public void AdsButton()
    {
        //TODO: check quang cao
        if (true)
        {
            UserDataManager.Ins.SetDataState(GetKeyEnumType(currentItem), currentItem.id, (int)ShopItem.State.Bought);
            SelectItem(currentItem);
        }
    }

    public void EquipButton()
    {
        if (currentItem != null)
        {
            UserDataManager.Ins.SetDataState(GetKeyEnumType(currentItem), currentItem.id, (int)ShopItem.State.Equipped);
            switch (shopType)
            {

                case ShopType.Pant:
                    UserDataManager.Ins.SetDataState("pantStateList", GetTypeIndex(UserDataManager.Ins.userData.playerPant), (int)ShopItem.State.Bought);
                    UserDataManager.Ins.SetEnumData("playerPant", (PantType)currentItem.Type);
                    break;

                case ShopType.Hat:
                    UserDataManager.Ins.SetDataState("hatStateList", GetTypeIndex(UserDataManager.Ins.userData.playerHat), (int)ShopItem.State.Bought);
                    UserDataManager.Ins.SetEnumData("playerHat", (HatType)currentItem.Type);
                    break;

                case ShopType.Accessory:
                    UserDataManager.Ins.SetDataState("accessoryStateList", GetTypeIndex(UserDataManager.Ins.userData.playerAccessory), (int)ShopItem.State.Bought);
                    UserDataManager.Ins.SetEnumData("playerAccessory", (AccessoryType)currentItem.Type);
                    break;

                case ShopType.Skin:
                    UserDataManager.Ins.SetDataState("skinStateList", GetTypeIndex(UserDataManager.Ins.userData.playerSkin), (int)ShopItem.State.Bought);
                    UserDataManager.Ins.SetEnumData("playerSkin", (SkinType)currentItem.Type);
                    break;

                case ShopType.Weapon:
                    break;
                default:
                    break;
            }

        }

        if (itemEquiped != null)
        {
            itemEquiped.SetState(ShopItem.State.Bought);
        }

        currentItem.SetState(ShopItem.State.Equipped);
        SelectItem(currentItem);
    }

    public string GetKeyEnumType<T>(T enumValue) where T : Enum
    {
        if (typeof(T) == typeof(PantType))
        {
            return "pantStateList";
        }

        if (typeof(T) == typeof(HatType))
        {
            return "hatStateList";
        }

        if (typeof(T) == typeof(SkinType))
        {
            return "skinStateList";
        }

        if (typeof(T) == typeof(AccessoryType))
        {
            return "accessoryStateList";
        }

        return null;
    }



    public int GetSkinTypeIndex(SkinType skin)
    {
        int poolTypeIndex = (int)skin;
        int offset = (int)PoolType.SKIN_Normal;
        return poolTypeIndex - offset;
    }

    public int GetHatTypeIndex(HatType hat)
    {
        int poolTypeIndex = (int)hat;
        int offset = (int)PoolType.HAT_Arrow;
        return poolTypeIndex - offset;
    }

    public int GetAccessoryTypeIndex(AccessoryType accessory)
    {
        int poolTypeIndex = (int)accessory;
        int offset = (int)PoolType.ACC_Captain;
        return poolTypeIndex - offset;
    }

    public int GetPantTypeIndex(PantType pant)
    {
        int poolTypeIndex = (int)pant;

        return poolTypeIndex;
    }

    public int GetTypeIndex(Enum t)
    {
        switch (GetKeyEnumType(currentItem))
        {
            case "pantStateList":
                return GetPantTypeIndex((PantType)t);

            case "accessoryStateList":
                return GetAccessoryTypeIndex((AccessoryType)t);

            case "hatStateList":
                return GetHatTypeIndex((HatType)t);

            case "skinStateList":
                return GetSkinTypeIndex((SkinType)t);

        }
        return 0;
    }
}
