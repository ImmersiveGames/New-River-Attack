using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ImmersiveGames;
using NewRiverAttack.AudioManagers;

namespace NewRiverAttack.HUBManagers.UI
{
    public class UiHubPlayer: MonoBehaviour
    {
        private HubGameManager _hubGameManager;
        public float offsetZ = 10f;
        [Header("Move Animation")]
        public float moveTime = 1.0f;
        public float rotateTime = 0.5f;
        public Ease exitRotationAnimation;
        public Ease enterRotationAnimation;
        public Ease enterAnimation;
        
        private AudioSource _audioSource;
        private void OnEnable()
        {
            SetInitialReferences();
            _hubGameManager.EventInitializeHub += StartPosition;
            _hubGameManager.EventCursorUpdateHub += CursorUpdatePosition;
        }
        private void SetInitialReferences()
        {
            _hubGameManager = HubGameManager.instance;
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnDisable()
        {
            _hubGameManager.EventInitializeHub -= StartPosition;
            _hubGameManager.EventCursorUpdateHub -= CursorUpdatePosition;
        }

        private void CursorUpdatePosition(List<HubOrderData> hubOrderData, int startIndex)
        {
            var position = SetPosition(hubOrderData[startIndex].position);
            MoveObject( position.z,  moveTime, rotateTime);
        }

        private void StartPosition(List<HubOrderData> hubOrderData, int startIndex)
        {
            transform.position = SetPosition(hubOrderData[startIndex].position);
        }

        private Vector3 SetPosition(float iconPosition)
        {
            var transform1 = transform;
            var position1 = transform1.position;
            var position = position1;
            return new Vector3(position.x, position.y, iconPosition - offsetZ);
        }

        private void MoveObject(float targetZ, float moveDuration, float rotateDuration)
        {
            // Get current object's Z position
            var cursor = gameObject;
            var currentZPosition = cursor.transform.position.z;

            // Calculate movement direction
            var direction = currentZPosition - targetZ;

            // Set initial rotation as a Vector3 (preserve Y rotation)
            var initialRotation = cursor.transform.rotation.eulerAngles;

            // Generate final rotation (0 degrees around Y axis)
            var finalRotation = Vector3.zero;

            // Check if object needs to rotate
            if (direction > 0)
            {
                // 180-degree rotation around Y axis if target position is greater than current position
                initialRotation = new Vector3(-180, 0, 0);
            }
            var audioGameOver = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxEngineAccelerate);
            // Create DoTween sequences for animation
            DOTween.Sequence()
                .OnStart(() =>
                {
                    _hubGameManager.IsHubReady = false;
                    audioGameOver.PlayOnShot(_audioSource);
                })
                .Append(gameObject.transform.DOMoveZ(targetZ, moveDuration).SetEase(enterAnimation))  // Slow down at start (InQuad)
                .Join(gameObject.transform.DORotate(initialRotation, moveDuration).SetEase(enterRotationAnimation))
                .Append(gameObject.transform.DORotate(finalRotation, rotateDuration).SetEase(exitRotationAnimation))  // Speed up at end (OutQuad)
                .OnComplete(() =>
                {
                    audioGameOver.Stop(_audioSource);
                    _hubGameManager.IsHubReady = true;
                })
                .Play();
        }
    }
}
