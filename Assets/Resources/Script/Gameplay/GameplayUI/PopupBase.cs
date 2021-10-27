using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
    protected Action onCloseEvent = null;

    public virtual void Initialize(Action OnCloseAction, object data = null)
    {
        onCloseEvent = OnCloseAction;
    }

    public virtual void CloseButton()
    {
        if (onCloseEvent != null)
        {
            onCloseEvent.Invoke();
        }
        onCloseEvent = null;
        Destroy(this.gameObject);
    }
}
