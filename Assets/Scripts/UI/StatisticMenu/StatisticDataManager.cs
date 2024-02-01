using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Utils;

namespace RiverAttack 
{
    public class StatisticDataManager : MonoBehaviour
    {
        [SerializeField] GamePlayingLog gamePlayingLog;
        [SerializeField] GameSettings gameSettings;
        [SerializeField] List<StatisticItemData> statisticsDataList;
        [SerializeField] GameObject statisticItemCard;
        [SerializeField] Transform containerTransform;
        [SerializeField] List<EnemiesScriptable> enemiesList;
        [SerializeField] List<CollectibleScriptable> collectiblesList;

        const string PT_BR_LOCALIZATION = "Portuguese (Brazil) (pt-BR)";
        
        void OnEnable()
        {            
            GetStatisticData();
            DisplayStatisticData();
        }

        void OnDisable()
        {
            ClearContainer();
            ClearStatisticList();
        }

        void GetStatisticData()
        {
            if (gamePlayingLog == null) return;

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

            if (gamePlayingLog.hitEnemiesResultsList.Count != 0) {

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
            if (gamePlayingLog.hitEnemiesResultsList.Count != 0) {

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
            foreach(Transform child in containerTransform)
            {
                Destroy(child.gameObject);
            }
        }

        void ClearStatisticList()
        {
            statisticsDataList.Clear();
        }
    }
}
