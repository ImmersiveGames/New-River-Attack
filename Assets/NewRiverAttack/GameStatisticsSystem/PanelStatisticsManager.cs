using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using RiverAttack;
using UnityEngine;
using UnityEngine.Localization.Settings;
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
        private string GetMaxPathDistance => $"{_gemeStatistics.playerMaxDistance.ToString(CultureInfo.CurrentCulture)} KM" ?? "0 KM";

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
                    /*case GameStatistics.MaxPathDistance:
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