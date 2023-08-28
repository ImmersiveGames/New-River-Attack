using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace RiverAttack
{
    public sealed class GamePlayManager : Singleton<GamePlayManager>
    {
        [SerializeField] internal bool completePath;
        [SerializeField]
        bool godMode;
        [SerializeField] public bool godModeSpeed;
        [SerializeField]
        GameSettings gameSettings;
        internal bool getGodMode { get { return godMode; } }
        [SerializeField]
        internal GamePlaySettings gamePlaySettings;

        internal bool playerDead;

        GameManager m_GameManager;
        PlayersInputActions m_InputSystem;
        #region Delegates
        public delegate void GeneralEventHandler();
        internal event GeneralEventHandler EventActivateEnemiesMaster;
        internal event GeneralEventHandler EventDeactivateEnemiesMaster;
        internal event GeneralEventHandler EventReSpawnEnemiesMaster;
        internal event GeneralEventHandler EventEnemiesMasterKillPlayer;
        internal event GeneralEventHandler EventPlayerPushButtonShoot;
        internal event GeneralEventHandler EventPlayerPushButtonBomb;
        internal delegate void UiUpdateEventHandler(int value);
        internal event UiUpdateEventHandler EventUpdateScore;
        internal event UiUpdateEventHandler EventUpdateDistance;
        internal event UiUpdateEventHandler EventUpdateRefugees;
        internal event UiUpdateEventHandler EventUpdateBombs;
        internal event UiUpdateEventHandler EventUpdateLives;

        internal delegate void BuildPatchHandler(float position);
        internal event BuildPatchHandler EventBuildPathUpdate;

        public delegate void CollectableEventHandle(CollectibleScriptable collectibles);
        public event CollectableEventHandle EventCollectItem;

        public delegate void ShakeCamEventHandle(float power, float inTime);
        public event ShakeCamEventHandle EventShakeCam;
        #endregion

        #region UNITYMETHODS
        void OnEnable()
        {
            m_GameManager = GameManager.instance;
        }
  #endregion
        public GameSettings getGameSettings
        {
            get { return gameSettings; }
        }

        public bool shouldBePlayingGame { get { return (m_GameManager.currentGameState is GameStatePlayGame && !completePath); } }

        public PlayerSettings GetNoPlayerPlayerSettings(int playerIndex = 0)
        {
            return m_GameManager.playerSettingsList.Count > 0 ? m_GameManager.playerSettingsList[playerIndex] : null;
        }
        public static void AddResultList(List<LogResults> list, PlayerSettings playerSettings, EnemiesScriptable enemy, int qnt, CollisionType collisionType)
        {
            var itemResults = list.Find(x => x.player == playerSettings && x.enemy == enemy && x.collisionType == collisionType);
            if (itemResults != null)
            {
                if (enemy is CollectibleScriptable collectibles)
                {
                    if (collectibles.maxCollectible == 0 || itemResults.quantity + qnt < collectibles.maxCollectible)
                        itemResults.quantity += qnt;
                    else
                        itemResults.quantity = collectibles.maxCollectible;
                }
                else
                    itemResults.quantity += qnt;
            }
            else
            {
                var newItemResults = new LogResults(playerSettings, enemy, qnt, collisionType);
                list.Add(newItemResults);
            }
        }
        public int HighScorePlayers()
        {
            if (m_GameManager.haveAnyPlayerInitialized == false) return 0;
            int score = 0;
            foreach (var pl in m_GameManager.initializedPlayerMasters.Where(pl => score < pl.GetComponent<PlayerMaster>().getPlayerSettings.score))
            {
                score += pl.GetComponent<PlayerMaster>().getPlayerSettings.score;
            }

            return score;
        }

        #region Calls
        internal void OnEventActivateEnemiesMaster()
        {
            EventActivateEnemiesMaster?.Invoke();
        }
        internal void OnEventDeactivateEnemiesMaster()
        {
            EventDeactivateEnemiesMaster?.Invoke();
        }
        internal void OnEventReSpawnEnemiesMaster()
        {
            EventReSpawnEnemiesMaster?.Invoke();
        }
        internal void OnEventEnemiesMasterKillPlayer()
        {
            EventEnemiesMasterKillPlayer?.Invoke();
        }
        internal void OnEventPlayerPushButtonShoot()
        {
            EventPlayerPushButtonShoot?.Invoke();
        }
        internal void OnEventPlayerPushButtonBomb()
        {
            EventPlayerPushButtonBomb?.Invoke();
        }
        internal void OnEventUpdateScore(int value)
        {
            EventUpdateScore?.Invoke(value);
        }
        internal void OnEventUpdateDistance(int value)
        {
            EventUpdateDistance?.Invoke(value);
        }
        internal void OnEventUpdateRefugees(int value)
        {
            EventUpdateRefugees?.Invoke(value);
        }
        internal void OnEventUpdateBombs(int value)
        {
            EventUpdateBombs?.Invoke(value);
        }
        internal void OnEventUpdateLives(int value)
        {
            EventUpdateLives?.Invoke(value);
        }
        internal void OnEventCollectItem(CollectibleScriptable collectibles)
        {
            EventCollectItem?.Invoke(collectibles);
        }
        internal void OnEventShakeCam(float power, float intime)
        {
            EventShakeCam?.Invoke(power, intime);
        }
        internal void OnEventBuildPathUpdate(float position)
        {
            EventBuildPathUpdate?.Invoke(position);
        }
  #endregion

    }
}
