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

    private int stockCounter;
    private StoreItemData storeItemData;
    private Action<StoreItemData> onPurchaseAction = null;

    public void Init(StoreItemData itemData, Action<StoreItemData> onPurchase)
    {
        storeItemData = itemData;
        if (storeItemData != null)
        {
            stockCounter = itemData.stockCount;
            stockCount.text = stockCounter > 0 ? "Stock:" + stockCounter.ToString() : "Out of stock";

            itemDisplayName.text = storeItemData.name;
            purchaseButton.enabled = stockCounter > 0;
            availability.SetActive(!storeItemData.inStock);
            onPurchaseAction = onPurchase;
            itemIcon.sprite = itemData.icon;
        }
        else
            this.gameObject.SetActive(false);
    }

    public void UpdateInfo()
    {
        bool isAvailable = stockCounter > 0;
        stockCount.text = isAvailable ? "Stock:" + stockCounter.ToString() : "Out of stock";
        purchaseButton.enabled = isAvailable;
        availability.SetActive(!isAvailable);
    }

    public void OnPurchase()
    {
        //popup show to determine quanity ? 
        stockCounter--;
        UpdateInfo();
        onPurchaseAction.Invoke(storeItemData);
    }
}
