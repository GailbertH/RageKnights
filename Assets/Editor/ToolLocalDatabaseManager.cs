using RageKnight.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
public enum DatabaseType
{
    Consumable = 0,
    Weapon = 1,
    Helmet = 2,
    Armor = 3,
}

namespace RageKnight.Tools
{
    public class ToolLocalDatabaseManager : EditorWindow
    {
        private const string _path = "Assets/Resources/";

        [MenuItem("RageKnight Tools/Data Manager")]
        public static void OpenDataManager()
        {
            ToolLocalDatabaseEditor viewer = ToolLocalDatabaseEditor.Instance;
            viewer.minSize = new Vector2(250f, 250f);
            viewer.maxSize = new Vector2(500f, 500f);
        }

        [MenuItem("RageKnight Tools/Item Database/Populate Consumable Database")]
        public static void PopulateConsumableDatabase()
        {
            string _conDBitemPath = "DatabaseData/Consumable/";
            string _conDBPath = "Database/Consumable-Database";
            if (EditorApplication.isPlaying)
            {
                return;
            }
            var databaseItem = Resources.LoadAll<Consumable>(_conDBitemPath).ToList();
            if (databaseItem == null || databaseItem.Count <= 0)
            {
                Debug.LogError("No consumable data items found");
            }

            var database = Resources.Load<DatabaseConsumable>(_conDBPath);
            if (database != null)
            {
                database.Consumables = new List<Consumable>(databaseItem);
                Debug.Log("<color=green>Populate Database of Consumables at " + _conDBPath + "</color>");
            }
            else
            {
                DatabaseConsumable asset = ScriptableObject.CreateInstance<DatabaseConsumable>();
                asset.Consumables = new List<Consumable>(databaseItem);
                AssetDatabase.CreateAsset(asset, _path + _conDBPath + ".asset");
                AssetDatabase.SaveAssets();
                Debug.Log("<color=green>Created and populated Database of Consumable " + _conDBPath + "</color>");
            }
        }

        [MenuItem("RageKnight Tools/Item Database/Apply Consumable Database")]
        public static void ApplyConsumableDatabase()
        {
            string _conDBPath = "Database/Consumable-Database";
            string _databaseManagerPath = "Prefab/Data/DatabaseManager";
            var _conDB = Resources.Load<DatabaseConsumable>(_conDBPath);
            if (_conDB == null)
            {
                Debug.LogError("No consumable database found");
            }
            else
            {
                var _dataManagerPrefab = Resources.Load<DatabaseManager>(_databaseManagerPath);
                _dataManagerPrefab.dbConsumable = _conDB;
                Debug.Log("<color=green>Database applied properly" + _conDBPath + "</color>");
            }
        }
    }

    public class ToolLocalDatabaseEditor : EditorWindow
    {
        private const string _path = "Assets/Resources/";
        private const string _conDBitemPath = "DatabaseData/Consumable/";
        private DatabaseType _currentViewType = DatabaseType.Consumable;
        List<Consumable> _consumableData = null;
        public static ToolLocalDatabaseEditor Instance
        {
            get { return GetWindow<ToolLocalDatabaseEditor>(); }
        }

        void Awake()
        {
            _consumableData = new List<Consumable>(Resources.LoadAll<Consumable>(_conDBitemPath).ToList());
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
            DrawButton(DatabaseType.Consumable);
            DrawButton(DatabaseType.Weapon);
            DrawButton(DatabaseType.Helmet);
            DrawButton(DatabaseType.Armor);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(20f);
            ViewChange();
        }

        private void DrawButton(DatabaseType viewType)
        {
            GUI.color = (viewType == _currentViewType) ? Color.yellow : new Color(0.75f, 0.75f, 0.75f);
            if (GUILayout.Button(viewType.ToString().ToUpper(), EditorStyles.toolbarButton))
            {
                _currentViewType = viewType;
            }

            GUI.color = Color.white;
        }

        private void ViewChange()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label(_currentViewType.ToString());
            GUILayout.FlexibleSpace();

            GUI.color = Color.cyan;
            LoadJsonButton();
            GUI.color = Color.yellow;
            SaveJsonButton();
            GUI.color = Color.green;
            AddButton();
            GUI.color = Color.magenta;
            FinalizeDataButton();
            GUI.color = Color.white;

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(3f);
            ShowConsumableField();
        }
        public Vector2 scroll = Vector2.zero;
        private void ShowConsumableField()
        {
            float labelWidth = 250f;
            this.scroll = GUILayout.BeginScrollView(this.scroll);
            List<string> idsToRemove = new List<string>();
            foreach (Consumable conDb in _consumableData)
            {
                GUILayout.BeginHorizontal();
                GUIStyle titleStyle = new GUIStyle();
                titleStyle.fontSize = 14;

                GUILayout.Label(" File Name:  " + (conDb.id + "-" + conDb.name).ToString(), titleStyle, GUILayout.Width(labelWidth));
                GUILayout.Label("ID:  " + conDb.id, titleStyle, GUILayout.Width(labelWidth - 100));
                GUI.color = Color.red;
                if (GUILayout.Button("REMOVE", GUILayout.Width(100)))
                {
                    idsToRemove.Add(conDb.id);
                }
                GUI.color = Color.white;
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Name", GUILayout.Width(labelWidth));
                conDb.name = EditorGUILayout.TextField(conDb.name, GUILayout.Width(labelWidth));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Description", GUILayout.Width(labelWidth));
                conDb.description = EditorGUILayout.TextArea(conDb.description, GUILayout.Width(labelWidth), GUILayout.Height(50));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Cost", GUILayout.Width(labelWidth));
                conDb.cost = EditorGUILayout.LongField(conDb.cost, GUILayout.Width(labelWidth));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Potency", GUILayout.Width(labelWidth));
                conDb.potency = EditorGUILayout.IntField(conDb.potency, GUILayout.Width(labelWidth));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Base Stock", GUILayout.Width(labelWidth));
                conDb.baseStockCount = EditorGUILayout.IntField(conDb.baseStockCount, GUILayout.Width(labelWidth));
                GUILayout.EndHorizontal();
            }

            foreach (string badIds in idsToRemove)
            {
                _consumableData.Remove(_consumableData.Find(x => x.id == badIds));
            }
            GUILayout.EndScrollView();
        }

        private void LoadJsonButton()
        {
            if (GUILayout.Button("LOAD JSON", EditorStyles.toolbarButton))
            {
                Debug.Log("Something");
            }
        }

        private void SaveJsonButton()
        {
            if (GUILayout.Button("SAVE JSON", EditorStyles.toolbarButton))
            {
                Debug.Log("Something");
            }
        }

        private void AddButton()
        {
            if (GUILayout.Button("ADD", EditorStyles.toolbarButton))
            {
                if (_consumableData == null)
                {
                    _consumableData = new List<Consumable>();
                }

                int newId = 0;
                if (_consumableData.Count > 0)
                {
                    int.TryParse(_consumableData[_consumableData.Count() - 1].id, out newId);
                    newId += 1;
                }
                Consumable newAdd = ScriptableObject.CreateInstance<Consumable>();
                newAdd.id = String.Format("{0:D4}", newId);
                _consumableData.Add(newAdd);
            }
        }

        private void FinalizeDataButton()
        {
            if (GUILayout.Button("Finalize Data", EditorStyles.toolbarButton))
            {
                var files = Directory.GetFiles(_path + _conDBitemPath);
                var newContainer = _consumableData.Select(x => new Consumable{
                    id = x.id,
                    name = x.name,
                    description = x.description,
                    cost = x.cost,
                    potency = x.potency,
                    icon = x.icon,
                    baseStockCount = x.baseStockCount,
                    ItemEffectType = x.ItemEffectType
                }).ToList();


                for (int i = 0; i < files.Length; i++)
                {
                    AssetDatabase.DeleteAsset(files[i]);
                }
                AssetDatabase.SaveAssets();

                foreach (Consumable conDb in newContainer)
                {
                    Consumable asset = ScriptableObject.CreateInstance<Consumable>();
                    asset.PopulateData(conDb);
                    string fileName = asset.id + "-" + asset.name;
                    if (asset != null)
                    {
                        AssetDatabase.RemoveObjectFromAsset(asset);
                    }
                    AssetDatabase.CreateAsset(asset, "Assets/Resources/DatabaseData/Consumable/" + fileName + ".asset");
                    //AssetDatabase.AddObjectToAsset(asset, "Assets/Resources/DatabaseData/Consumable/" + fileName + ".asset");
                }
                AssetDatabase.SaveAssets();
            }
        }
    }
}
