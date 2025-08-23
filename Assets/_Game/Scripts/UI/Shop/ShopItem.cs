using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UIShop;


public class ShopItem : MonoBehaviour
{
    public enum State { Buy = 0, Bought = 1, Equipped = 2, Selecting = 3 }

    [SerializeField] GameObject[] stateObjects;

    private UIShop shop;

    [SerializeField] Color[] colorBg;
    [SerializeField] Image icon;
    [SerializeField] Image bgIcon;

    public int id;
    public State state;

    public Enum Type;
    internal ShopItemData data;

    public void SetShop(UIShop shop)
    {
        this.shop = shop;
    }

    public void SetData<T>(int id, ShopItemData<T> itemData, UIShop shop) where T : Enum
    {
        this.id = id;
        Type = itemData.type;
        this.data = itemData;
        this.shop = shop;
        icon.sprite = itemData.icon;
        bgIcon.color = colorBg[(int)shop.shopType];
    }

    public void SelectButton()
    {
        shop.SelectItem(this);
    }

    public void SetState(State state)
    {
        for (int i = 0; i < stateObjects.Length; i++)
        {
            stateObjects[i].SetActive(false);
        }

        stateObjects[(int)state].SetActive(true);

        this.state = state;
    }

    public Type GetEnumType()
    {
        return Type.GetType();
    }

}
