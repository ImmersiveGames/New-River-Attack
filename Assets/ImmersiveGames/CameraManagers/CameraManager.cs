using Cinemachine;
using ImmersiveGames.Utils;
using UnityEngine;

namespace ImmersiveGames.CameraManagers
{
    public class CameraManager : Singleton<CameraManager>
    {
        public CinemachineVirtualCamera[] virtualCameras;

        private static CinemachineVirtualCamera _endVirtualCamera;
        private static CinemachineVirtualCamera _startVirtualCamera;

        protected override void Awake()
        {
            base.Awake();
            _endVirtualCamera = virtualCameras[^1];
            _startVirtualCamera = virtualCameras[1];
        }

        public static void RepositionEndCamera(Vector3 zPosition)
        {
            _endVirtualCamera.transform.position = new Vector3(zPosition.x, zPosition.y, zPosition.z);
        }

        public static void ActiveEndCamera(bool active)
        {
            _endVirtualCamera.gameObject.SetActive(active);
        }

        public static void ActiveStartCamera()
        {
            _startVirtualCamera.gameObject.SetActive(true);
            _startVirtualCamera.transform.position = _startVirtualCamera.transform.position;
            _startVirtualCamera.transform.rotation = _startVirtualCamera.transform.rotation;
        }
    }
}