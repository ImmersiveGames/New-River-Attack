using System.Collections.Generic;
using CarterGames.Assets.SaveManager;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using NewRiverAttack.LevelBuilder;
using UnityEngine;
using UnityEngine.Localization;

namespace Saves
{
    [CreateAssetMenu(fileName = "PlayerSaveSaveObject")]
    public class PlayerSaveSaveObject : SaveObject
    {
        //public SaveValue<List<ShopProduct>> listProducts;
        public SaveValue<int> wealth;

        public SaveValue<float> musicVolume;
        public SaveValue<float> sfxVolume;
        public SaveValue<Locale> startLocale;
        public SaveValue<int> indexQuality;
        public SaveValue<Vector2Int> actualResolution;
        public SaveValue<int> indexFrameRate;
        public SaveValue<int> indexResolution;
        
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
        public SaveValue<LevelData> activeMission;
        public SaveValue<List<LevelData>> finishLevels;
        public SaveValue<List<GameStatisticHit>> hitEnemiesResultsList;

    }
}