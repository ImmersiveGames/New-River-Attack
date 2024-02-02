using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Serialization;
using Utils;

namespace RiverAttack
{
    public enum GameStatistics {
        Score, Time, MaxPathDistance, ShootSpent, BombSpent, FuelSpent, LifeSpent, DeathByWall, DeathByBullets, DieByFuel,
        CompletedLevels, EnemiesDestroyed, CollectableItems
    };
    public class StatisticDataManager : MonoBehaviour
    {
        [SerializeField] Transform containerTransform;
        [SerializeField] GameObject statisticItemCard;
        [SerializeField] List<EnemiesScriptable> enemiesList;
        [SerializeField] List<CollectibleScriptable> collectiblesList;
        [SerializeField] List<StatisticItemData> statisticsListData;
        public static string getScore
        {
            get { return GamePlayingLog.instance.totalScore.ToString(); }
        }
        public static string getSpendTime
        {
            get { return Tools.TimeFormat(GamePlayingLog.instance.timeSpent); }
        }
        public static string getMaxPathDistance
        {
            get { return $"{GamePlayingLog.instance.maxPathDistance.ToString(CultureInfo.CurrentCulture)} KM"; }
        }
        public string getShootSpent
        {
            get { return GamePlayingLog.instance.shootSpent.ToString(CultureInfo.CurrentCulture); }
        }
        public string getBombSpent
        {
            get { return $"{GamePlayingLog.instance.bombSpent} KM"; }
        }
        public string getFuelSpent
        {
            get { return $"{GamePlayingLog.instance.fuelSpent} L"; }
        }
        public string getLifeSpent
        {
            get { return GamePlayingLog.instance.livesSpent.ToString(); }
        }
        public string getDeathByWall
        {
            get { return GamePlayingLog.instance.playerDieWall.ToString(); }
        }
        public string getDeathByBullets
        {
            get { return GamePlayingLog.instance.playerDieBullet.ToString(); }
        }
        public string getDeathByFuel
        {
            get { return GamePlayingLog.instance.playerDieFuelEmpty.ToString(); }
        }
        public string getCompletedLevels
        {
            get { return GamePlayingLog.instance.finishLevels.Count.ToString(); }
        }
        public List<LogResults> getEnemiesDestroyed
        {
            get { return GamePlayingLog.instance.hitEnemiesResultsList.Where(item=>enemiesList.Contains(item.enemy)).ToList(); }
        }
        public List<LogResults> getCollectableItems
        {
            get { return GamePlayingLog.instance.hitEnemiesResultsList.Where(item=>collectiblesList.Contains(item.enemy)).ToList(); }
        }
        void OnEnable()
        {
            ClearContainer();
            CreateFullStatisticList();
        }
        
        void ClearContainer()
        {
            foreach (Transform child in containerTransform)
            {
                Destroy(child.gameObject);
            }
        }

        void CreateFullStatisticList()
        {
            statisticsListData = FilterByItemReference(statisticsListData);
            foreach (var itemData in statisticsListData)
            {
                itemData.itemString = itemData.itemLocalizedString.GetLocalizedString();
                itemData.itemStatisticsList = null;
                itemData.itemValue = null;
                switch (itemData.itemReference)
                {
                    case GameStatistics.Score:
                        itemData.itemValue = getScore;
                        break;
                    case GameStatistics.Time:
                        itemData.itemValue = getSpendTime;
                        break;
                    case GameStatistics.MaxPathDistance:
                        itemData.itemValue = getMaxPathDistance;
                        break;
                    case GameStatistics.ShootSpent:
                        itemData.itemValue = getShootSpent;
                        break;
                    case GameStatistics.BombSpent:
                        itemData.itemValue = getBombSpent;
                        break;
                    case GameStatistics.FuelSpent:
                        itemData.itemValue = getFuelSpent;
                        break;
                    case GameStatistics.LifeSpent:
                        itemData.itemValue = getLifeSpent; ;
                        break;
                    case GameStatistics.DeathByWall:
                        itemData.itemValue = getDeathByWall;
                        break;
                    case GameStatistics.DeathByBullets:
                        itemData.itemValue = getDeathByBullets;
                        break;
                    case GameStatistics.DieByFuel:
                        itemData.itemValue = getDeathByFuel;
                        break;
                    case GameStatistics.CompletedLevels:
                        itemData.itemValue = getCompletedLevels;
                        break;
                    case GameStatistics.EnemiesDestroyed:
                        itemData.itemStatisticsList = SumStatistic(getEnemiesDestroyed);
                        break;
                    case GameStatistics.CollectableItems:
                        itemData.itemStatisticsList = SumStatistic(getCollectableItems);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                CreateItem(itemData);
            }
        }

        void CreateItem(StatisticItemData itemData)
        {
            if (itemData.itemStatisticsList != null)
            {
                CreateSubItem(new StatisticItemData()); // Cria uma linha em Branco
            }
            CreateSubItem(itemData); // Cria o item ou Cria o titulo
            if (itemData.itemStatisticsList == null)
                return;
            foreach (var newItemData in itemData.itemStatisticsList)
            {
                CreateSubItem(newItemData); // Cria uma linha em Branco
            }
        }
        void CreateSubItem(StatisticItemData itemData)
        {
            var itemCard = Instantiate(statisticItemCard, containerTransform);
            itemCard.GetComponent<ItemCardDisplayHolder>().itemNameText.text = itemData.itemString;
            itemCard.GetComponent<ItemCardDisplayHolder>().itemValueText.text = itemData.itemValue;
        }
        static List<StatisticItemData> FilterByItemReference(IEnumerable<StatisticItemData> originalList)
        {
            // Usa Distinct com um comparador personalizado
            var finalList = originalList.Distinct(new CompareItemReference()).ToList();
            return finalList;
        }
        
        static IEnumerable<StatisticItemData> SumStatistic(IEnumerable<LogResults> logResultsList)
        {
            var list = logResultsList
                .OrderBy(localName => localName.enemy.localizeName.GetLocalizedString())
                .GroupBy(localName => localName.enemy.localizeName.GetLocalizedString())
                .Select(
                    grouping => new StatisticItemData()
                    {
                        itemString = grouping.Key,
                        itemValue = grouping.Sum(quantity => quantity.quantity).ToString()
                    }
                );
            return list;
        }
       

        /*
         [SerializeField] GamePlayingLog gamePlayingLog;
        [SerializeField] GameSettings gameSettings;
        [SerializeField] List<StatisticItemData> statisticsDataList;
        [SerializeField] GameObject statisticItemCard;
        
        [SerializeField] List<EnemiesScriptable> enemiesList;
        [SerializeField] List<CollectibleScriptable> collectiblesList;
        
        [SerializeField] List<LocalizedString> statisticNameList;
        public Tools.SerializableDictionary<GameStatistics, LocalizedString> referenceString;
        const string PT_BR_LOCALIZATION = "Portuguese (Brazil) (pt-BR)";

        #region UNITYMETHODS
        void OnEnable()
        {
            
            //GetStatisticData();
            //DisplayStatisticData();
        }
        void Start()
        {
            var tt = LocalizationSettings.StringDatabase.GetTable("StatisticsTable");

            foreach (var m_keys in tt)
            {
                Debug.Log($"Table: {m_keys.Value.Key}");
            }

            //Debug.Log($"Table: {tt.GetEntry("Statistic_MaxPathDistance").Value}");
           
        }

        void OnDisable()
        {
            //ClearContainer();
            //ClearStatisticList();
        }
  #endregion


        
        void GetStatisticData()
        {
            if (gamePlayingLog == null) return;

            LocalizationSettings.StringDatabase.GetDefaultTableAsync();
            // Score
            statisticsDataList.Add(AddStatisticDataToList(
                "Pontos acumulados",
                "Score",
                gamePlayingLog.totalScore.ToString()
            ));

            // Time
            statisticsDataList.Add(AddStatisticDataToList(
                "Tempo Jogando",
                "Time Playing",
                Tools.TimeFormat(gamePlayingLog.timeSpent)
            ));

            // Max Path Distance
            statisticsDataList.Add(AddStatisticDataToList(
                "Distância Percorrida",
                "Distance traveled",
                $"{gamePlayingLog.maxPathDistance} KM"
            ));

            // Shoot Spent
            statisticsDataList.Add(AddStatisticDataToList(
                "Tiros Disparados",
                "Shots Fired",
                gamePlayingLog.shootSpent.ToString(CultureInfo.CurrentCulture)
            ));

            // Bomb Spent
            statisticsDataList.Add(AddStatisticDataToList(
                "Bombas Utilizadas",
                "Bombs Spent",
                gamePlayingLog.bombSpent.ToString()
            ));

            // Fuel Spent
            statisticsDataList.Add(AddStatisticDataToList(
                "Combustivel Gasto",
                "Fuel Spent",
                $"{gamePlayingLog.fuelSpent} L"
            ));

            // Life Spent
            statisticsDataList.Add(AddStatisticDataToList(
                "Vidas Gastas",
                "Life Spent",
                gamePlayingLog.livesSpent.ToString()
            ));

            // Death by Wall
            statisticsDataList.Add(AddStatisticDataToList(
                "Colisão",
                "Air Crash",
                gamePlayingLog.playerDieWall.ToString()
            ));

            // Death by Bullets
            statisticsDataList.Add(AddStatisticDataToList(
                "Abatido",
                "Shot Down",
                gamePlayingLog.playerDieBullet.ToString()
            ));

            // Die by Bullets
            statisticsDataList.Add(AddStatisticDataToList(
                "Sem Combustivel",
                "No Fuel",
                gamePlayingLog.playerDieFuelEmpty.ToString()
            ));

            // Completed Levels        
            statisticsDataList.Add(AddStatisticDataToList(
                "Missões Concluídas",
                "Missions Completed",
                gamePlayingLog.finishLevels.Count.ToString()
            ));

            // Blank Space
            statisticsDataList.Add(AddStatisticDataToList(
                "",
                "",
                ""
            ));

            // Enemies Destroyed
            int quantity;

            if (gamePlayingLog.hitEnemiesResultsList.Count != 0)
            {

                statisticsDataList.Add(AddStatisticDataToList(
                    "Inimigos Abatidos:",
                    "Enemies Shot Down:",
                    ""
                ));

                foreach (var enemy in enemiesList)
                {
                    quantity = gamePlayingLog.hitEnemiesResultsList.Where(log => log.enemy.name == enemy.name).Sum(log => log.quantity);

                    statisticsDataList.Add(AddStatisticDataToList(
                        enemy.namePT_BR,
                        enemy.name,
                        quantity.ToString()
                    ));
                }
            }

            // Blank Space
            statisticsDataList.Add(AddStatisticDataToList(
                "",
                "",
                ""
            ));

            // Collectable Itens
            if (gamePlayingLog.hitEnemiesResultsList.Count != 0)
            {

                statisticsDataList.Add(AddStatisticDataToList(
                    "Coletáveis:",
                    "Colectables:",
                    ""
                ));

                foreach (var col in collectiblesList)
                {
                    quantity = gamePlayingLog.hitEnemiesResultsList.Where(log => log.enemy == col).Sum(log => log.quantity);

                    statisticsDataList.Add(AddStatisticDataToList(
                        col.namePT_BR,
                        col.name,
                        quantity.ToString()
                    ));
                }
            }
        }


        static StatisticItemData AddStatisticDataToList(string dataNamePt, string dataNameEn, string dataValue)
        {
            var itemData = new StatisticItemData
            {
                itemNamePt = dataNamePt,
                itemNameEn = dataNameEn,
                itemValue = dataValue
            };

            return itemData;
        }

        void DisplayStatisticData()
        {
            foreach (var itemData in statisticsDataList)
            {
                var itemCard = Instantiate(statisticItemCard, containerTransform);

                itemCard.GetComponent<ItemCardDisplayHolder>().itemNameText.text = gameSettings.startLocale.LocaleName == PT_BR_LOCALIZATION ? itemData.itemNamePt : itemData.itemNameEn;

                itemCard.GetComponent<ItemCardDisplayHolder>().itemValueText.text = itemData.itemValue;
            }
        }

        void ClearContainer()
        {
            foreach (Transform child in containerTransform)
            {
                Destroy(child.gameObject);
            }
        }

        void ClearStatisticList()
        {
            statisticsDataList.Clear();
        }*/
    }
    class CompareItemReference : IEqualityComparer<StatisticItemData>
    {
        public bool Equals(StatisticItemData x, StatisticItemData y)
        {
            return x!.itemReference == y!.itemReference;
        }

        public int GetHashCode(StatisticItemData obj)
        {
            return obj.itemReference.GetHashCode();
        }
    }
    [System.Serializable]
    public class StatisticItemData
    {
        public GameStatistics itemReference;
        public LocalizedString itemLocalizedString;
        [HideInInspector]
        public string itemString;
        [HideInInspector]
        public string itemValue;
        [HideInInspector]
        public IEnumerable<StatisticItemData> itemStatisticsList;
    }
}
