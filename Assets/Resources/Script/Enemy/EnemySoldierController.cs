using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldierController : MonoBehaviour
{
    void Start()
    {
        
    }

    public void Init()
    {

    }

    public void ShowUnits(bool isShow = true)
    {
        this.gameObject.SetActive(isShow);
    }

    public void MoveUnits(float speed)
    {
        if (this.transform != null)
        {
            Vector3 currentPosition = this.transform.position;
            this.transform.position = new Vector3((float)Math.Round(currentPosition.x + speed, 2), currentPosition.y, currentPosition.z);
        }
    }
}
