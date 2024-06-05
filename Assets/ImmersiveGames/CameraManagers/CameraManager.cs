using Cinemachine;
using ImmersiveGames.Utils;
using UnityEngine;

namespace ImmersiveGames.CameraManagers
{
    public class CameraManager : Singleton<CameraManager>
    {
        public CinemachineVirtualCamera[] virtualCameras;

        private static CinemachineVirtualCamera _endVirtualCamera;
        private static CinemachineVirtualCamera _principalVirtualCamera;

        protected override void Awake()
        {
            base.Awake();
            _principalVirtualCamera = virtualCameras[0];
            _endVirtualCamera = virtualCameras[^1];
        }

        public static void RepositionEndCamera(Vector3 zPosition)
        {
            _endVirtualCamera.transform.position = new Vector3(zPosition.x, zPosition.y, zPosition.z);
        }

        public static void ActiveEndCamera(bool active)
        {
            _endVirtualCamera.gameObject.SetActive(active);
        }
    }
}