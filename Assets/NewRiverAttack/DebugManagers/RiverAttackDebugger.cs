using ImmersiveGames;
using ImmersiveGames.AudioEvents;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BulletsManagers;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.LevelBuilder;
using ImmersiveGames.MenuManagers;
using ImmersiveGames.MenuManagers.Abstracts;
using ImmersiveGames.MenuManagers.PanelOptionsManagers;
using ImmersiveGames.PoolManagers;
using ImmersiveGames.PoolManagers.Test;
using ImmersiveGames.ShopManagers.NavigationModes;
using ImmersiveGames.StateManagers;
using NewRiverAttack.BulletsManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.HUBManagers;
using NewRiverAttack.HUDManagers.UI;
using NewRiverAttack.LevelBuilder;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.ObstaclesSystems.BossSystems;
using NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours;
using NewRiverAttack.ObstaclesSystems.BossSystems.Strategies;
using NewRiverAttack.ObstaclesSystems.BridgeSystems;
using NewRiverAttack.ObstaclesSystems.CollectibleSystems;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using NewRiverAttack.SaveManagers;
using NewRiverAttack.ShoppingSystems.SimpleShopping;
using NewRiverAttack.ShoppingSystems.SimpleShopping.Abstracts;
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
            DebugManager.SetScriptDebugLevel<GameStartMenu>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<MenuGamePauseManager>(DebugManager.DebugLevels.None);
            
            
            //Audios
            DebugManager.SetScriptDebugLevel<AudioManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<AudioEvent>(DebugManager.DebugLevels.None);
            
            //GamePlayManager
            DebugManager.SetScriptDebugLevel<GamePlayManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<GameOptionsSave>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerAchievements>(DebugManager.DebugLevels.None);
            
            //Inputs
            DebugManager.SetScriptDebugLevel<ActionManager>(DebugManager.DebugLevels.None);
            
            //PoolManager
            DebugManager.SetScriptDebugLevel<PoolObject>(DebugManager.DebugLevels.LogsAndWarnings);
            DebugManager.SetScriptDebugLevel<SpherePoolable>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PoolTest>(DebugManager.DebugLevels.None);
            
            //shopping
            DebugManager.SetScriptDebugLevel<SimpleShoppingManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<ShopProductSimpleSkins>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<FiniteNavigationMode>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<SmoothFiniteNavigationMode>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<ShopProductSettings>(DebugManager.DebugLevels.None);
            
            
            //Scenes e States
            DebugManager.SetScriptDebugLevel<GameState>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<StateManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<GameStateBriefingRoom>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<GameStateMenuInitial>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<GameStatePlay>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<GameStateGameOver>(DebugManager.DebugLevels.None);
            
            
            //Players
            DebugManager.SetScriptDebugLevel<PlayerMaster>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<PlayerController>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerSound>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerSkin>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<PlayerSkinTrail>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerDistance>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerCollisions>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerLives>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<UILifeDisplay>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerShoot>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<ObjectShoot>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerBombs>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<Bullets>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<BulletBombPlayer>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerFuel>(DebugManager.DebugLevels.All);
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
            /*DebugManager.SetScriptDebugLevel<BehaviorManager>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<Behavior>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<BossBehavior>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<GamePlayBossManager>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<EnterSceneBehavior>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<MoveNorthBehavior>(DebugManager.DebugLevels.All);*/

            
        }
    }
}