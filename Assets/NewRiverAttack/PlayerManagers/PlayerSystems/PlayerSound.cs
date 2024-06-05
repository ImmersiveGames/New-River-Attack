using ImmersiveGames;
using ImmersiveGames.AudioEvents;
using ImmersiveGames.DebugManagers;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerSound : MonoBehaviour
    {
        
        [SerializeField] private AudioEvent audioEngineLoop;
        [SerializeField] private AudioEvent audioStartAccelEngine;
        [SerializeField] private AudioEvent audioEngineAccelerator;
        [SerializeField] private AudioEvent audioStartDeceleratorEngine;
        [SerializeField] private AudioEvent audioDeceleratorEngine;
        [SerializeField] private AudioEvent audioPlayerExplosion;
        
        private PlayerMaster _playerMaster;
        private AudioManager _audioManager;
        private AudioSource _audioSource;
        private bool _onAccelerate;
        private bool _onDecelerate;

        #region unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            audioEngineLoop.SimplePlay(_audioSource);
            _playerMaster.EventPlayerMasterAxisMovement += SoundEngine;
            _playerMaster.EventPlayerMasterGetHit += PlayerExplode;
            _playerMaster.EventPlayerMasterReady += RestartEngine;
        }

        private void OnDisable()
        {
            _audioSource.Stop();
            _playerMaster.EventPlayerMasterAxisMovement -= SoundEngine;
            _playerMaster.EventPlayerMasterGetHit -= PlayerExplode;
            _playerMaster.EventPlayerMasterReady -= RestartEngine;
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void SoundEngine(Vector2 dir)
        {
            if (!_playerMaster.ObjectIsReady) return;
            var accelerate = dir.y > 0 && !_onAccelerate;
            var decelerate = dir.y < 0 && !_onDecelerate;
            
            if (accelerate)
            {
                _onAccelerate = true;
                DebugManager.Log<PlayerSound>($"Acelerado");
                ChangeEngine(audioStartAccelEngine, audioEngineAccelerator);
            }
            else if (decelerate)
            {
                _onDecelerate = true;
                DebugManager.Log<PlayerSound>($"Desacelerar");
                ChangeEngine(audioStartDeceleratorEngine, audioDeceleratorEngine);

            }
            else if (dir.y == 0 && (_onAccelerate || _onDecelerate))
            {
                RestartEngine();
            }
        }

        private void PlayerExplode()
        {
            _audioSource.Stop();
            audioPlayerExplosion.PlayOnShot(_audioSource);
        }
        private void RestartEngine()
        {
            DebugManager.Log<PlayerSound>($"Normal");
            _onAccelerate = false;
            _onDecelerate = false;
            audioEngineLoop.SimplePlay(_audioSource);
        }
        
        private void ChangeEngine(AudioEvent audioStart, AudioEvent audioLoop)
        {
            audioStart.PlayOnShot(_audioSource);
            audioLoop.SimplePlay(_audioSource);
        }
    }
}