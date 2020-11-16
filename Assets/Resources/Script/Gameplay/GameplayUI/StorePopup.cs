using RageKnight;
using RageKnight.Database;
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
        storeItemData = ConsumablesMapperToStoreItemDatas(DatabaseManager.Instance.GetAllConsumables());
        PopulateStoreData();
    }

    private void PopulateStoreData()
    {
        if (storeItemData == null)
            return;

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
        GameManager.Instance.AddGold(purchaseItemData.price * -1);
        Debug.Log("Buying item " + purchaseItemData.name);
    }

    private List<StoreItemData> ConsumablesMapperToStoreItemDatas(List<Consumable> consumables)
    {
        List<StoreItemData> mappedData = new List<StoreItemData>();
        if (consumables != null)
        {
            Debug.Log(consumables.Count);
            for (int i = 0; i < consumables.Count; i++)
            {
                Consumable con = consumables[i];
                mappedData.Add(new StoreItemData {
                    id = con.id,
                    price = con.cost,
                    name = con.name,
                    potency = con.potency,
                    itemEffectType = con.ItemEffectType,
                    icon = con.icon,
                    stockCount = con.baseStockCount,
                    inStock = con.baseStockCount > 0
                });
            }
        }
        return mappedData;
    }
}

public class StoreItemData
{
    public string id;
    public long price;
    public string name;
    public int stockCount;
    public bool inStock;
    public int potency;
    public ItemEffectType itemEffectType;
    public Sprite icon;
}
