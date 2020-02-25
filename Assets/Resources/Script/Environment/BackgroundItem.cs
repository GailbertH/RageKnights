using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundItem : MonoBehaviour
{
    private SpriteRenderer itemSprite = null;
    private Transform itemTransform = null;
    [SerializeField] private Transform frontConnectingItem;
    [SerializeField] private Transform backConnectingItem;
    [SerializeField] private float itemWidth = 8; //Default For now

    public SpriteRenderer ItemSprite
    {
        get { return itemSprite; }
        set { itemSprite = value; }
    }

    public Transform ItemTransform
    {
        get { return itemTransform; }
        set { itemTransform = value; }
    }

    public float ItemWidth
    {
        get { return itemWidth; }
    }

    private void Awake()
    {
        ItemSprite = this.GetComponent<SpriteRenderer>();
        ItemTransform = this.GetComponent<Transform>();
    }

    public void AssignSprite(Sprite nextSprite)
    {
        if (ItemSprite != null)
        {
            ItemSprite.sprite = nextSprite;
        }
    }

    public void MovePosition(float speed)
    {
        if (ItemTransform != null)
        {
            Vector3 currentPosition = ItemTransform.position;
            ItemTransform.position = new Vector3((float)Math.Round(currentPosition.x + speed, 2), currentPosition.y, currentPosition.z);
        }
    }

    public void MoveForwardToConnectingItem()
    {
        if (frontConnectingItem != null)
        {
            ItemTransform.position = new Vector3((frontConnectingItem.position.x + ItemWidth),
                            ItemTransform.position.y,
                            ItemTransform.position.z);
        }
    }


    public void MoveBackToConnectingItem()
    {
        if (backConnectingItem != null)
        {
            ItemTransform.position = new Vector3((backConnectingItem.position.x - ItemWidth),
                            ItemTransform.position.y,
                            ItemTransform.position.z);
        }
    }
}
