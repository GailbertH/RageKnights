using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItems : MonoBehaviour
{
    [SerializeField] Text itemDisplayName;
    [SerializeField] Text stockCount;
    [SerializeField] Button purchaseButton;
    [SerializeField] GameObject availability;
    [SerializeField] Image itemIcon;

    private StoreItemData storeItemData;
    private Action<StoreItemData> onPurchaseAction = null;

    public void Init(StoreItemData itemData, Action<StoreItemData> onPurchase)
    {
        storeItemData = itemData;
        if (storeItemData != null)
        {
            itemDisplayName.text = storeItemData.name;
            purchaseButton.enabled = storeItemData.inStock;
            availability.SetActive(!storeItemData.inStock);
            onPurchaseAction = onPurchase;
            stockCount.text = itemData.stockCount > 0 ? "Stock:" + itemData.stockCount.ToString() : "Out of stock";
            itemIcon.sprite = itemData.icon;
        }
        else
            this.gameObject.SetActive(false);
    }

    public void OnPurchase()
    {
        //popup show to determine quanity ? 
        onPurchaseAction.Invoke(storeItemData);
    }
}
