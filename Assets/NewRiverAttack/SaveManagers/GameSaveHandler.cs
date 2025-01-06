using CarterGames.Assets.SaveManager;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using Saves;
using UnityEngine;

namespace NewRiverAttack.SaveManagers
{
    public class GameSaveHandler : MonoBehaviour
    {
        [SerializeField] private GemeStatisticsDataLog dataLog;
        [SerializeField] private GameOptionsSave gameOptionsSave;
        [SerializeField] private RiverSaveObject saveObject;

        [SerializeField] private string versionTarget;
        
        public static GameSaveHandler Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                DebugManager.LogWarning<GameSaveHandler>("Instância duplicada destruída.");
            }
        }
        
        private void OnEnable()
        {
            saveObject = SaveManager.GetSaveObject<RiverSaveObject>();
            LoadGameData();
        }

        private void Start()
        {
            LoadGameLocation();

            CheckLastVersion();
        }

        private void OnDisable()
        {
            SaveGameData();
        }

        private void CheckLastVersion()
        {
            if (string.IsNullOrEmpty(saveObject.lastVersion.Value))
            {
                DebugManager.Log<GameSaveHandler>("Não há uma versão salva " + versionTarget);
                ResetFiles();
                return;
            }

            if (saveObject.lastVersion == null ||
                !VersionChecker.IsTargetVersionLowerThenSave(versionTarget, saveObject.lastVersion.Value)) return;
            DebugManager.Log<GameSaveHandler>("A versão salva atual é menor que a versão Alvo " + saveObject.lastVersion.Value);
            ResetFiles();
        }

        private void ResetFiles()
        {
            Debug.Log("RESETOU OS SAVE");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();  // Salva a exclusão dos dados no PlayerPrefs
            saveObject.ResetObjectSaveValues();
            SaveManager.Save();  // Salva o novo estado do saveObject
            // Limpa os logs também
            dataLog.ResetLogs();
        }

        private void OnApplicationQuit()
        {
            SaveGameData();
        }

        private void LoadGameLocation()
        {
            if (saveObject.startLocale == null) return;
            gameOptionsSave.startLocale = saveObject.startLocale.Value;
            DebugManager.Log<GameSaveHandler>($"Locale carregado: {gameOptionsSave.startLocale}");
        }
        
        private void SaveGameLocation()
        {
            if (gameOptionsSave.startLocale == null) return;
            saveObject.startLocale.Value = gameOptionsSave.startLocale;
            DebugManager.Log<GameSaveHandler>($"Locale salvo: {saveObject.startLocale.Value}");
        }

        private void LoadGameData()
        {
            saveObject.Load();  // Carrega os dados do SaveObject
            LoadGameOptions();
            LoadGameStatistics();
        }

        public void SaveGameData()
        {
            if (saveObject == null) return;

            SaveGameLocation();
            SaveGameOptions();
            SaveGameStatistics();
            // Atualiza a data do último save
            saveObject.lastVersion.Value = Application.version;

            // Salva o estado atualizado
            saveObject.Save();
            SaveManager.Save();
        }

        private void SaveGameOptions()
        {
            // Exemplo para salvar uma opção
            if (!Mathf.Approximately(saveObject.bgmVolume.Value, gameOptionsSave.bgmVolume))
                saveObject.bgmVolume.Value = gameOptionsSave.bgmVolume;
            
            if (!Mathf.Approximately(saveObject.sfxVolume.Value, gameOptionsSave.sfxVolume))
                saveObject.sfxVolume.Value = gameOptionsSave.sfxVolume;
            
            saveObject.selectedQualityIndex.Value = gameOptionsSave.selectedQualityIndex;
            saveObject.actualResolution.Value = gameOptionsSave.actualResolution;
            saveObject.frameRate.Value = gameOptionsSave.frameRate;
                 
            saveObject.listPlayerProductStocks.Value = gameOptionsSave.listPlayerProductStocks;
    
            saveObject.playerSettings.Value = gameOptionsSave.playerSettings;
            
            saveObject.wallet.Value = gameOptionsSave.wallet;
            
            saveObject.activeIndexMissionLevel.Value = gameOptionsSave.activeIndexMissionLevel;
            saveObject.missionLives.Value = gameOptionsSave.missionLives;
            saveObject.missionBombs.Value = gameOptionsSave.missionBombs;
        }

        private void SaveGameStatistics()
        {
            saveObject.playersMaxScore.Value = dataLog.playersMaxScore;
            saveObject.playersTimeSpent.Value = dataLog.playersTimeSpent;
            saveObject.playersMaxDistance.Value = dataLog.playersMaxDistance;
            saveObject.playersDeaths.Value = dataLog.playersDeaths;
            saveObject.playersDieWall.Value = dataLog.playersDieWall;
            saveObject.playersDieEnemyCollider.Value = dataLog.playersDieEnemyCollider;
            saveObject.playersDieEnemyBullets.Value = dataLog.playersDieEnemyBullets;
            saveObject.playersDieFuelOut.Value = dataLog.playersDieFuelOut;
            saveObject.playersShoots.Value = dataLog.playersShoots;
            saveObject.playersBombs.Value = dataLog.playersBombs;
            saveObject.playersTimeRapidFire.Value = dataLog.playersTimeRapidFire;
            saveObject.playersBombHit.Value = dataLog.playersBombHit;
            saveObject.playersFuelSpent.Value = dataLog.playersFuelSpent;
            saveObject.playersFuelCharge.Value = dataLog.playersFuelCharge;
            saveObject.playersAmountDistance.Value = dataLog.playersAmountDistance;
            saveObject.playersClassicPath.Value = dataLog.playersClassicPath;
            saveObject.playersMissionPath.Value = dataLog.playersMissionPath;
            saveObject.playersCountPath.Value = dataLog.playersCountPath;
            saveObject.hitEnemiesResultsList.Value = dataLog.GetEnemyList;
        }
        
        private void LoadGameOptions()
        {
            // Exemplo para carregar opções do jogo
            if (saveObject.bgmVolume.Value != 0)
                gameOptionsSave.bgmVolume = saveObject.bgmVolume.Value;
            if (saveObject.sfxVolume.Value != 0)
                gameOptionsSave.sfxVolume = saveObject.sfxVolume.Value;
            
            if (saveObject.selectedQualityIndex.Value != 0)
                gameOptionsSave.selectedQualityIndex = saveObject.selectedQualityIndex.Value;
            if (saveObject.actualResolution.Value != Vector2Int.zero)
                gameOptionsSave.actualResolution = saveObject.actualResolution.Value;
            if (saveObject.frameRate.Value != 0)
                gameOptionsSave.frameRate = saveObject.frameRate.Value;

            if (saveObject.listPlayerProductStocks.Value.Count > 0)
                gameOptionsSave.listPlayerProductStocks = saveObject.listPlayerProductStocks.Value;
            if (saveObject.playerSettings.Value.Length > 0)
                gameOptionsSave.playerSettings = saveObject.playerSettings.Value;
            
                gameOptionsSave.wallet = saveObject.wallet.Value;
            
            if (saveObject.activeIndexMissionLevel.Value != 0)
                gameOptionsSave.activeIndexMissionLevel = saveObject.activeIndexMissionLevel.Value;
            if (saveObject.missionLives.Value != 0)
                gameOptionsSave.missionLives = saveObject.missionLives.Value;
            if (saveObject.missionBombs.Value != 0)
                gameOptionsSave.missionBombs = saveObject.missionBombs.Value;
        }
        private void LoadGameStatistics()
        {
            if (saveObject.playersMaxScore.Value != 0)
                dataLog.playersMaxScore = saveObject.playersMaxScore.Value;
            if (saveObject.playersTimeSpent.Value != 0)
                dataLog.playersTimeSpent = saveObject.playersTimeSpent.Value;
            if (saveObject.playersMaxDistance.Value != 0)
                dataLog.playersMaxDistance = saveObject.playersMaxDistance.Value;
            if (saveObject.playersDeaths.Value != 0)
                dataLog.playersDeaths = saveObject.playersDeaths.Value;
            if (saveObject.playersDieWall.Value != 0)
                dataLog.playersDieWall = saveObject.playersDieWall.Value;
            if (saveObject.playersDieEnemyCollider.Value != 0)
                dataLog.playersDieEnemyCollider = saveObject.playersDieEnemyCollider.Value;
            if (saveObject.playersDieEnemyBullets.Value != 0)
                dataLog.playersDieEnemyBullets = saveObject.playersDieEnemyBullets.Value;
            if (saveObject.playersDieFuelOut.Value != 0)
                dataLog.playersDieFuelOut = saveObject.playersDieFuelOut.Value;
            if (saveObject.playersShoots.Value != 0)
                dataLog.playersShoots = saveObject.playersShoots.Value;
            if (saveObject.playersBombs.Value != 0)
                dataLog.playersBombs = saveObject.playersBombs.Value;
            if (saveObject.playersBombHit.Value != 0)
                dataLog.playersBombHit = saveObject.playersBombHit.Value;
            if (saveObject.playersFuelSpent.Value != 0)
                dataLog.playersFuelSpent = saveObject.playersFuelSpent.Value;
            if (saveObject.playersFuelCharge.Value != 0)
                dataLog.playersFuelCharge = saveObject.playersFuelCharge.Value;
            if (saveObject.playersAmountDistance.Value != 0)
                dataLog.playersAmountDistance = saveObject.playersAmountDistance.Value;
            if (saveObject.playersClassicPath.Value != 0)
                dataLog.playersClassicPath = saveObject.playersClassicPath.Value;
            if (saveObject.playersMissionPath.Value != 0)
                dataLog.playersMissionPath = saveObject.playersMissionPath.Value;
            if (saveObject.playersCountPath.Value != 0)
                dataLog.playersCountPath = saveObject.playersCountPath.Value;
            if (saveObject.hitEnemiesResultsList.Value.Count != 0)
                dataLog.GetEnemyList = saveObject.hitEnemiesResultsList.Value;
        }
    }
}
