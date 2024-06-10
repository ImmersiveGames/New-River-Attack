using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.BulletsManagers;
using NewRiverAttack.LevelBuilder.Abstracts;
using NewRiverAttack.PlayerManagers.Tags;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerCollisions: MonoBehaviour
    {
        private PlayerMaster _playerMaster;
        private PlayerAchievements _playerAchievements;
        private bool _invulnerability;
        private Collider[] _colliders;

        #region Unity Methods
        private void Awake()
        {
            SetInitialReferences();
        }

        private void OnEnable()
        {
            _playerMaster.EventPlayerMasterChangeSkin += GetSkinColliders;
            _playerMaster.EventPlayerMasterRespawn += PlayerRestoreColliders;
            _playerMaster.EventPlayerMasterGetHit += PlayerDisableColliders;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (MustCollider(other) || !_playerMaster.ObjectIsReady || _playerMaster.InFinishPath) return;
            if (_playerMaster.godMode || _invulnerability) return;
            //Se não for nada disso significa que o player será destruído.
            _playerAchievements.LogCollision(other);
            _playerMaster.OnEventPlayerMasterGetHit();
            DebugManager.Log<PlayerCollisions>($"Collider: {other}");
        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterChangeSkin -= GetSkinColliders;
            _playerMaster.EventPlayerMasterRespawn -= PlayerRestoreColliders;
            _playerMaster.EventPlayerMasterGetHit -= PlayerDisableColliders;
        }

        private void PlayerRestoreColliders()
        {
            SetPlayerColliders(true);
            _invulnerability = false;
        }
        private void PlayerDisableColliders()
        {
            SetPlayerColliders(false);
            _invulnerability = true;
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
            _playerAchievements = GetComponent<PlayerAchievements>();
            GetSkinColliders(null);
        }
        

        private void GetSkinColliders(ShopProductSkin shopProductSkin)
        {
            var skin = GetComponentInChildren<SkinAttach>();
            if (skin == null) return;
            _colliders = skin.GetComponentsInChildren<Collider>();
        }

        private void SetPlayerColliders(bool active)
        {
            foreach (var collider1 in _colliders)
            {
                collider1.enabled = active;
            }
        }

        private bool MustCollider(Component component)
        {
            return component.GetComponent<BulletPlayer>() != null || component.GetComponent<BulletBombPlayer>() != null 
                                                                  || component.GetComponent<LevelFinishers>() != null 
                                                                  || component.GetComponentInParent<ICollectable>() != null 
                                                                  ||  component.GetComponentInParent<IAreaEffect>() != null;
        }
        
    }
}