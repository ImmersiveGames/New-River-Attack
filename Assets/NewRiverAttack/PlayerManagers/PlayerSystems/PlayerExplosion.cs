using ImmersiveGames.CameraManagers;
using ImmersiveGames.Utils;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerExplosion : MonoBehaviour
    {
        [Header("Death Player Settings")]
        [SerializeField] private GameObject deadParticlePrefab;
        [SerializeField] private float timeoutDestroyExplosion= 2f;
        [Header("Shake Camera")]
        [SerializeField] private float shakeIntensity = 3f;
        [SerializeField] private float shakeTime = 0.2f;
        
        private PlayerMaster _playerMaster;

        #region Unity Methods

        private void Awake()
        {
            SetInitialReferences();
        }

        private void OnEnable()
        {
            _playerMaster.EventPlayerMasterGetHit += PlayerExplode;
            _playerMaster.EventPlayerMasterRespawn += PlayerRestore;
            _playerMaster.EventPlayerMasterForceExplode += PlayerForceExplode;
        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterGetHit -= PlayerExplode;
            _playerMaster.EventPlayerMasterRespawn -= PlayerRestore;
            _playerMaster.EventPlayerMasterForceExplode -= PlayerForceExplode;
        }

        #endregion
        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
        }
        private void PlayerExplode()
        {
            CameraShake.ShakeCamera(shakeIntensity,shakeTime);
            if (_playerMaster.BossController) return;
            
            _playerMaster.OnEventPlayerMasterToggleSkin(false);
            var go = Instantiate(deadParticlePrefab, transform);
            Destroy(go, timeoutDestroyExplosion);
        }
        private void PlayerForceExplode()
        {
            CameraShake.ShakeCamera(shakeIntensity,shakeTime);
            _playerMaster.OnEventPlayerMasterToggleSkin(false);
            var go = Instantiate(deadParticlePrefab, transform);
            Destroy(go, timeoutDestroyExplosion);
        }
        

        private void PlayerRestore()
        {
            _playerMaster.OnEventPlayerMasterToggleSkin(true);
        }
    }
}