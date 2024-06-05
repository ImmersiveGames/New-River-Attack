using ImmersiveGames.DebugManagers;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.SteamGameManagers;
using NewRiverAttack.WallsManagers;
using UnityEngine;
using Bullets = ImmersiveGames.BulletsManagers.Bullets;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerAchievements: MonoBehaviour
    {
        private const int DistanceToCheckAchievement = 100;
        private const int BombToHitAchievement = 3;
        private bool _connectSteam;
        private PlayerMaster _playerMaster;
        private void Start()
        {
            _playerMaster = GetComponent<PlayerMaster>();
            _connectSteam = SteamGameManager.ConnectedToSteam;
            if (!_connectSteam)
            {
                DebugManager.Log<PlayerAchievements>($"Steam not online");
            }
        }

        private void OnDisable()
        {
            LogPlayerScore(_playerMaster.GetPlayerScore, _playerMaster.PlayerIndex);
        }

        private void LogPlayerScore(int valueUpdate, int playerIndex)
        {
            DebugManager.Log<PlayerDistance>($"Registrar Score: {valueUpdate}");
            if (!_connectSteam) return;
            SteamGameManager.UpdateScore(valueUpdate, false);
            SteamGameManager.StoreStats();
        }

        protected internal void LogHitsBomb(int number)
        {
            if (number >= BombToHitAchievement)
            {
                SteamGameManager.UnlockAchievement("ACH_HIT_BOMB_3");
                SteamGameManager.StoreStats();
            }
        }

        protected internal void LogDistanceReach(int distance)
        {
            GamePlayLog.instance.amountDistance += distance;
            if (distance <= 0 || distance % DistanceToCheckAchievement != 0) return;
            DebugManager.Log<PlayerDistance>($"Distance Maxima para Registrar: {distance}");
            
            if (!_connectSteam) return;
            SteamGameManager.AddStat("stat_FlightDistance", distance, true);
            SteamGameManager.StoreStats();
        }
        

        protected internal void LogOutFuel()
        {
            DebugManager.Log<PlayerAchievements>( $"FUEL OUT WAS UNLOCKED!" );
            if (!_connectSteam) return;
            SteamGameManager.UnlockAchievement("ACH_DIE_PLAYER_GAS");
            SteamGameManager.StoreStats();
        }

        protected internal void LogSpendFuel(float fuel)
        {
            var spendFuel = (int)fuel;
            GamePlayLog.instance.fuelSpent += spendFuel;
            DebugManager.Log<PlayerAchievements>( $"Registrou gasto de gasolina: {spendFuel} No total: {GamePlayLog.instance.fuelSpent}" );
            if (!_connectSteam) return;
            SteamGameManager.AddStat("stat_SpendGas", spendFuel, false);
            SteamGameManager.StoreStats();
        }

        protected internal void LogCollectible(Component component)
        {
            DebugManager.Log<PlayerAchievements>($"Registrado Collecionavel");
        }
        protected internal void LogCollision(Component component)
        {
            var walls = component.GetComponentInParent<WallMaster>();
            var enemies = component.GetComponentInParent<ObstacleMaster>();
            var bullets = component.GetComponentInParent<Bullets>();

            if (walls != null)
            {
                // Colidiu com uma parede.
                GamePlayLog.instance.playersDieWall += 1;
                DebugManager.Log<PlayerAchievements>($"Registrado +1 colisão na Parede #{GamePlayLog.instance.playersDieWall}");
                if (_connectSteam)
                {
                    SteamGameManager.AddStat("stat_CrashPlayer",1,false);
                    SteamGameManager.UnlockAchievement("ACH_CRASH_PLAYER_WALL"); 
                }
            }
            if (bullets != null)
            {
                // Colidiu com uma bala inimiga.
                GamePlayLog.instance.playersDieEnemyBullets += 1;
                DebugManager.Log<PlayerAchievements>($"Registrado +1 colisão por Bullets #{GamePlayLog.instance.playersDieEnemyBullets}");
            }
            if (enemies != null && enemies is not EnemiesMaster)
            { 
                DebugManager.Log<PlayerAchievements>($"Registrado +1 colisão NÃo Inimiga");
                //Colidiu e algo que não é um inimigo
                //GamePlayManager.AddResultList(_gamePlayingLog.GetEnemiesResult(), getPlayerSettings, enemies.enemy, 1, CollisionType.Collected);
            }
            else if (enemies != null && enemies is EnemiesMaster)
            {
                DebugManager.Log<PlayerAchievements>($"Registrado +1 colisão NÃo Inimiga");
                //SteamGameManager.AddStat("stat_CrashPlayer",1,false);
                //GamePlayManager.AddResultList(_gamePlayingLog.GetEnemiesResult(), getPlayerSettings, enemies.enemy, 1, CollisionType.Collider);
            }

            if (!_connectSteam) return;
            SteamGameManager.StoreStats();
            DebugManager.Log<PlayerAchievements>($"Registrados Feitos com sucesso");
        }
    }
}