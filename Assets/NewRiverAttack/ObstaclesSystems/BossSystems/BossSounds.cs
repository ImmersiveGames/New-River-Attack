using ImmersiveGames.AudioEvents;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossSounds : MonoBehaviour
    {
        [SerializeField] private AudioEvent audioExplosion;
        [SerializeField] private AudioEvent audioHit;
        private ObstacleMaster _obstacleMaster;
        private AudioSource _audioSource;
        
        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _obstacleMaster.EventObstacleHit += HitSound;
            _obstacleMaster.EventObstacleDeath += ExplodeSound;
        }

        private void OnDisable()
        {
            _obstacleMaster.EventObstacleDeath -= ExplodeSound;
            _obstacleMaster.EventObstacleHit -= HitSound;
        }

        #endregion
        private void SetInitialReferences()
        {
            _obstacleMaster = GetComponent<ObstacleMaster>();
            _audioSource = GetComponentInChildren<AudioSource>();
        }
        
        private void ExplodeSound(PlayerMaster playerMaster)
        {
            if (audioExplosion == null || _audioSource == null) return;
            audioExplosion.PlayOnShot(_audioSource);
        }
        private void HitSound(PlayerMaster playerMaster)
        {
            if (audioHit == null || _audioSource == null) return;
            audioHit.PlayOnShot(_audioSource);
        }
    }
}