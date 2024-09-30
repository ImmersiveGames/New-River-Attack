using System.Collections.Generic;
using ImmersiveGames;
using ImmersiveGames.Utils;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.SaveManagers;
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
            // Sempre use o index salvo mais alto ao retornar para o HUB
            var highestIndex = GameOptionsSave.Instance.activeIndexMissionLevel;

            // Desativa a ponte se o índice da ponte for menor que o índice salvo
            gameObject.SetActive(_hubOrder >= highestIndex);
            // Mantém ativa se for maior ou igual
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
