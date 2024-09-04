using System.Collections.Generic;
using ImmersiveGames;
using ImmersiveGames.Utils;
using NewRiverAttack.AudioManagers;
using UnityEngine;

namespace NewRiverAttack.HUBManagers.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class UiHubBridges : MonoBehaviour
    {
        public Transform vfxExplosion;
        private Transform _bridges;
        private int _hubOrder;
        private HubGameManager _hubGameManager;
        private AudioSource _audioSource;

        private void OnEnable()
        {
            SetInitialReferences();
            _hubGameManager.EventInitializeHub += InitializeBridge;
        }

        private void OnDisable()
        {
            _hubGameManager.EventInitializeHub -= InitializeBridge;
        }

        private void InitializeBridge(List<HubOrderData> listHubOrderData, int startIndex)
        {
            if (startIndex > _hubOrder)
            {
                gameObject.SetActive(false);
            }
        }

        private void SetInitialReferences()
        {
            _hubGameManager = HubGameManager.Instance;
            _audioSource = GetComponent<AudioSource>();
        }

        internal void ExplodeBridge()
        {
            Tools.ToggleChildren(transform,false);
            var audioGameOver = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxExplode);
            audioGameOver.PlayOnShot(_audioSource);
            vfxExplosion.gameObject.SetActive(true);
        }
        public void SetBridge(int hubIndex)
        {
            _hubOrder = hubIndex;
        }
    }
}
