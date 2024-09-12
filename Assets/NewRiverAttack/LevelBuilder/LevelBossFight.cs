using ImmersiveGames.Utils;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.LevelBuilder.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.LevelBuilder
{
    public class LevelBossFight : LevelFinishers
    {
        private GamePlayBossManager _gamePlayBossManager;
        private PlayerMaster _playerMaster;
        private MoveTiles _moveTiles;
        
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
            
            GetComponent<Collider>().enabled = false;
            Invoke(nameof(StopAutoMove),1.5f);
        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _gamePlayBossManager = GamePlayBossManager.instance;
        }
        private void StopAutoMove()
        {
            _playerMaster.BossController = true;
            _playerMaster.AutoPilot = false;
            
            //Aqui precisa mudar para reiniciar no local da morte
            var position = transform.position;
            var savePosition = new Vector3(position.x, _playerMaster.transform.position.y,
                position.z);
            _playerMaster.SavePosition(savePosition);
            _gamePlayBossManager.OnEventEnterBoss();
            _moveTiles.ActiveTiles(_playerMaster);
        }
    }
}