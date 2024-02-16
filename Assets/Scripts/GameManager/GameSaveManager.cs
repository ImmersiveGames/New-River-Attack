using CarterGames.Assets.SaveManager;
using Save;
using Utils;
using UnityEngine;
namespace RiverAttack
{
    public class GameSaveManager : Singleton<GameSaveManager>
    {
        [SerializeField] private PlayerSettings playerSettings;
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private GamePlayingLog gamePlayingLog;
        private PlayerSaveSaveObject m_PlayerSave;

        private void Start()
        {
            m_PlayerSave = SaveManager.GetSaveObject<PlayerSaveSaveObject>();
            LoadPlayerSaves();
        }

        private void OnDisable()
        {
            SavePlayerSaves();
        }

        protected override void OnDestroy()
        {
            //base.OnDestroy();
        }

        private void LoadPlayerSaves()
        {
            SaveManager.Load();
            //m_PlayerSave.Load();
            playerSettings.listProducts = m_PlayerSave.listProducts.Value;
            playerSettings.wealth = m_PlayerSave.wealth.Value;

            gameSettings.musicVolume = m_PlayerSave.musicVolume.Value;
            gameSettings.sfxVolume = m_PlayerSave.sfxVolume.Value;
            gameSettings.startLocale = m_PlayerSave.startLocale.Value;
            gameSettings.indexQuality = m_PlayerSave.indexQuality.Value;
            gameSettings.actualResolution = m_PlayerSave.actualResolution.Value;
            gameSettings.indexFrameRate = m_PlayerSave.indexFrameRate.Value;
            gameSettings.indexResolution = m_PlayerSave.indexResolution.Value;

            gamePlayingLog.pathDistance = m_PlayerSave.pathDistance.Value;
            gamePlayingLog.maxPathDistance = m_PlayerSave.maxPathDistance.Value;
            gamePlayingLog.shootSpent = m_PlayerSave.shootSpent.Value;
            gamePlayingLog.livesSpent = m_PlayerSave.livesSpent.Value;
            gamePlayingLog.fuelSpent = m_PlayerSave.fuelSpent.Value;
            gamePlayingLog.fuelStocked = m_PlayerSave.fuelStocked.Value;
            gamePlayingLog.bombSpent = m_PlayerSave.bombSpent.Value;
            gamePlayingLog.totalScore = m_PlayerSave.totalScore.Value;
            gamePlayingLog.timeSpent = m_PlayerSave.timeSpent.Value;
            gamePlayingLog.playerDieWall = m_PlayerSave.playerDieWall.Value;
            gamePlayingLog.playerDieBullet = m_PlayerSave.playerDieBullet.Value;
            gamePlayingLog.playerDieFuelEmpty = m_PlayerSave.playerDieFuelEmpty.Value;
            gamePlayingLog.activeMission = m_PlayerSave.activeMission.Value;
            gamePlayingLog.LevelRecover(m_PlayerSave.finishLevels.Value);
            gamePlayingLog.ResultRecover(m_PlayerSave.hitEnemiesResultsList.Value);
        }

        public void SavePlayerSaves()
        {
            m_PlayerSave.listProducts.Value = playerSettings.listProducts;
            m_PlayerSave.wealth.Value = playerSettings.wealth;
            m_PlayerSave.musicVolume.Value = gameSettings.musicVolume;
            m_PlayerSave.sfxVolume.Value = gameSettings.sfxVolume;
            m_PlayerSave.startLocale.Value = gameSettings.startLocale;
            m_PlayerSave.indexQuality.Value = gameSettings.indexQuality;
            m_PlayerSave.actualResolution.Value = gameSettings.actualResolution;
            m_PlayerSave.indexFrameRate.Value = gameSettings.indexFrameRate;
            m_PlayerSave.indexResolution.Value = gameSettings.indexResolution;

            m_PlayerSave.pathDistance.Value = gamePlayingLog.pathDistance;
            m_PlayerSave.maxPathDistance.Value = gamePlayingLog.maxPathDistance;
            m_PlayerSave.shootSpent.Value = gamePlayingLog.shootSpent;
            m_PlayerSave.livesSpent.Value = gamePlayingLog.livesSpent;
            m_PlayerSave.fuelSpent.Value = gamePlayingLog.fuelSpent;
            m_PlayerSave.fuelStocked.Value = gamePlayingLog.fuelStocked;
            m_PlayerSave.bombSpent.Value = gamePlayingLog.bombSpent;
            m_PlayerSave.totalScore.Value = gamePlayingLog.totalScore;
            m_PlayerSave.timeSpent.Value = gamePlayingLog.timeSpent;
            m_PlayerSave.playerDieWall.Value = gamePlayingLog.playerDieWall;
            m_PlayerSave.playerDieBullet.Value = gamePlayingLog.playerDieBullet;
            m_PlayerSave.playerDieFuelEmpty.Value = gamePlayingLog.playerDieFuelEmpty;
            m_PlayerSave.activeMission.Value = gamePlayingLog.activeMission;
            m_PlayerSave.finishLevels.Value = gamePlayingLog.GetLevelsResult();
            m_PlayerSave.hitEnemiesResultsList.Value = gamePlayingLog.GetEnemiesResult();

            //m_PlayerSave.Save();
            SaveManager.Save();
        }

        private void ClearPlayerSave()
        {
            m_PlayerSave.ResetObjectSaveValues();
            SaveManager.Save();
        }

    }
}
