using System;
using Cinemachine;
using ImmersiveGames;
using ImmersiveGames.Utils;
using NewRiverAttack.LevelBuilder.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.LevelBuilder
{
    public class LevelBossFight : LevelFinishers
    {
        private PlayerMaster _playerMaster;
        protected override void OnTriggerEnter(Collider other)
        {
            _playerMaster = other.GetComponentInParent<PlayerMaster>();
            if (_playerMaster == null) return;
            _playerMaster.AutoPilot = true;
            
            GetComponent<Collider>().enabled = false;
        }
        /*private bool _changeControl;
        
        private MoveTiles _moveTiles;

        public float speed;

        protected override void Start()
        {
            base.Start();
            _moveTiles = GetComponentInChildren<MoveTiles>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            _playerMaster = other.GetComponentInParent<PlayerMaster>();
            if (_playerMaster == null) return;
            _playerMaster.AutoPilot = true;
            
            Invoke(nameof(StopAutoMove),1.5f);
            GetComponent<Collider>().enabled = false;

        }

        private void StopAutoMove()
        {
            _playerMaster.AutoPilot = false;
            _moveTiles.ActiveTiles(_playerMaster);
        }

        void LateUpdate()
        {
            if (_playerMaster != null)
            {
                // Calcular a nova posição no plano XZ sem mudar a rotação
                Vector3 targetPosition = new Vector3(0, transform.position.y, _playerMaster.transform.position.z);
                Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, 
                    _playerMaster.ActualSkin.playerSpeed);
                transform.position = newPosition;
            }
        }*/
    }
}