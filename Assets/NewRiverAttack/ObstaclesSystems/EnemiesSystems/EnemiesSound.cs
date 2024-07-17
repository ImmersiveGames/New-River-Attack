using ImmersiveGames.AudioEvents;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesSound : MonoBehaviour
    {
        [SerializeField] protected AudioEvent audioExplosion;

        protected ObstacleMaster ObstacleMaster;
        protected AudioSource AudioSource;

        #region Unity Methods

        protected virtual void OnEnable()
        {
            SetInitialReferences();
            ObstacleMaster.EventObstacleDeath += ExplodeSound;
        }

        protected virtual void OnDisable()
        {
            ObstacleMaster.EventObstacleDeath -= ExplodeSound;
        }

        #endregion
        protected virtual void SetInitialReferences()
        {
            ObstacleMaster = GetComponent<ObstacleMaster>();
            AudioSource = GetComponentInChildren<AudioSource>();
        }
        
        private void ExplodeSound(PlayerMaster playerMaster)
        {
            if (AudioSource == null || audioExplosion == null) return;
            audioExplosion.PlayOnShot(AudioSource);
        }
    }
}