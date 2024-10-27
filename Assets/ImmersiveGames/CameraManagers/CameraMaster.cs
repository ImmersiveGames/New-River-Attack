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
        private static CinemachineVirtualCamera _virtualCamera;

        private void Awake()
        {
            SetInitialReferences();
        }
        public static void TargetPlayer(PlayerMaster playerMaster)
        {
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
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
    }
}