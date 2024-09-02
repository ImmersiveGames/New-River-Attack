using System;
using Cinemachine;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace ImmersiveGames.CameraManagers
{
    public class CameraMaster : MonoBehaviour
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
            //Debug.Log($"playerMaster: {playerMaster}");
            if (playerMaster != null)
            {
                _virtualCamera.Follow = playerMaster.transform;
                DebugManager.Log<CameraManager>($"Camera: {_virtualCamera.Follow}");
            }
            else
            {
                DebugManager.LogWarning<CameraManager>("PlayerMaster não encontrado para o índice especificado.");
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