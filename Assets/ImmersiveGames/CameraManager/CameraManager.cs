using Cinemachine;
using ImmersiveGames.GamePlayManagers;
using ImmersiveGames.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace ImmersiveGames.CameraManager
{
    public class CameraManager : MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;
        private GamePlayManager _gamePlayManager;

        private void Awake()
        {
            SetInitialReferences();
            _gamePlayManager.EventPlayerInitialize += TargetPlayer;
        }

        private void OnDisable()
        {
            _gamePlayManager.EventPlayerInitialize -= TargetPlayer;
        }

        private void TargetPlayer(PlayerMaster playerMaster)
        {
            if (playerMaster != null)
            {
                _virtualCamera.Follow = playerMaster.transform;
            }
            else
            {
                Debug.LogWarning("PlayerMaster não encontrado para o índice especificado.");
            }
        }


        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        public CinemachineVirtualCamera GetVirtualCamera()
        {
            return _virtualCamera;
        }
    }
}