using System.Collections.Generic;
using CarterGames.Assets.SaveManager;
using UnityEngine;
using RiverAttack;
using Shopping;
using UnityEngine.Localization;

namespace Save
{
    [CreateAssetMenu(fileName = "PlayerSaveSaveObject")]
    public class PlayerSaveSaveObject : SaveObject
    {
        #region PlayerSettingsSave
        public SaveValue<List<ShopProduct>> listProducts;
        #endregion

        #region GameSettingsSave
        public SaveValue<float> musicVolume;
        public SaveValue<float> sfxVolume;
        public SaveValue<Locale> startLocale;
        public SaveValue<int> actualQuality;
        public SaveValue<int> actualResolutionWidth;      
        public SaveValue<int> actualResolutionHeight; 
        #endregion

        #region GamePlayLog
        public SaveValue<bool> myBoolSaveValue;
        public SaveValue<float> pathDistance;
        public SaveValue<float> maxPathDistance;
        public SaveValue<float> shootSpent;
        public SaveValue<int> livesSpent;
        public SaveValue<int> fuelSpent;
        public SaveValue<int> fuelStocked;
        public SaveValue<int> bombSpent;
        public SaveValue<int> totalScore;
        public SaveValue<float> timeSpent;
        public SaveValue<int> playerDieWall;
        public SaveValue<int> playerDieBullet;
        public SaveValue<int> playerDieFuelEmpty;
        public SaveValue<Levels> activeMission;
        public SaveValue<List<Levels>> finishLevels;
        public SaveValue<List<LogResults>> hitEnemiesResultsList;
        #endregion

    }
}