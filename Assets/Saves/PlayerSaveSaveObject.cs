using System.Collections.Generic;
using CarterGames.Assets.SaveManager;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using NewRiverAttack.LevelBuilder;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace Saves
{
    [CreateAssetMenu(fileName = "PlayerSaveSaveObject")]
    public class PlayerSaveSaveObject : SaveObject
    {
        public SaveValue<int> wallet;

        public SaveValue<float> bgmVolume;
        public SaveValue<float> sfxVolume;
        public SaveValue<Locale> startLocale;
        public SaveValue<int> selectedQualityIndex;
        public SaveValue<Vector2Int> actualResolution;
        public SaveValue<int> frameRate;
        
        public SaveValue<List<ProductStock>> listPlayerProductStocks;
        public SaveValue<PlayerSettings[]> playerSettings;
        
        public SaveValue<int> activeIndexMissionLevel;
        public SaveValue<int> missionLives;
        public SaveValue<int> missionBombs;
        
        //Datalog
        public SaveValue<int> playersMaxScore;
        public SaveValue<float> playersTimeSpent;
        public SaveValue<int> playersMaxDistance;
        public SaveValue<int> playersDeaths;
        public SaveValue<int> playersDieWall;
        public SaveValue<int> playersDieEnemyCollider;
        public SaveValue<int> playersDieEnemyBullets;
        public SaveValue<int> playersDieFuelOut;
        public SaveValue<int> playersShoots;
        public SaveValue<int> playersBombs;
        public SaveValue<float> playersTimeRapidFire;
        public SaveValue<int> playersBombHit;
        public SaveValue<float> playersFuelSpent;
        public SaveValue<float> playersFuelCharge;
        public SaveValue<float> playersAmountDistance;
        public SaveValue<int> playersClassicPath;
        public SaveValue<int> playersMissionPath;
        
        public SaveValue<List<GameStatisticHit>> hitEnemiesResultsList;

    }
}