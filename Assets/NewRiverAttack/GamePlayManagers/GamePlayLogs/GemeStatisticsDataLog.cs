using System;
using System.Collections.Generic;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.LogManagers;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using UnityEngine;
using UnityEngine.Serialization;

namespace NewRiverAttack.GamePlayManagers.GamePlayLogs
{
    [CreateAssetMenu(fileName = "GemeStatisticsDataLog", menuName = "ImmersiveGames/GemeStatisticsDataLog", order = 2)]
    public class GemeStatisticsDataLog : SingletonScriptable<GemeStatisticsDataLog>
    {
        private GemeStatisticsDataLog()
        {
            SetResourcePath("SavesSO/GemeStatisticsDataLog");
        }

        [Header("Players Logs")] public int playerMaxScore;
        public float playerTimeSpent;
        public int playerMaxDistance;
        public int playerDeaths;

        [Header("Players Archive Stats")] public int playersDieWall;
        public int playersDieEnemyCollider;
        public int playersDieEnemyBullets;
        public int playersDieFuelOut;
        public int playersShoots;
        public int playersBombs;
        public float fuelSpent;
        public float fuelCharge;
        public float amountDistance;

        [Header("Distance Settings")]
        [Tooltip("Define quantas unidades de medida no eixo Z equivalem a 1 unidade de medida personalizada.")]
        public int baseConversion = 20;

        private List<LogPlayerResult> _listEnemyHit = new List<LogPlayerResult>();

        public string GetAmountDistance => $"{amountDistance:F2}";

        public List<LogPlayerResult> GetEnemiesResult()
        {
            return _listEnemyHit;
        }

        public int GetEnemiesHit(ObjectsScriptable enemy)
        {
            var item = _listEnemyHit.Find(x => x.enemy == enemy);
            DebugManager.Log<GemeStatisticsDataLog>($"Foram Encontrados {item?.quantity ?? 0} em {enemy.name}");
            return item?.quantity ?? 0;
        }

        public void RecoverLogPlayerResult(List<LogPlayerResult> resultsList)
        {
            _listEnemyHit = new List<LogPlayerResult>(resultsList);
        }

        public void ResetLogs()
        {
            playerMaxScore = 0;
            playerTimeSpent = 0;
            playerMaxDistance = 0;

            playersDieWall = 0;
            playersDieEnemyCollider = 0;
            playersDieEnemyBullets = 0;
            playersDieFuelOut = 0;

            playersShoots = 0;
            playersBombs = 0;
            fuelSpent = 0;
            fuelCharge = 0;
            amountDistance = 0;
        }

        public void IncrementStat(ref int stat, int value)
        {
            stat += value;
            DebugManager.Log<GemeStatisticsDataLog>($"Incremented stat to {stat}");
        }

        public void IncrementStat(ref float stat, float value)
        {
            stat += value;
            DebugManager.Log<GemeStatisticsDataLog>($"Incremented stat to {stat}");
        }

        public void SetAmountDistance(float amount)
        {
            amountDistance = amount / baseConversion;
            DebugManager.Log<GemeStatisticsDataLog>($"Distance converted to custom unit: {amountDistance:F2}");
        }
    }
}