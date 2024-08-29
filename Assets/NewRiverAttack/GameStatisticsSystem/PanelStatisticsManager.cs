using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using RiverAttack;
using UnityEngine;
using Utils;
using CollectibleScriptable = NewRiverAttack.ObstaclesSystems.ObjectsScriptable.CollectibleScriptable;
using EnemiesScriptable = NewRiverAttack.ObstaclesSystems.ObjectsScriptable.EnemiesScriptable;

namespace NewRiverAttack.GameStatisticsSystem
{
    public class PanelStatisticsManager : MonoBehaviour
    {
        [SerializeField] private Transform containerTransform;
        [SerializeField] private GameObject statisticItemCard;
        [SerializeField] private List<EnemiesScriptable> enemiesList;
        [SerializeField] private List<CollectibleScriptable> collectiblesList;
        [SerializeField] private List<StatisticItemData> statisticsListData;
        private GemeStatisticsDataLog _gemeStatistics;

        #region UnityMethods

        private void OnEnable()
        {
            ClearContainer();
            _gemeStatistics = GemeStatisticsDataLog.instance;
            CreateFullStatisticList();
            //LocalizationSettings.SelectedLocaleChanged += UpdateStatistics;
        }

        #endregion

        #region GetValues

        private string GetScore => _gemeStatistics.playerMaxScore.ToString();
        private string GetSpendTime => Tools.TimeFormat(_gemeStatistics.playerTimeSpent) ?? "0000000";
        private string GetMaxPathDistance => $"{_gemeStatistics.GetAmountDistance} KM";
        private string GetShootSpent => _gemeStatistics.playersShoots.ToString();
        private string GetBombsSpent => _gemeStatistics.playersBombs.ToString();
        private string GetFuelSpent => $"{_gemeStatistics.fuelSpent:F2} L";
        private string GetFuelCharge => $"{_gemeStatistics.fuelCharge:F2} L";
        private string GetDeaths => _gemeStatistics.playerDeaths.ToString();
        private string GetWallDeaths => _gemeStatistics.playersDieWall.ToString();
        private string GetEnemyDeaths => _gemeStatistics.playersDieEnemyCollider.ToString();
        private string GetBulletsDeaths => _gemeStatistics.playersDieEnemyBullets.ToString();
        private string GetFuelOut => _gemeStatistics.playersDieFuelOut.ToString();
        private string GetCompleteMissionPath => _gemeStatistics.playersMissionPath.ToString();
        private string GetCompleteClassicPath => _gemeStatistics.playersClassicPath.ToString();
        private string GetTimeRapidFire => Tools.TimeFormat(_gemeStatistics.playersTimeRapidFire) ?? "0000000";
        private string GetBombHits => _gemeStatistics.playersBombHit.ToString();
        #endregion

        #region Panels Auxiliar
        private void ClearContainer()
        {
            foreach (Transform child in containerTransform)
            {
                Destroy(child.gameObject);
            }
        }

        #endregion

        #region Funções Auxiliares de Lista
        
        private void CreateFullStatisticList()
        {
            statisticsListData = FilterByItemReference(statisticsListData);
            DebugManager.Log<PanelStatisticsManager>($"Lista de statistics Filtrada: {statisticsListData}");
            foreach (var itemData in statisticsListData)
            {
                itemData.itemString = itemData.itemLocalizedString.GetLocalizedString();
                DebugManager.Log<PanelStatisticsManager>($"ItemData: {itemData.itemString}");
                DebugManager.Log<PanelStatisticsManager>($"ItemData itemReference: {itemData.itemReference}");
                itemData.ItemStatisticsList = null;
    
                itemData.itemValue = null;
                
                switch (itemData.itemReference)
                {
                    case EnumGameStatistics.Score:
                        itemData.itemValue = GetScore;
                        break;
                    case EnumGameStatistics.Time:
                        itemData.itemValue = GetSpendTime;
                        break;
                    case EnumGameStatistics.MaxPathDistance:
                        itemData.itemValue = GetMaxPathDistance;
                        break;
                    case EnumGameStatistics.ShootSpent:
                        itemData.itemValue = GetShootSpent;
                        break;
                    case EnumGameStatistics.BombSpent:
                        itemData.itemValue = GetBombsSpent;
                        break;
                    case EnumGameStatistics.FuelSpent:
                        itemData.itemValue = GetFuelSpent;
                        break;
                    case EnumGameStatistics.FuelCharge:
                        itemData.itemValue = GetFuelCharge;
                        break;
                    case EnumGameStatistics.LifeSpent:
                        itemData.itemValue = GetDeaths;
                        break;
                    case EnumGameStatistics.DeathByWall:
                        itemData.itemValue = GetWallDeaths;
                        break;
                    case EnumGameStatistics.DeathByBullets:
                        itemData.itemValue = GetBulletsDeaths;
                        break;
                    case EnumGameStatistics.DeathByEnemy:
                        itemData.itemValue = GetEnemyDeaths;
                        break;
                    case EnumGameStatistics.DieByFuel:
                        itemData.itemValue = GetFuelOut;
                        break;
                    case EnumGameStatistics.CompletedClassicLevels:
                        itemData.itemValue = GetCompleteClassicPath;
                        break;
                    case EnumGameStatistics.CompletedMissionLevels:
                        itemData.itemValue = GetCompleteMissionPath;
                        break;
                    case EnumGameStatistics.RapidFireTimer:
                        itemData.itemValue = GetTimeRapidFire;
                        break;
                    case EnumGameStatistics.BombHits:
                        itemData.itemValue = GetBombHits;
                        break;
                    /*case GameStatistics.EnemiesDestroyed:
                        itemData.ItemStatisticsList = SumStatistic(enemiesList,getEnemiesDestroyed);
                        break;
                    case GameStatistics.CollectableItems:
                        itemData.ItemStatisticsList = SumStatistic(collectiblesList,getCollectableItems);
                        break;*/
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                DebugManager.Log<PanelStatisticsManager>($"ItemData ItemStatisticsList: {itemData.ItemStatisticsList}");
                DebugManager.Log<PanelStatisticsManager>($"ItemData itemValue: {itemData.itemValue}");
                DebugManager.Log<PanelStatisticsManager>($"Cria a linha: {itemData}");
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

        private class CompareItemReference : IEqualityComparer<StatisticItemData>
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
        

        #endregion
    }
}