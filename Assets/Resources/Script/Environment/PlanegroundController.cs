using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanegroundController : MonoBehaviour
{
    [SerializeField] private List<PlanegroundItem> planegroundItem;

    public void MovePlaneground(float speed)
    {
        if (planegroundItem != null)
        {
            //backgroundItems.ForEach(x => x.MovePosition(speed));
            for (int i = 0; i < planegroundItem.Count; i++)
            {
                PlanegroundItem planeground = planegroundItem[i];
                planeground.MovePosition(speed);

                if (Math.Round((planeground.ItemTransform.position.x * -1), 2) >= (planeground.ItemWidth * 1.5f))
                {
                    planeground.MoveBackToConnectingItem();
                }
                //Debug.Log(background.ItemSprite.bounds.size.x);

            }
        }
    }
}
