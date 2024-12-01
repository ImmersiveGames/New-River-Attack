using ImmersiveGames;
using ImmersiveGames.Utils;
using NewRiverAttack.AudioManagers;
using UnityEngine;

namespace NewRiverAttack.HUBManagers.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class UiHubBridges : MonoBehaviour
    {
        public Transform vfxExplosion; // Referência ao efeito visual de explosão
        
        private int _hubIndex; // Índice da ponte no HUB
        
        private HubGameManager _hubGameManager;
        private AudioSource _audioSource;

        private void Awake()
        {
            SetInitialReferences();
        }
        private void OnEnable()
        {
            _hubGameManager.EventExplodeBridge += ExplodeBridge;
        }
        private void OnDisable()
        {
            _hubGameManager.EventExplodeBridge -= ExplodeBridge;
        }
        /// <summary>
        /// Configura o índice da ponte no HUB.
        /// </summary>
        public void SetBridge(int hubIndex)
        {
            _hubIndex = hubIndex;
            UpdateBridge();
        }
        
        private void SetInitialReferences()
        {
            _hubGameManager = HubGameManager.Instance;
            _audioSource = GetComponent<AudioSource>();
        }
        private void UpdateBridge()
        {
            var highestIndex = _hubGameManager.SaveIndex;
            // Desativa a ponte se o jogador já passou do nível associado
            gameObject.SetActive(_hubIndex >= highestIndex);
        }
        private void ExplodeBridge(int obj)
        {
            if(obj != _hubIndex) return;
            Tools.ToggleChildren(transform,false);
            var audioGameOver = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxExplode);
            audioGameOver.PlayOnShot(_audioSource);
            vfxExplosion.gameObject.SetActive(true);
        }
    }
}
