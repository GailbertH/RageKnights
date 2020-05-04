using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePopup : PopupBase
{
    [SerializeField] GameObject panelClone;
    [SerializeField] Transform panelGroup;

    List<StoreItemData> storeItemData = new List<StoreItemData>();

    public override void Initialize(Action OnCloseAction)
    {
        onCloseEvent = OnCloseAction;
        storeItemData.Add(new StoreItemData { Name = "Potion", Price = 10f, InStock = true});
        storeItemData.Add(new StoreItemData { Name = "Sword", Price = 100f, InStock = false });
        storeItemData.Add(new StoreItemData { Name = "Armor", Price = 200f, InStock = false });
        storeItemData.Add(new StoreItemData { Name = "Artifact", Price = 100f, InStock = false });

        PopulateStoreData();
    }

    private void PopulateStoreData()
    {
        for (int i = 0; i < storeItemData.Count; i++)
        {
            GameObject newPanel = Instantiate<GameObject>(panelClone, panelGroup) as GameObject;
            StoreItems storeItem = newPanel.GetComponent<StoreItems>();
            storeItem.Init(storeItemData[i], OnPurchase);
            newPanel.SetActive(true);
        }
    }

    private void OnPurchase(StoreItemData purchaseItemData)
    {
        RageKnight.GameManager.Instance.AddPotion(1);
        Debug.Log("Buying item " + purchaseItemData.Name);
    }
}

public class StoreItemData
{
    public float Price;
    public string Name;
    public bool InStock;
}
