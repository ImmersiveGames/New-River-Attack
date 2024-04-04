using ImmersiveGames.AudioEvents;
using ImmersiveGames.GamePlayManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.LevelBuilder;
using ImmersiveGames.MenuManagers;
using ImmersiveGames.MenuManagers.Abstracts;
using ImmersiveGames.MenuManagers.PanelOptionsManagers;
using ImmersiveGames.PlayerManagers.PlayerSystems;
using ImmersiveGames.PoolManagers;
using ImmersiveGames.PoolManagers.Test;
using ImmersiveGames.ShopManagers.SimpleShopping;
using ImmersiveGames.StateManagers;
using ImmersiveGames.StateManagers.States;
using UnityEngine;

namespace ImmersiveGames.DebugManagers.Test
{
    public class TestDebugManager: MonoBehaviour
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
            
            //Audios
            DebugManager.SetScriptDebugLevel<AudioManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<AudioEvent>(DebugManager.DebugLevels.None);
            
            //GamePlayManager
            DebugManager.SetScriptDebugLevel<GamePlayManager>(DebugManager.DebugLevels.None);
            
            //Inputs
            DebugManager.SetScriptDebugLevel<ActionManager>(DebugManager.DebugLevels.None);
            
            //PoolManager
            DebugManager.SetScriptDebugLevel<PoolObject>(DebugManager.DebugLevels.LogsAndWarnings);
            DebugManager.SetScriptDebugLevel<SpherePoolable>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<PoolTest>(DebugManager.DebugLevels.All);
            
            //shopping
            DebugManager.SetScriptDebugLevel<SimpleShoppingManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<ShopProductSimpleSkins>(DebugManager.DebugLevels.None);
            
            //Scenes e States
            DebugManager.SetScriptDebugLevel<GameState>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<GameStatePause>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<GameStateBriefingRoom>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<GameStateMenuInicial>(DebugManager.DebugLevels.All);
            DebugManager.SetScriptDebugLevel<GameStatePlay>(DebugManager.DebugLevels.All);
            
            //Players
            DebugManager.SetScriptDebugLevel<PlayerMaster>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerController>(DebugManager.DebugLevels.None);
            
            //Build
            DebugManager.SetScriptDebugLevel<LevelBuilderManager>(DebugManager.DebugLevels.None);
        }
    }
}