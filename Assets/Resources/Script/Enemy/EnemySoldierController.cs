using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldierController : MonoBehaviour
{
    [SerializeField] private EnemyHandler enemyHandler;
    private const int MAX_LAYERS = 3;
    private const float MIN_X_POS = 1f;
    private const float MAX_X_POS = 3.5f;

    private Dictionary<CombatPlacement, Tuple<float, float>> placement = new Dictionary<CombatPlacement, Tuple<float, float>>();
    private List<EnemyController> enemyArmy;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = this.transform.position;

        placement.Add(CombatPlacement.TOP, new Tuple<float, float>(1.5f, 2.1f)); //6
        placement.Add(CombatPlacement.MID, new Tuple<float, float>(0.7f, 1.4f)); //7
        placement.Add(CombatPlacement.BOT, new Tuple<float, float>(0.1f, 0.7f)); //6
    }

    public void Init(int armySize, List<GameObject> enemyList)
    {
        enemyArmy = new List<EnemyController>();
        int playerLimit = 25;
        armySize = armySize > playerLimit ? playerLimit : armySize;
        SetupUnits(armySize, enemyList);
    }

    public void ShowUnits(bool isShow = true)
    {
        this.gameObject.SetActive(isShow);
        for (int i = 0; i < enemyArmy.Count; i++)
        {
            if (enemyArmy[i] != null)
            {
                enemyArmy[i].animationPlayer.Idle();
            }
        }
    }

    public void MoveUnits(float speed)
    {
        if (this.transform != null)
        {
            Vector3 currentPosition = this.transform.position;
            this.transform.position = new Vector3((float)Math.Round(currentPosition.x + speed, 2), currentPosition.y, currentPosition.z);
        }
    }

    public void DamageArmy(float damage)
    {
        for (int i = 0; i < enemyArmy.Count; i++)
        {
            if (enemyArmy[i] != null)
            {
                if(enemyArmy[i].transform.position.x <= 1)
                    enemyArmy[i].Damaged(damage);
            }
        }
    }

    public void End()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        this.transform.position = initialPosition;
    }

    //Will contain army data someday
    private void SetupUnits(int armyCount, List<GameObject> enemyList)
    {
        //Max Height 2.1 Min Heigh is 0.1
        //EachUnit has a space of 1 - 3.5
        GameObject newGameObject = new GameObject();
        CombatPlacement currentPlacement = CombatPlacement.MID;
        newGameObject.name = "EnemyContainer";
        for (int i = 0; i < armyCount; i++)
        {
            Tuple<float, float> yRange = placement[currentPlacement];
            int layer = i / 3;
            float incrementLayer = (float)layer * 3.5f;
            GameObject enemyHolder = Instantiate<GameObject>(newGameObject, this.transform);
            float ypos = UnityEngine.Random.Range(yRange.Item1, yRange.Item2);
            float xpos = UnityEngine.Random.Range(1f + incrementLayer, 3.5f + incrementLayer);
            enemyHolder.transform.localPosition = new Vector3(xpos, ypos, 1);

            //need to reset everything for safety cause of animation
            GameObject enemyObject = Instantiate<GameObject>(enemyList[0],
                enemyHolder.transform
                ) as GameObject;

            enemyObject.transform.localPosition = enemyList[0].transform.position;
            enemyObject.transform.localRotation = enemyList[0].transform.rotation;
            enemyObject.SetActive(true);
            var enemyController = enemyObject.GetComponent<EnemyController>();
            enemyController.Initialize(currentPlacement, enemyHandler);
            enemyArmy.Add(enemyController);
            currentPlacement = IncrementPlacement(currentPlacement);
        }
    }

    private CombatPlacement IncrementPlacement(CombatPlacement currentPlacement)
    {
        if (currentPlacement == CombatPlacement.MID)
        {
            return CombatPlacement.BOT;
        }
        else if (currentPlacement == CombatPlacement.BOT)
        {
            return CombatPlacement.TOP;
        }
        return CombatPlacement.MID;
    }
}
