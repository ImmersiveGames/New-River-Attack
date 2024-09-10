using CarterGames.Assets.SaveManager;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using Saves;
using UnityEngine;

namespace NewRiverAttack.SaveManagers
{
    public class GameSaveHandler : MonoBehaviour
    {
        [SerializeField] private GemeStatisticsDataLog dataLog;
        [SerializeField] private GameOptionsSave gameOptionsSave;
        [SerializeField] private PlayerSaveSaveObject saveObject;
        
        public static GameSaveHandler Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DebugManager.Log<GameSaveHandler>("Instância criada e marcada para não destruir ao carregar uma nova cena.");
            }
            else
            {
                Destroy(gameObject);
                DebugManager.LogWarning<GameSaveHandler>("Tentativa de criar outra instância evitada e o novo objeto foi destruído.");
            }
        }
        private void OnEnable()
        {
            saveObject = SaveManager.GetSaveObject<PlayerSaveSaveObject>();
            LoadGameData();
        }

        private void Start()
        {
            LoadGameLocation();
        }

        private void OnApplicationQuit()
        {
            SaveGameData();
        }
        private void LoadGameLocation()
        {
            if(saveObject.startLocale.Value == null) return;
            gameOptionsSave.startLocale = saveObject.startLocale.Value;
            Debug.Log(gameOptionsSave.startLocale);
        }
        
        private void SaveGameLocation()
        {
            if(gameOptionsSave.startLocale == null) return;
            saveObject.startLocale.Value = gameOptionsSave.startLocale;
            Debug.Log(saveObject.startLocale.Value);
        }

        private void LoadGameData()
        {
            saveObject.Load();
            
        // Game Options
        if(saveObject.bgmVolume.Value != 0)
            gameOptionsSave.bgmVolume = saveObject.bgmVolume.Value;
        if(saveObject.sfxVolume.Value != 0)
            gameOptionsSave.sfxVolume = saveObject.sfxVolume.Value;
        if(saveObject.frameRate.Value != 0)
            gameOptionsSave.frameRate = saveObject.frameRate.Value;
        if(saveObject.actualResolution.Value != Vector2Int.zero)
            gameOptionsSave.actualResolution = saveObject.actualResolution.Value;
        if(saveObject.selectedQualityIndex.Value != 0)
            gameOptionsSave.selectedQualityIndex = saveObject.selectedQualityIndex.Value;
        
        //Player Game
        if(saveObject.wallet.Value != 0)
            gameOptionsSave.wallet = saveObject.wallet.Value;
        if(saveObject.listPlayerProductStocks.Value.Count > 0)
            gameOptionsSave.listPlayerProductStocks = saveObject.listPlayerProductStocks.Value;
        if(saveObject.playerSettings.Value.Length > 0)
            gameOptionsSave.playerSettings = saveObject.playerSettings.Value;
        
        //Mission Mode
        if(saveObject.activeIndexMissionLevel.Value != 0)
            gameOptionsSave.activeIndexMissionLevel = saveObject.activeIndexMissionLevel.Value;
        if(saveObject.missionLives.Value != 0)
            gameOptionsSave.missionLives = saveObject.missionLives.Value;
        if(saveObject.missionBombs.Value != 0)
            gameOptionsSave.missionBombs = saveObject.missionBombs.Value;
        
        //DataLog
        if(saveObject.playersMaxScore.Value != 0)
            dataLog.playersMaxScore = saveObject.playersMaxScore.Value;
        if(saveObject.playersTimeSpent.Value != 0)
            dataLog.playersTimeSpent = saveObject.playersTimeSpent.Value;
        if(saveObject.playersMaxDistance.Value != 0)
            dataLog.playersMaxDistance = saveObject.playersMaxDistance.Value;
        if(saveObject.playersDeaths.Value != 0)
            dataLog.playersDeaths = saveObject.playersDeaths.Value;
        if(saveObject.playersDieWall.Value != 0)
            dataLog.playersDieWall = saveObject.playersDieWall.Value;
        if(saveObject.playersDieEnemyCollider.Value != 0)
            dataLog.playersDieEnemyCollider = saveObject.playersDieEnemyCollider.Value;
        if(saveObject.playersDieEnemyBullets.Value != 0)
            dataLog.playersDieEnemyBullets = saveObject.playersDieEnemyBullets.Value;
        if(saveObject.playersDieFuelOut.Value != 0)
            dataLog.playersDieFuelOut = saveObject.playersDieFuelOut.Value;
        if(saveObject.playersShoots.Value != 0)
            dataLog.playersShoots = saveObject.playersShoots.Value;
        if(saveObject.playersBombs.Value != 0)
            dataLog.playersBombs = saveObject.playersBombs.Value;
        if(saveObject.playersTimeRapidFire.Value != 0)
            dataLog.playersTimeRapidFire = saveObject.playersTimeRapidFire.Value;
        if(saveObject.playersBombHit.Value != 0)
            dataLog.playersBombHit = saveObject.playersBombHit.Value;
        if(saveObject.playersFuelSpent.Value != 0)
            dataLog.playersFuelSpent = saveObject.playersFuelSpent.Value;
        if(saveObject.playersFuelCharge.Value != 0)
            dataLog.playersFuelCharge = saveObject.playersFuelCharge.Value;
        if(saveObject.playersAmountDistance.Value != 0)
            dataLog.playersAmountDistance = saveObject.playersAmountDistance.Value;
        if(saveObject.playersClassicPath.Value != 0)
            dataLog.playersClassicPath = saveObject.playersClassicPath.Value;
        if(saveObject.playersMissionPath.Value != 0)
            dataLog.playersMissionPath = saveObject.playersMissionPath.Value;
        if(saveObject.hitEnemiesResultsList.Value.Count > 0)
            dataLog.GetEnemyList = saveObject.hitEnemiesResultsList.Value;
        }

        public void SaveGameData()
        {
            if(saveObject == null) return;
            SaveGameLocation();
            
            // Game Options
            if(!Mathf.Approximately(gameOptionsSave.bgmVolume, saveObject.bgmVolume.Value))
                saveObject.bgmVolume.Value = gameOptionsSave.bgmVolume;
            if(!Mathf.Approximately(gameOptionsSave.sfxVolume, saveObject.sfxVolume.Value))
                gameOptionsSave.sfxVolume = saveObject.sfxVolume.Value;
            if(saveObject.frameRate.Value != gameOptionsSave.frameRate)
                saveObject.frameRate.Value = gameOptionsSave.frameRate;
            if(saveObject.actualResolution.Value != gameOptionsSave.actualResolution)
                saveObject.actualResolution.Value = gameOptionsSave.actualResolution;
            if(saveObject.selectedQualityIndex.Value != gameOptionsSave.selectedQualityIndex)
                 saveObject.selectedQualityIndex.Value = gameOptionsSave.selectedQualityIndex;
            
            //Player Game
            if(gameOptionsSave.wallet != saveObject.wallet.Value)
                saveObject.wallet.Value = gameOptionsSave.wallet;
            if(saveObject.listPlayerProductStocks.Value != gameOptionsSave.listPlayerProductStocks)
                saveObject.listPlayerProductStocks.Value = gameOptionsSave.listPlayerProductStocks;
            if(saveObject.playerSettings.Value != gameOptionsSave.playerSettings)
                saveObject.playerSettings.Value = gameOptionsSave.playerSettings;
            
            //Mission Mode
            if(saveObject.activeIndexMissionLevel.Value != gameOptionsSave.activeIndexMissionLevel)
                saveObject.activeIndexMissionLevel.Value = gameOptionsSave.activeIndexMissionLevel;
            if(saveObject.missionLives.Value != gameOptionsSave.missionLives)
                saveObject.missionLives.Value = gameOptionsSave.missionLives;
            if(saveObject.missionBombs.Value != gameOptionsSave.missionBombs)
                saveObject.missionBombs.Value = gameOptionsSave.missionBombs;
            
            //DataLog
        if(saveObject.playersMaxScore.Value != dataLog.playersMaxScore)
            saveObject.playersMaxScore.Value = dataLog.playersMaxScore;
        if(!Mathf.Approximately(saveObject.playersTimeSpent.Value, dataLog.playersTimeSpent))
             saveObject.playersTimeSpent.Value = dataLog.playersTimeSpent;
        if(saveObject.playersMaxDistance.Value != dataLog.playersMaxDistance)
            saveObject.playersMaxDistance.Value = dataLog.playersMaxDistance;
        if(saveObject.playersDeaths.Value != dataLog.playersDeaths)
            saveObject.playersDeaths.Value = dataLog.playersDeaths;
        if(saveObject.playersDieWall.Value != dataLog.playersDieWall)
            saveObject.playersDieWall.Value = dataLog.playersDieWall;
        if(saveObject.playersDieEnemyCollider.Value != dataLog.playersDieEnemyCollider)
            saveObject.playersDieEnemyCollider.Value = dataLog.playersDieEnemyCollider;
        if(saveObject.playersDieEnemyBullets.Value != dataLog.playersDieEnemyBullets)
            saveObject.playersDieEnemyBullets.Value = dataLog.playersDieEnemyBullets;
        if(saveObject.playersDieFuelOut.Value != dataLog.playersDieFuelOut)
            saveObject.playersDieFuelOut.Value = dataLog.playersDieFuelOut;
        if(saveObject.playersShoots.Value != dataLog.playersShoots)
            saveObject.playersShoots.Value = dataLog.playersShoots;
        if(saveObject.playersBombs.Value != dataLog.playersBombs)
            saveObject.playersBombs.Value = dataLog.playersBombs;
        if(!Mathf.Approximately(saveObject.playersTimeRapidFire.Value, dataLog.playersTimeRapidFire))
             saveObject.playersTimeRapidFire.Value = dataLog.playersTimeRapidFire;
        if(saveObject.playersBombHit.Value != dataLog.playersBombHit)
            saveObject.playersBombHit.Value = dataLog.playersBombHit;
        if(!Mathf.Approximately(saveObject.playersFuelSpent.Value, dataLog.playersFuelSpent))
            saveObject.playersFuelSpent.Value = dataLog.playersFuelSpent;
        if(!Mathf.Approximately(saveObject.playersFuelCharge.Value, dataLog.playersFuelCharge))
            saveObject.playersFuelCharge.Value = dataLog.playersFuelCharge;
        if(!Mathf.Approximately(saveObject.playersAmountDistance.Value, dataLog.playersAmountDistance))
            saveObject.playersAmountDistance.Value = dataLog.playersAmountDistance;
        if(saveObject.playersClassicPath.Value != dataLog.playersClassicPath)
            saveObject.playersClassicPath.Value = dataLog.playersClassicPath;
        if(saveObject.playersMissionPath.Value != dataLog.playersMissionPath)
            saveObject.playersMissionPath.Value = dataLog.playersMissionPath;
        if(saveObject.hitEnemiesResultsList.Value != dataLog.GetEnemyList)
            saveObject.hitEnemiesResultsList.Value = dataLog.GetEnemyList;
            
            //Update Files
            saveObject.Save();
            SaveManager.Save();
        }

        public void ClearGameData()
        {
            saveObject.ResetObjectSaveValues();
            SaveManager.Save();
        }
    }
}