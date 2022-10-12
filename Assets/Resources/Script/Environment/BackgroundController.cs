using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private List<BackgroundItem> backgroundItems;

    public void MoveBackground(float speed)
    {
        if (backgroundItems != null)
        {
            //backgroundItems.ForEach(x => x.MovePosition(speed));
            for (int i = 0; i < backgroundItems.Count; i++)
            {
                BackgroundItem background = backgroundItems[i];
                background.MovePosition(speed);

                if (Math.Round((background.ItemTransform.position.x * -1), 2) >= (background.ItemWidth * 3f))
                {
                    background.MoveForwardToConnectingItem();
                }
                //else if (Math.Round((background.ItemTransform.position.x), 2) >= (background.ItemWidth * 3f))
                //{
                //    background.MoveBackToConnectingItem();
                //}
                //Debug.Log(background.ItemSprite.bounds.size.x);

            }
        }
    }

}
