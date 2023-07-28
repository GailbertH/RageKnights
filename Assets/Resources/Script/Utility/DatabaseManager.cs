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
        public DatabaseContainerUnit dbPlayerCharacterUnit;
        public DatabaseContainerUnit dbEnemyCharacterUnit;
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

        public List<UnitDataModel> GetPlayerUnits(List<string> ids)
        {
            List<UnitDataModel> unitModels = new List<UnitDataModel>();
            foreach(string id in ids)
            {
                unitModels.Add(GetPlayerUnit(id));
            }
            return unitModels;
        }
        public UnitDataModel GetPlayerUnit(string id)
        {
            var data = dbPlayerCharacterUnit.Units.FirstOrDefault(x => x.id == id);
            UnitDataModel unitModel = new UnitDataModel(data);
            return unitModel;
        }

        public List<UnitDataModel> GetEnemyUnits(List<string> ids)
        {
            List<UnitDataModel> unitModels = new List<UnitDataModel>();
            foreach (string id in ids)
            {
                unitModels.Add(GetEnemyUnit(id));
            }
            return unitModels;
        }
        public UnitDataModel GetEnemyUnit(string id)
        {
            var data = dbEnemyCharacterUnit.Units.FirstOrDefault(x => x.id == id);
            UnitDataModel unitModel = new UnitDataModel(data);
            return unitModel;
        }
    }

    public class UnitDataModel
    {
        public string id;
        public string name;
        public int healthPoints;
        public int attackPower;
        public int defensePower;
        public string role; //  don't use this yet
        public string job; //   don't use this yet
        public Sprite icon;
        public Sprite splashArt;
        public GameObject unitPrefab;

        public UnitDataModel(DatabaseUnit dataModel)
        {
            id = dataModel.id;
            name = dataModel.unitName;
            healthPoints = dataModel.baseHealthPoints;
            attackPower = dataModel.baseAttackPower;
            defensePower = dataModel.baseDefensePower;
            icon = dataModel.icon;
            splashArt = dataModel.splashArt;
            unitPrefab = dataModel.unitPrefab;
        }
    }
}
