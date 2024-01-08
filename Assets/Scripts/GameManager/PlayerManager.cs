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
            GameTimelineManager.instance.InitializePLayerInTimeline(playerObject.transform, playerMaster.GetComponent<Animator>());
            playerMaster.SetPlayerSettingsToPlayMaster(playerSettings);
            initializedPlayerMasters.Add(playerMaster);
            
        }
        public void ActivePlayers(bool active)
        {
            if (initializedPlayerMasters.Count <= 0) return;
            foreach (var playerMaster in initializedPlayerMasters)
            {
                GameObject o;
                (o = playerMaster.gameObject).SetActive(active);
                o.transform.rotation = Quaternion.identity;
            }
        }
        internal Transform GetTransformFirstPlayer()
        {
            return initializedPlayerMasters[0].transform;
        }
        internal void DestroyPlayers()
        {
            foreach (var playerMaster in initializedPlayerMasters)
            {
                Destroy(playerMaster.gameObject);
            }
            initializedPlayerMasters = new List<PlayerMaster>();
        }
        bool haveAnyPlayerInitialized
        {
            get { return initializedPlayerMasters.Count > 0; }
        }

        public void UnPausedMovementPlayers()
        {
            if (initializedPlayerMasters.Count <= 0) return;
            foreach (var playerMaster in initializedPlayerMasters)
            {
                playerMaster.playerMovementStatus = PlayerMaster.MovementStatus.None;
                playerMaster.gameObject.transform.rotation = Quaternion.identity;
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
