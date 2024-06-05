using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerCollectible : MonoBehaviour
    {
        private PlayerMaster _playerMaster;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
        }

        private void OnTriggerEnter(Collider other)
        {
            var collect = other.GetComponentInParent<ICollectable>();
            if (collect == null) return;
            DebugManager.Log<PlayerCollisions>($"Active: {other}");
            collect.Collect(_playerMaster);
            _playerMaster.OnEventPlayerMasterCollect(collect);
        }

        #endregion
        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
        }
    }
}