using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NewRiverAttack.GameStatisticsSystem;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Utils;

namespace RiverAttack
{
    public class StatisticDataManager : MonoBehaviour
    {
        [SerializeField] private Transform containerTransform;
        [SerializeField] private GameObject statisticItemCard;
        [SerializeField] private List<EnemiesScriptable> enemiesList;
        [SerializeField] private List<CollectibleScriptable> collectiblesList;
        [SerializeField] private List<StatisticItemData> statisticsListData;

        private static string getScore => GamePlayingLog.instance.totalScore.ToString();

        private static string getSpendTime => Tools.TimeFormat(GamePlayingLog.instance.timeSpent);

        private static string getMaxPathDistance => $"{GamePlayingLog.instance.maxPathDistance.ToString(CultureInfo.CurrentCulture)} KM";

        private static string getShootSpent => GamePlayingLog.instance.shootSpent.ToString(CultureInfo.CurrentCulture);

        private static string getBombSpent => $"{GamePlayingLog.instance.bombSpent} KM";

        private static string getFuelSpent => $"{GamePlayingLog.instance.fuelSpent} L";

        private static string getLifeSpent => GamePlayingLog.instance.livesSpent.ToString();

        private static string getDeathByWall => GamePlayingLog.instance.playerDieWall.ToString();

        private static string getDeathByBullets => GamePlayingLog.instance.playerDieBullet.ToString();

        private static string getDeathByFuel => GamePlayingLog.instance.playerDieFuelEmpty.ToString();

        private static string getCompletedLevels => GamePlayingLog.instance.GetLevelsResult().Count.ToString();

        private IEnumerable<LogResults> getEnemiesDestroyed
        {
            get { return GamePlayingLog.instance.GetEnemiesResult().Where(item=>enemiesList.Contains(item.enemy))
                .ToList(); }
        }

        private IEnumerable<LogResults> getCollectableItems
        {
            get { return GamePlayingLog.instance.GetEnemiesResult().Where(item=>collectiblesList.Contains(item.enemy)).ToList(); }
        }

        private void OnEnable()
        {
            ClearContainer();
            CreateFullStatisticList();
            LocalizationSettings.SelectedLocaleChanged += UpdateStatistics;
        }

        private void UpdateStatistics(Locale obj)
        {
            ClearContainer();
            CreateFullStatisticList();
        }

        private void ClearContainer()
        {
            foreach (Transform child in containerTransform)
            {
                Destroy(child.gameObject);
            }
        }


        private void CreateFullStatisticList()
        {
            statisticsListData = FilterByItemReference(statisticsListData);
            foreach (var itemData in statisticsListData)
            {
                itemData.itemString = itemData.itemLocalizedString.GetLocalizedString();
                itemData.ItemStatisticsList = null;
                itemData.itemValue = null;
                switch (itemData.itemReference)
                {
                    case EnumGameStatistics.Score:
                        itemData.itemValue = getScore;
                        break;
                    case EnumGameStatistics.Time:
                        itemData.itemValue = getSpendTime;
                        break;
                    case EnumGameStatistics.MaxPathDistance:
                        itemData.itemValue = getMaxPathDistance;
                        break;
                    case EnumGameStatistics.ShootSpent:
                        itemData.itemValue = getShootSpent;
                        break;
                    case EnumGameStatistics.BombSpent:
                        itemData.itemValue = getBombSpent;
                        break;
                    case EnumGameStatistics.FuelSpent:
                        itemData.itemValue = getFuelSpent;
                        break;
                    case EnumGameStatistics.LifeSpent:
                        itemData.itemValue = getLifeSpent;
                        break;
                    case EnumGameStatistics.DeathByWall:
                        itemData.itemValue = getDeathByWall;
                        break;
                    case EnumGameStatistics.DeathByBullets:
                        itemData.itemValue = getDeathByBullets;
                        break;
                    case EnumGameStatistics.DieByFuel:
                        itemData.itemValue = getDeathByFuel;
                        break;
                    case EnumGameStatistics.CompletedClassicLevels:
                        itemData.itemValue = getCompletedLevels;
                        break;
                    case EnumGameStatistics.EnemiesDestroyed:
                        itemData.ItemStatisticsList = SumStatistic(enemiesList,getEnemiesDestroyed);
                        break;
                    case EnumGameStatistics.CollectableItems:
                        itemData.ItemStatisticsList = SumStatistic(collectiblesList,getCollectableItems);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                CreateItem(itemData);
            }
        }

        private void CreateItem(StatisticItemData itemData)
        {
            if (itemData.ItemStatisticsList != null)
            {
                CreateSubItem(new StatisticItemData()); // Cria uma linha em Branco
            }
            CreateSubItem(itemData); // Cria o item ou Cria o titulo
            if (itemData.ItemStatisticsList == null)
                return;
            foreach (var newItemData in itemData.ItemStatisticsList)
            {
                CreateSubItem(newItemData); // Cria uma linha em Branco
            }
        }

        private void CreateSubItem(StatisticItemData itemData)
        {
            var itemCard = Instantiate(statisticItemCard, containerTransform);
            itemCard.GetComponent<ItemCardDisplayHolder>().itemNameText.text = itemData.itemString;
            itemCard.GetComponent<ItemCardDisplayHolder>().itemValueText.text = itemData.itemValue;
        }

        private static List<StatisticItemData> FilterByItemReference(IEnumerable<StatisticItemData> originalList)
        {
            // Usa Distinct com um comparador personalizado
            var finalList = originalList.Distinct(new CompareItemReference()).ToList();
            return finalList;
        }

        private static IEnumerable<StatisticItemData> SumStatistic(IEnumerable<EnemiesScriptable> enemiesList, IEnumerable<LogResults> logResultsList)
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

    internal class CompareItemReference : IEqualityComparer<StatisticItemData>
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
    
}
