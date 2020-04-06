using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItems : MonoBehaviour
{
    [SerializeField] Text itemDisplayName;
    [SerializeField] Button purchaseButton;
    [SerializeField] GameObject availability;

    private StoreItemData storeItemData;
    private Action<StoreItemData> onPurchaseAction = null;

    public void Init(StoreItemData itemData, Action<StoreItemData> onPurchase)
    {
        storeItemData = itemData;
        if (storeItemData != null)
        {
            itemDisplayName.text = storeItemData.Name;
            purchaseButton.enabled = storeItemData.InStock;
            availability.SetActive(!storeItemData.InStock);
            onPurchaseAction = onPurchase;
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
