using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBar : MonoBehaviour
{
    [SerializeField] GameObject bg;
    [SerializeField] UIShop.ShopType type;
    public UIShop.ShopType Type => type;

    UIShop shop;

    public void SetShop(UIShop shop)
    {
        this.shop = shop;
    }

    public void Select()
    {
        shop.SelectBar(this);
    }

    public void Active(bool active)
    {
        bg.SetActive(!active);
    }
}
