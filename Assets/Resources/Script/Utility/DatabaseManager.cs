using System.Collections;
using RageKnight.Database;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RageKnight.Database
{
    public class DatabaseManager : MonoBehaviour
    {
        public DatabaseConsumable dbConsumable;
        public DatabasePlayerUnit dbCharacterUnit;
        private static DatabaseManager instance;
        public static DatabaseManager Instance { get { return instance; } }

        void Awake()
        {
            instance = this;
        }

        public List<DB_Consumable> GetAllConsumables()
        {
            return dbConsumable.Consumables;
        }

        public PlayerUnitModel GetPlayerUnit(string id)
        {
            var data = dbCharacterUnit.Units.FirstOrDefault(x => x.id == id);
            PlayerUnitModel unitModel = new PlayerUnitModel();
            unitModel.Convert(data);
            return unitModel;
        }

    }

    public class PlayerUnitModel
    {
        public string id;
        public string name;
        public float healthPoints;
        public int attackPower;
        public int defensePower;
        public int skillId1;
        public int skillId2;
        public string role; //don't use this yet
        public string job; // dont use this yet
        public Sprite icon;
        public Sprite splashArt;

        public void Convert(DB_PlayerUnit dataModel)
        {
            id = dataModel.id;
            name = dataModel.unitName;
            healthPoints = dataModel.healthPoints;
            attackPower = dataModel.attackPower;
            defensePower = dataModel.defensePower;
            skillId1 = dataModel.skillId1;
            skillId2 = dataModel.skillId2;
            icon = dataModel.icon;
            splashArt = dataModel.splashArt;
        }
    }
}
