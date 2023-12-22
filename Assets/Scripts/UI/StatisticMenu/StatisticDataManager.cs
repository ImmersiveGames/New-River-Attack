using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack 
{
    public class StatisticDataManager : MonoBehaviour
    {
        [SerializeField] GamePlayingLog gamePlayingLog;
        [SerializeField] GameSettings gameSettings;
        [SerializeField] List<StatisticItemData> statisticsDataList;
        [SerializeField] GameObject statisticItemCard;
        [SerializeField] Transform containerTransform;
        [SerializeField] List<EnemiesScriptable> enemysList;
        [SerializeField] List<CollectibleScriptable> collectiblesList;

        const string PT_BR_LOCALIZATION = "Portuguese (Brazil) (pt-BR)";
        const string EN_LOCALIZATION = "English (en)";

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
                TimeFormat(gamePlayingLog.timeSpent)
                ));

            // Max Path Distance
            statisticsDataList.Add(AddStatisticDataToList(
                "Distancia Percorida",
                "Distance traveled",
                gamePlayingLog.maxPathDistance.ToString() + " Km"
                ));

            // Shoot Spent
            statisticsDataList.Add(AddStatisticDataToList(
                "Tiros Disparados",
                "Shots Fired",
                gamePlayingLog.shootSpent.ToString()
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
                gamePlayingLog.fuelSpent.ToString() + " L"
                ));

            // Life Spent
            statisticsDataList.Add(AddStatisticDataToList(
                "Vidas Gastas",
                "Life Spent",
                gamePlayingLog.livesSpent.ToString()
                ));

            // Die by Wall
            statisticsDataList.Add(AddStatisticDataToList(
                "Colisão",
                "Air Crash",
                gamePlayingLog.playerDieWall.ToString()
                ));

            // Die by Bullets
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
            int quantity = 0;

            if (gamePlayingLog.hitEnemiesResultsList.Count != 0) {

                statisticsDataList.Add(AddStatisticDataToList(
                    "Inimigos Abatidos:",
                    "Enemies Shot Down:",
                    ""
                ));

                foreach (EnemiesScriptable enemy in enemysList)
                {
                    quantity = 0;
                    foreach (LogResults log in gamePlayingLog.hitEnemiesResultsList)
                    {
                        if (log.enemy.name == enemy.name)
                        {
                            quantity += log.quantity;
                        }
                    }

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

                foreach (CollectibleScriptable col in collectiblesList)
                {
                    quantity = 0;

                    foreach (LogResults log in gamePlayingLog.hitEnemiesResultsList)
                    {
                        if (log.enemy == col)
                        {
                            quantity += log.quantity;
                        }
                    }

                    statisticsDataList.Add(AddStatisticDataToList(
                        col.namePT_BR,
                        col.name,
                        quantity.ToString()
                    ));
                }
            }
        }

        private string TimeFormat(float timeToFormat)
        {
            int hour = Mathf.FloorToInt(timeToFormat / 3600);
            int minutes = Mathf.FloorToInt((timeToFormat % 3600) / 60);
            int seconds = Mathf.FloorToInt(timeToFormat % 60);

            string time = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minutes, seconds);
            
            return time;
        }

        StatisticItemData AddStatisticDataToList(string dataNamePT, string dataNameEN, string dataValue)
        {
            StatisticItemData itemData = new StatisticItemData();
            
            itemData.itemNamePT = dataNamePT;
            itemData.itemNameEN = dataNameEN;
            itemData.itemValue = dataValue;

            return itemData;
        }

        void DisplayStatisticData()
        {
            foreach (StatisticItemData itemData in statisticsDataList)
            {
                var itemCard = Instantiate(statisticItemCard, containerTransform);
                
                if (gameSettings.startLocale.LocaleName == PT_BR_LOCALIZATION) 
                    itemCard.GetComponent<ItemCardDisplayHolder>().itemNameText.text = itemData.itemNamePT;

                else 
                    itemCard.GetComponent<ItemCardDisplayHolder>().itemNameText.text = itemData.itemNameEN;

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
