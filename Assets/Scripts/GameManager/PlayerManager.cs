using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class PlayerManager: Singleton<PlayerManager>
    {
        [Header("Player Settings")]
        public GameObject playerPrefab;
        public Vector3 spawnPlayerPosition;
        public List<PlayerSettings> playerSettingsList = new List<PlayerSettings>();
        [SerializeField] internal List<PlayerMaster> initializedPlayerMasters = new List<PlayerMaster>();
        
        protected override void OnDestroy()
        {
            //base.OnDestroy();
        }

        public void InstantiatePlayers()
        {
            if (initializedPlayerMasters.Count != 0)
                return;
            var playerSettings = playerSettingsList[^1];
            var playerObject = Instantiate(playerPrefab, spawnPlayerPosition, Quaternion.identity);
            playerObject.name = playerSettings.name;
            var playerMaster = playerObject.GetComponent<PlayerMaster>();
            GameTimelineManager.instance.InitializePLayerInTimeline(playerObject.transform, playerMaster.GetPlayerAnimator());
            playerMaster.SetPlayerSettingsToPlayMaster(playerSettings);
            initializedPlayerMasters.Add(playerMaster);
            
        }
        public void ActivePlayers(bool active)
        {
            if (initializedPlayerMasters.Count <= 0) return;
            foreach (var playerMaster in initializedPlayerMasters)
            {
                playerMaster.gameObject.SetActive(active);
            }
        }
        public bool haveAnyPlayerInitialized
        {
            get { return initializedPlayerMasters.Count > 0; }
        }

        public void UnPausedMovementPlayers()
        {
            if (initializedPlayerMasters.Count <= 0) return;
            foreach (var playerMaster in initializedPlayerMasters)
            {
                playerMaster.playerMovementStatus = PlayerMaster.MovementStatus.None;
            }
        }
        public void RemoveAllPlayers()
        {
            if (initializedPlayerMasters.Count <= 0) return;
            foreach (var playerMaster in initializedPlayerMasters)
            {
                DestroyImmediate(playerMaster.gameObject);
            }
            initializedPlayerMasters = new List<PlayerMaster>();
        }
        public int HighScorePlayers()
        {
            if (haveAnyPlayerInitialized == false) return 0;
            int score = 0;
            foreach (var pl in initializedPlayerMasters.Where(pl => score < pl.GetComponent<PlayerMaster>().getPlayerSettings.score))
            {
                score += pl.GetComponent<PlayerMaster>().getPlayerSettings.score;
            }

            return score;
        }
        public PlayerSettings GetPlayerSettingsByIndex(int playerIndex = 0)
        {
            return playerSettingsList.Count > 0 ? playerSettingsList[playerIndex] : null;
        }
    }
}
