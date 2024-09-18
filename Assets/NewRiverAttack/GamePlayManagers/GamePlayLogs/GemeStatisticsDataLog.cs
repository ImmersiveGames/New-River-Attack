using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using NewRiverAttack.PlayerManagers.ScriptableObjects;
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

        [Header("Players Logs")] 
        public int playersMaxScore;
        public float playersTimeSpent;
        public int playersMaxDistance;
        public int playersDeaths;
        public int playersDieWall;
        public int playersDieEnemyCollider;
        public int playersDieEnemyBullets;
        public int playersDieFuelOut;
        public int playersShoots;
        public int playersBombs;
        public float playersTimeRapidFire;
        public int playersBombHit;
        public float playersFuelSpent; 
        public float playersFuelCharge;
        public float playersAmountDistance;
        public int playersClassicPath;
        public int playersMissionPath;
        public int playersCountPath;
        internal const int BaseConversion = 20;

        public List<GameStatisticHit> GetEnemyList
        {
            get => listEnemyHit;
            set
            {
                listEnemyHit = new List<GameStatisticHit>();
                listEnemyHit = value;
            }
        }
        

        [SerializeField] private List<GameStatisticHit> listEnemyHit = new List<GameStatisticHit>();
        public string GetAmountDistance => $"{playersAmountDistance:F2}";

        #region List Auxiliares

        // 1. Retornar todos os registros com o mesmo jogador
        public IEnumerable<GameStatisticHit> GetHitsByPlayer(PlayerSettings player)
        {
            return listEnemyHit
                .Where(hit => hit.player == player)
                .GroupBy(hit => hit.enemy.localizeName)
                .Select(group => new GameStatisticHit(player, group.First().enemy, group.Sum(hit => hit.quantity), group.First().collisionType))
                .OrderBy(hit => hit.enemy.localizeName.GetLocalizedString())
                .ToList();
        }
        // 2. Retornar todos os registros com o mesmo jogador e inimigo
        public IEnumerable<GameStatisticHit> GetHitsByPlayerAndEnemy(PlayerSettings player, ObjectsScriptable enemy)
        {
            return listEnemyHit
                .Where(hit => hit.player == player && hit.enemy == enemy)
                .GroupBy(hit => hit.enemy.localizeName)
                .Select(group => new GameStatisticHit(player, group.First().enemy, group.Sum(hit => hit.quantity), group.First().collisionType))
                .OrderBy(hit => hit.enemy.localizeName.GetLocalizedString())
                .ToList();
        }
        // 3. Retornar todos os registros com o mesmo jogador e tipo de colisão
        public List<GameStatisticHit> GetHitsByPlayerAndCollision(PlayerSettings player, EnumCollisionType collision)
        {
            return listEnemyHit
                .Where(hit => hit.player == player && hit.collisionType == collision)
                .GroupBy(hit => hit.enemy.localizeName)
                .Select(group => new GameStatisticHit(player, group.First().enemy, group.Sum(hit => hit.quantity), collision))
                .OrderBy(hit => hit.enemy.localizeName.GetLocalizedString())
                .ToList();
        }
        // 4. Adicionar novo registro ou somar quantidade ao existente
        public void AddOrUpdateStatisticHit(PlayerSettings player, ObjectsScriptable enemy, int quantity, EnumCollisionType collision)
        {
            var existingHit = listEnemyHit
                .FirstOrDefault(hit => hit.player == player && hit.enemy == enemy && hit.collisionType == collision);

            if (existingHit != null)
            {
                existingHit.quantity += quantity;
            }
            else
            {
                listEnemyHit.Add(new GameStatisticHit(player, enemy, quantity, collision));
            }
        }
        public int GetTotalQuantityByPlayerAndEnemy(PlayerSettings player, ObjectsScriptable enemy)
        {
            return listEnemyHit
                .Where(hit => hit.player == player && hit.enemy.localizeName == enemy.localizeName)
                .Sum(hit => hit.quantity);
        }
        public int GetTotalQuantityByPlayerAndType(ObstacleTypes types)
        {
            if (listEnemyHit != null)
            {
                return listEnemyHit
                    .Where(hit => hit.enemy.obstacleTypes == types)
                    .Sum(hit => hit.quantity);
            }

            return 0;
        }
        public Dictionary<ObjectsScriptable, int> GetTotalQuantityByObstacleTypeSortedByName(PlayerSettings player)
        {
            return listEnemyHit
                .Where(hit => hit.player == player) // Filtra por jogador
                .GroupBy(hit => hit.enemy) // Agrupa por inimigo (ObjectsScriptable)
                .Select(group => new
                {
                    Enemy = group.Key,
                    TotalQuantity = group.Sum(hit => hit.quantity),
                    LocalizedName = group.Key.localizeName.GetLocalizedString()
                })
                .OrderBy(item => item.LocalizedName) // Ordena pelo nome localizado
                .ToDictionary(
                    item => item.Enemy,
                    item => item.TotalQuantity
                );
        }


        public Dictionary<ObjectsScriptable, int> GetTotalQuantityByObstacleTypeSortedByQuantity(PlayerSettings player)
        {
            return listEnemyHit
                .Where(hit => hit.player == player) // Filtra por jogador
                .GroupBy(hit => hit.enemy) // Agrupa por inimigo (ObjectsScriptable)
                .Select(group => new
                {
                    Enemy = group.Key,
                    TotalQuantity = group.Sum(hit => hit.quantity)
                })
                .OrderByDescending(item => item.TotalQuantity) // Ordena pela quantidade total em ordem decrescente
                .ToDictionary(
                    item => item.Enemy,
                    item => item.TotalQuantity
                );
        }
        #endregion

        public void ResetLogs()
        {
            playersMaxScore = 0;
            playersTimeSpent = 0;
            playersMaxDistance = 0;

            playersDieWall = 0;
            playersDieEnemyCollider = 0;
            playersDieEnemyBullets = 0;
            playersDieFuelOut = 0;

            playersShoots = 0;
            playersBombs = 0;
            playersFuelSpent = 0;
            playersFuelCharge = 0;
            playersAmountDistance = 0;
            playersCountPath = 0;
            listEnemyHit = new List<GameStatisticHit>();
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
            playersAmountDistance = amount / BaseConversion;
            DebugManager.Log<GemeStatisticsDataLog>($"Distance converted to custom unit: {playersAmountDistance:F2}");
        }

        public int GetCrashes => playersDieWall + playersDieEnemyCollider + playersDieFuelOut;
    }
}