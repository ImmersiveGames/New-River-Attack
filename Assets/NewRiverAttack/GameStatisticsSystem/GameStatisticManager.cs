using System;
using System.Linq;
using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.SteamServicesManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using NewRiverAttack.LevelBuilder;
using NewRiverAttack.ObstaclesSystems;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems.PowerUpSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
using NewRiverAttack.WallsManagers;
using UnityEngine;

namespace NewRiverAttack.GameStatisticsSystem
{
    public sealed class GameStatisticManager : Singleton<GameStatisticManager>
    {
        private float _startTimer;
        private const float Epsilon = 0.1f;
        private GemeStatisticsDataLog _gemeStatisticsDataLog;
        private SteamAchievementService _steamAchievementService;

        public delegate void GameLogIntHandler(string stateName, int intValue);
        public event GameLogIntHandler EventServiceUpdateInt;

        public delegate void GameLogFloatHandler(string stateName, float floatValue);
        public event GameLogFloatHandler EventServiceUpdateFloat;

        protected override void Awake()
        {
            _startTimer = Time.time;
            base.Awake();
        }

        private void Start()
        {
            _steamAchievementService = SteamAchievementService.Instance;
            _gemeStatisticsDataLog = GemeStatisticsDataLog.Instance;
        }

        private void OnApplicationQuit()
        {
            LogSessionTime(_startTimer);
        }

        private void LogSessionTime(float sessionStartTime)
        {
            if (sessionStartTime <= 0) return;
            var sessionTime = Time.time - sessionStartTime;
            if (!_gemeStatisticsDataLog) return;
            _gemeStatisticsDataLog.playersTimeSpent += sessionTime;
            //DebugManager.Log<GameStatisticManager>($"Log Offline Timer {_gemeStatisticsDataLog.playersTimeSpent}");
        }

        internal async void LogMaxScore(int score)
        {
            if(_gemeStatisticsDataLog == null || score <= 0) return;
            if (score <= _gemeStatisticsDataLog.playersMaxScore) return;
            _gemeStatisticsDataLog.playersMaxScore = score;
            await SteamLeaderboardService.Instance.UpdateScore(score, true).ConfigureAwait(false);
            //DebugManager.Log<GameStatisticManager>($"Log Offline Max Score {_gemeStatisticsDataLog.playersMaxScore}");
        }

        public void LogAmountDistance(float amount)
        {
            if(_gemeStatisticsDataLog == null || amount <= 0) return;
            _gemeStatisticsDataLog.SetAmountDistance(amount);
        }

        internal void LogDistance(int distance)
        {
            if(distance <= 0) return;
            LogMaxDistance(distance);
            if(_gemeStatisticsDataLog == null) return;
            CheckAmountUpdate("stat_FlightDistance", _gemeStatisticsDataLog.playersAmountDistance, 500);
        }

        private void LogMaxDistance(int intValue)
        {
            if(_gemeStatisticsDataLog == null || intValue <= 0) return;
            _gemeStatisticsDataLog.playersMaxDistance = Mathf.Max(_gemeStatisticsDataLog.playersMaxDistance, intValue);
            //DebugManager.Log<GameStatisticManager>($"Log Offline MAX Distance: {_gemeStatisticsDataLog.playersMaxDistance}");
        }

        public void LogShoots(int intValue)
        {
            if(_gemeStatisticsDataLog == null || intValue <= 0) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersShoots, intValue);
            CheckAmountUpdate("stat_Shoots", _gemeStatisticsDataLog.playersShoots);
        }

        public void LogBombs(int intValue)
        {
            if(_gemeStatisticsDataLog == null || intValue <= 0) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersBombs, intValue);
            CheckAmountUpdate("stat_Bombs", _gemeStatisticsDataLog.playersBombs);
        }
        public void LogFuelSpend(float floatValue)
        {
            if(_gemeStatisticsDataLog == null || floatValue <= 0) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersFuelSpent, floatValue);
            CheckAmountUpdate("stat_SpendGas", _gemeStatisticsDataLog.playersFuelSpent,1000);
        }
        public void LogFuelCharge(float floatValue)
        {
            if(_gemeStatisticsDataLog == null || floatValue <= 0) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersFuelCharge, floatValue);
            CheckAmountUpdate("stat_FillGas", _gemeStatisticsDataLog.playersFuelCharge);
        }
        public void LogDeaths(int intValue)
        {
            if(_gemeStatisticsDataLog == null || intValue <= 0) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersDeaths, intValue);
            OnEventServiceSet("stat_Deaths", _gemeStatisticsDataLog.playersDeaths);
        }

        public void LogCollision(Component other)
        {
            if(_gemeStatisticsDataLog == null || other ==null) return;
            // Testa se o componente é do tipo WallMaster
            if (other.GetComponentInParent<WallMaster>())
            {
                _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersDieWall, 1);
                OnEventServiceSet("stat_WallDeaths", _gemeStatisticsDataLog.playersDieWall);
                _steamAchievementService.UnlockAchievement("ACH_CRASH_PLAYER_WALL");
            }
            // Testa se o componente é do tipo ObstacleMaster
            else if (other.GetComponentInParent<ObstacleMaster>())
            {
                _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersDieEnemyCollider, 1);
                OnEventServiceSet("stat_ObstacleDeaths", _gemeStatisticsDataLog.playersDieEnemyCollider);
            }
            // Testa se o componente é do tipo Bullets
            else if (other.GetComponentInParent<Bullets>())
            {
                _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersDieEnemyBullets, 1);
                OnEventServiceSet("stat_BulletsDeaths", _gemeStatisticsDataLog.playersDieEnemyBullets);
            }
            CheckAmountUpdate("stat_CrashPlayer", _gemeStatisticsDataLog.GetCrashes, 50);
        }
        
        public void LogFuelOut(int intValue)
        {
            if(_gemeStatisticsDataLog == null || intValue <= 0) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersDieFuelOut, intValue);
            OnEventServiceSet("stat_FuelOut", _gemeStatisticsDataLog.playersDieFuelOut);
        }
        public void LogCompletePath(int intValue, GamePlayModes modes)
        {
            if(_gemeStatisticsDataLog == null || intValue <= 0) return;
            switch (modes)
            {
                case GamePlayModes.ClassicMode:
                    _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersClassicPath, intValue);
                    OnEventServiceSet("stat_FinishCPath", _gemeStatisticsDataLog.playersClassicPath);
                    break;
                case GamePlayModes.MissionMode:
                    _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersMissionPath, intValue);
                    OnEventServiceSet("stat_FinishMPath", _gemeStatisticsDataLog.playersMissionPath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modes), modes, null);
            }
        }
        public void LogTimeRapidFire(float floatValue)
        {
            if(_gemeStatisticsDataLog == null || floatValue <= 0) return;
            _gemeStatisticsDataLog.IncrementStat(ref _gemeStatisticsDataLog.playersTimeRapidFire, floatValue);
            CheckAmountUpdate("stat_TimeRapidFire", _gemeStatisticsDataLog.playersTimeRapidFire);
        }
        
        public void LogBombsHit(int intValue)
        {
            if(_gemeStatisticsDataLog == null || intValue <= 0) return;
            _gemeStatisticsDataLog.playersBombHit = Mathf.Max(_gemeStatisticsDataLog.playersBombHit, intValue);
            OnEventServiceSet("stat_BombHit", _gemeStatisticsDataLog.playersBombHit);
        }
        
        internal void LogCollectables(ObjectsScriptable objects)
        {
            if(_gemeStatisticsDataLog == null || objects == null) return;
            var powerUp = objects as PowerUpScriptable;
            if (powerUp != null && powerUp.powerUpData.powerUpType == PowerUpTypes.Bomb)
            {
                _steamAchievementService.UnlockAchievement("ACH_COLLECT_BOMB");
            }
            if (powerUp != null && powerUp.powerUpData.powerUpType == PowerUpTypes.RapidFire)
            {
                _steamAchievementService.UnlockAchievement("ACH_COLLECT_RAPID_FIRE");
            }
            if (powerUp != null && powerUp.powerUpData.powerUpType == PowerUpTypes.Live)
            {
                _steamAchievementService.UnlockAchievement("ACH_COLLECT_LIFE");
            }
            if (powerUp == null && objects.obstacleTypes == ObstacleTypes.Refugee)
            {
                CheckAmountUpdateType("stat_CollectRefugee", ObstacleTypes.Refugee); 
            }
            
        }

        public void LogEnemiesHit(PlayerSettings player, ObjectsScriptable enemy, int quantity, EnumCollisionType collision)
        {
            if(_gemeStatisticsDataLog == null || enemy == null) return;
            _gemeStatisticsDataLog.AddOrUpdateStatisticHit(player,enemy, quantity,collision);

            switch (enemy.obstacleTypes)
            {
                case ObstacleTypes.AirCraft:
                    CheckAmountUpdateType("stat_BeatJet", ObstacleTypes.AirCraft);
                    break;
                case ObstacleTypes.Ship:
                    CheckAmountUpdateType("stat_BeatShip", ObstacleTypes.Ship);
                    if(collision == EnumCollisionType.Collider)
                        _steamAchievementService.UnlockAchievement("ACH_CRASH_PLAYER_SHIP");
                    break;
                case ObstacleTypes.Helicopter:
                    CheckAmountUpdateType("stat_BeatHeli", ObstacleTypes.Helicopter);
                    if(collision == EnumCollisionType.Collider)
                        _steamAchievementService.UnlockAchievement("ACH_CRASH_PLAYER_HELI");
                    break;
                case ObstacleTypes.Hovercraft:
                    CheckAmountUpdateType("stat_BeatHover", ObstacleTypes.Hovercraft);
                    break;
                case ObstacleTypes.Drone:
                    CheckAmountUpdateType("stat_BeatDrones", ObstacleTypes.Drone);
                    if(collision == EnumCollisionType.Collider)
                        _steamAchievementService.UnlockAchievement("ACH_CRASH_PLAYER_DRONE");
                    break;
                case ObstacleTypes.Tower:
                    CheckAmountUpdateType("stat_BeatTower", ObstacleTypes.Tower);
                    break;
                case ObstacleTypes.Rock:
                    CheckAmountUpdateType("stat_BeatRock", ObstacleTypes.Rock);
                    break;
                case ObstacleTypes.Tanks:
                    CheckAmountUpdateType("stat_BeatTanks", ObstacleTypes.Tanks);
                    break;
                case ObstacleTypes.Bridges:
                    CheckAmountUpdateType("stat_BeatBridge", ObstacleTypes.Bridges);
                    if(collision == EnumCollisionType.Collider)
                        _steamAchievementService.UnlockAchievement("ACH_CRASH_PLAYER_BRIDGE");
                    break;
                case ObstacleTypes.Submarine:
                    CheckAmountUpdateType("stat_BeatSubmarine", ObstacleTypes.Submarine);
                    _steamAchievementService.UnlockAchievement("ACH_BEAT_SUBMARINE");
                    break;
                case ObstacleTypes.GasStation:
                    CheckAmountUpdateType("stat_BeatGasStation", ObstacleTypes.GasStation);
                    break;
                case ObstacleTypes.Refugee:
                    CheckAmountUpdateType("stat_BeatRefugee", ObstacleTypes.Refugee);
                    break;
                case ObstacleTypes.Mine:
                    CheckAmountUpdateType("stat_BeatMine", ObstacleTypes.Mine);
                    break;
                case ObstacleTypes.Collectable:
                    CheckAmountUpdateType("stat_BeatCollectable", ObstacleTypes.Collectable);
                    break;
                case ObstacleTypes.Decoration:
                    CheckAmountUpdateType("stat_BeatDecoration", ObstacleTypes.Decoration);
                    break;
                case ObstacleTypes.Secret:
                    CheckAmountUpdateType("stat_BeatSecret", ObstacleTypes.Secret);
                    _steamAchievementService.UnlockAchievement("ACH_FIND_SECRET");
                    break;
                case ObstacleTypes.Others:
                default:
                    CheckAmountUpdateType("stat_BeatOthers", ObstacleTypes.Others);
                    break;
            }
        }
        public void LogSegmentActions(ScenarioObjectData? activeSegment)
        {
            if(activeSegment == null) return;
            OnEventServiceSet("stat_FinishCPath", _gemeStatisticsDataLog.playersCountPath);
            switch (activeSegment.Value.levelType)
            {
                case LevelTypes.Grass:
                    _steamAchievementService.UnlockAchievement("ACH_FINISH_M_GRASS");
                    break;
                case LevelTypes.Forest:
                    _steamAchievementService.UnlockAchievement("ACH_FINISH_M_FOREST");
                    break;
                case LevelTypes.Swamp:
                    _steamAchievementService.UnlockAchievement("ACH_FINISH_M_SWAMP");
                    break;
                case LevelTypes.Antique:
                    _steamAchievementService.UnlockAchievement("ACH_FINISH_M_ANTIQUE");
                    break;
                case LevelTypes.Desert:
                    _steamAchievementService.UnlockAchievement("ACH_FINISH_M_DESERT");
                    break;
                case LevelTypes.Ice:
                    _steamAchievementService.UnlockAchievement("ACH_FINISH_M_ICE");
                    break;
                case LevelTypes.Boss:
                    _steamAchievementService.UnlockAchievement("ACH_BEAT_SUBMARINE");
                    break;
                case LevelTypes.Multi:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal void OnEventServiceUpdate()
        {
            if(_gemeStatisticsDataLog == null) return;
            DebugManager.Log<GameStatisticManager>($"Service Update");
            OnEventServiceSet("highScore", _gemeStatisticsDataLog.playersMaxScore);
            OnEventServiceSet("stat_FlightDistance", _gemeStatisticsDataLog.playersAmountDistance);
            OnEventServiceSet("stat_Shoots", _gemeStatisticsDataLog.playersShoots);
            OnEventServiceSet("stat_Bombs", _gemeStatisticsDataLog.playersBombs);
            OnEventServiceSet("stat_SpendGas", _gemeStatisticsDataLog.playersFuelSpent);
            OnEventServiceSet("stat_FillGas", _gemeStatisticsDataLog.playersFuelCharge);
            OnEventServiceSet("stat_FinishCPath", _gemeStatisticsDataLog.playersCountPath);
            OnEventServiceSet("stat_CrashPlayer", _gemeStatisticsDataLog.GetCrashes);
            OnEventServiceSet("stat_CollectRefugee", _gemeStatisticsDataLog.GetTotalQuantityByPlayerAndType(ObstacleTypes.Refugee));
        }
        
        #region Auxiliar
        private void CheckAmountUpdateType(string statName, ObstacleTypes obstacleTypes, int amount = 100)
        {
            // Verifica se _gemeStatisticsDataLog está inicializado
            if (_gemeStatisticsDataLog == null)
            {
                DebugManager.LogError<GameStatisticManager>("_gemeStatisticsDataLog está null. Abandonando a execução.");
                return;
            }

            // Verifica se listEnemyHit dentro de _gemeStatisticsDataLog está inicializado
            if (_gemeStatisticsDataLog.GetEnemyList == null || !_gemeStatisticsDataLog.GetEnemyList.Any())
            {
                DebugManager.LogWarning<GameStatisticManager>("listEnemyHit está null ou vazia.");
                return;
            }

            // Obtém a quantidade total de obstáculos do tipo fornecido
            var obstacles = _gemeStatisticsDataLog.GetTotalQuantityByPlayerAndType(obstacleTypes);

            // Verifica se a quantidade de obstáculos retornada é válida
            if (obstacles <= 0)
            {
                DebugManager.LogWarning<GameStatisticManager>("Nenhum obstáculo encontrado para o tipo especificado.");
                return;
            }

            // Chama CheckAmountUpdate com a quantidade de obstáculos obtida
            CheckAmountUpdate(statName, obstacles, amount);
            
        }


        private void CheckAmountUpdate(string statName,int quantity, int amount = 100)
        {
            if (quantity % amount == 0)
            {
                OnEventServiceSet(statName, quantity);
            }
        }
        private void CheckAmountUpdate(string statName,float quantity, float amount = 100)
        {
            if (Mathf.Abs(quantity % amount) < Epsilon || Mathf.Abs(amount - (quantity % amount)) < Epsilon)
            {
                OnEventServiceSet(statName, quantity);
            }
        }
        #endregion

        private void OnEventServiceSet(string stateName, int intValue)
        {
            EventServiceUpdateInt?.Invoke(stateName, intValue);
        }

        private void OnEventServiceSet(string stateName, float floatValue)
        {
            EventServiceUpdateFloat?.Invoke(stateName, floatValue);
        }
    }
}
