using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
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
        static string getScore
        {
            get { return GamePlayingLog.instance.totalScore.ToString(); }
        }
        static string getSpendTime
        {
            get { return Tools.TimeFormat(GamePlayingLog.instance.timeSpent); }
        }
        static string getMaxPathDistance
        {
            get { return $"{GamePlayingLog.instance.maxPathDistance.ToString(CultureInfo.CurrentCulture)} KM"; }
        }
        static string getShootSpent
        {
            get { return GamePlayingLog.instance.shootSpent.ToString(CultureInfo.CurrentCulture); }
        }
        static string getBombSpent
        {
            get { return $"{GamePlayingLog.instance.bombSpent} KM"; }
        }
        static string getFuelSpent
        {
            get { return $"{GamePlayingLog.instance.fuelSpent} L"; }
        }
        static string getLifeSpent
        {
            get { return GamePlayingLog.instance.livesSpent.ToString(); }
        }
        static string getDeathByWall
        {
            get { return GamePlayingLog.instance.playerDieWall.ToString(); }
        }
        static string getDeathByBullets
        {
            get { return GamePlayingLog.instance.playerDieBullet.ToString(); }
        }
        static string getDeathByFuel
        {
            get { return GamePlayingLog.instance.playerDieFuelEmpty.ToString(); }
        }
        static string getCompletedLevels
        {
            get { return GamePlayingLog.instance.finishLevels.Count.ToString(); }
        }
        IEnumerable<LogResults> getEnemiesDestroyed
        {
            get { return GamePlayingLog.instance.hitEnemiesResultsList.Where(item=>enemiesList.Contains(item.enemy))
                .ToList(); }
        }
        IEnumerable<LogResults> getCollectableItems
        {
            get { return GamePlayingLog.instance.hitEnemiesResultsList.Where(item=>collectiblesList.Contains(item.enemy)).ToList(); }
        }
        void OnEnable()
        {
            ClearContainer();
            CreateFullStatisticList();
            LocalizationSettings.SelectedLocaleChanged += UpdateStatistics;
        }
        void UpdateStatistics(Locale obj)
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
                        itemData.itemValue = getLifeSpent;
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
                        itemData.itemStatisticsList = SumStatistic(enemiesList,getEnemiesDestroyed);
                        break;
                    case GameStatistics.CollectableItems:
                        itemData.itemStatisticsList = SumStatistic(collectiblesList,getCollectableItems);
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
        
        static IEnumerable<StatisticItemData> SumStatistic(IEnumerable<EnemiesScriptable> enemiesList, IEnumerable<LogResults> logResultsList)
        {
            var list = enemiesList
                .OrderBy(localName => localName.localizeName.GetLocalizedString())
                .GroupBy(localName => localName.localizeName.GetLocalizedString())
                .Select(
                    grouping => new StatisticItemData()
                    {
                        itemString = grouping.Key,
                        itemValue = logResultsList.Where(log => log.enemy.localizeName.GetLocalizedString() == grouping.Key)
                            .Sum(log => log.quantity).ToString()
                    }
                );
            return list;
        }
        
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
    [Serializable]
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
