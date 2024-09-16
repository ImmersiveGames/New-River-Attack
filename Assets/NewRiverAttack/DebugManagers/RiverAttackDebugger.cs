using ImmersiveGames;
using ImmersiveGames.AudioEvents;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.MenuManagers;
using ImmersiveGames.MenuManagers.Abstracts;
using ImmersiveGames.MenuManagers.PanelGameManagers;
using ImmersiveGames.MenuManagers.PanelOptionsManagers;
using ImmersiveGames.PoolManagers;
using ImmersiveGames.ShopManagers.NavigationModes;
using ImmersiveGames.StateManagers;
using ImmersiveGames.SteamServicesManagers;
using NewRiverAttack.BulletsManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.GameStatisticsSystem;
using NewRiverAttack.HUBManagers;
using NewRiverAttack.HUDManagers.UI;
using NewRiverAttack.LevelBuilder;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.BossSystems;
using NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours;
using NewRiverAttack.ObstaclesSystems.BridgeSystems;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using NewRiverAttack.SaveManagers;
using NewRiverAttack.ShoppingSystems.SimpleShopping;
using NewRiverAttack.ShoppingSystems.SimpleShopping.Abstracts;
using NewRiverAttack.ShoppingSystems.SkinChanger;
using NewRiverAttack.StateManagers.States;
using UnityEngine;

namespace NewRiverAttack.DebugManagers
{
    public class RiverAttackDebugger: MonoBehaviour
    {
        private void Awake()
        {
            //Aqui vão todos os Scripts que devem ter seus debugs ativos
            
            //Panels
            DebugManager.SetScriptDebugLevel<PanelGraphicsOptions>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PanelFrameRateOptions>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PanelLanguagesOptions>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PanelResolutionOptions>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<AbstractMenuManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<MainMenu>(DebugManager.DebugLevels.None);

            
            DebugManager.SetScriptDebugLevel<PanelGamePause>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PanelGameManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PanelGameHud>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PanelGameOver>(DebugManager.DebugLevels.None);
            
            //Audios
            DebugManager.SetScriptDebugLevel<AudioManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<AudioEvent>(DebugManager.DebugLevels.None);
            
            //GamePlayManager
            DebugManager.SetScriptDebugLevel<GamePlayManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<GameOptionsSave>(DebugManager.DebugLevels.None);
            
            //Inputs
            DebugManager.SetScriptDebugLevel<ActionManager>(DebugManager.DebugLevels.None);
            
            //PoolManager
            DebugManager.SetScriptDebugLevel<PoolObject>(DebugManager.DebugLevels.LogsAndWarnings);
            
            //shopping
            DebugManager.SetScriptDebugLevel<SimpleShoppingManager>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<ShopProductSimpleSkins>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<FiniteNavigationMode>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<SmoothFiniteNavigationMode>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<ShopProductSettings>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<ShopSkinChanger>(DebugManager.DebugLevels.None);
            
            
            
            //Scenes e States
            DebugManager.SetScriptDebugLevel<GameState>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<StateManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<GameStateBriefingRoom>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<GameStateMenuInitial>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<GameStatePlay>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<GameStateGameOver>(DebugManager.DebugLevels.None);
            
            
            //Players
            DebugManager.SetScriptDebugLevel<PlayerMaster>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerController>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerSound>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerSkin>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerSkinTrail>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerDistance>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerCollisions>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerLives>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<UILifeDisplay>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerShoot>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<ObjectShoot>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerBombs>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<Bullets>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<BulletBombPlayer>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerFuel>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<UIFuelDisplay>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerPowerUp>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerAreaEffect>(DebugManager.DebugLevels.None);
            
            //Enemies
            DebugManager.SetScriptDebugLevel<ObjectMaster>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<ObstacleMaster>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<EnemiesMaster>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<ObstacleSkin>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<EnemiesExplosion>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<EnemiesMovement>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<ObstacleCollider>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<EnemiesDropItem>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<EnemiesShoot>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<CollectibleMaster>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<BridgeMaster>(DebugManager.DebugLevels.None);
            
            //Build
            DebugManager.SetScriptDebugLevel<LevelBuilderManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<HubGameManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<HubBuilder>(DebugManager.DebugLevels.None);
            
            //Boss
            DebugManager.SetScriptDebugLevel<BossMaster>(DebugManager.DebugLevels.None);
            
            //Behaviors
            DebugManager.SetScriptDebugLevel<BehaviorManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<Behavior>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<BossBehavior>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<GamePlayBossManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<EnterSceneBehavior>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<MoveNorthBehavior>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<MissileBehavior>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<CleanShootBehavior>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<BossMineShoot>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<BossMissileShoot>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<ObjectShoot>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<BossDirections>(DebugManager.DebugLevels.None);
            
            //Steam Services
            DebugManager.SetScriptDebugLevel<SteamConnectionManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<SteamAchievementService>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<SteamLeaderboardService>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<SteamStatsService>(DebugManager.DebugLevels.None);
            
            DebugManager.SetScriptDebugLevel<SteamServerUpdater>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<GameStatisticManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PanelStatisticsManager>(DebugManager.DebugLevels.None);
            
            
        }
    }
}