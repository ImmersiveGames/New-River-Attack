using UnityEngine;
using DG.Tweening;
using ImmersiveGames;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.GameManagers;
using NewRiverAttack.SaveManagers;

namespace NewRiverAttack.HUBManagers.UI
{
    public class UiHubPlayer : MonoBehaviour
    {
        public float distanceFromCenter = 3f;
        
        public float moveDuration = 1f; // Tempo para movimento
        public float rotateDuration = 0.5f; // Tempo para rotação
        
        public Ease exitRotationAnimation;
        public Ease enterRotationAnimation;
        public Ease enterAnimation;
        
        private HubGameManager _hubGameManager;
        private GameManager _gameManager;
        private AudioSource _audioSource;

        private void Awake()
        {
            _gameManager = GameManager.instance;
            _hubGameManager = HubGameManager.Instance;
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _hubGameManager.EventBuildHub += StartPosition;
            _hubGameManager.EventCursorMove += HandleCursorMove;
        }

        private void Start()
        {
            SetSkin();
        }

        private void OnDisable()
        {
            _hubGameManager.EventBuildHub -= StartPosition;
            _hubGameManager.EventCursorMove -= HandleCursorMove;
        }

        private void StartPosition()
        {
            var hubIndex = _gameManager.ActiveIndex >= 0? _gameManager.ActiveIndex: _hubGameManager.SaveIndex;
            var iconPosition = _hubGameManager.GetPositionByIndex(hubIndex);
            transform.position = SetPosition(iconPosition);
        }
        private Vector3 SetPosition(float iconPosition)
        {
            var position = transform.position;
            return new Vector3(position.x, position.y, iconPosition - distanceFromCenter);
        }

        private void SetSkin()
        {
            if (transform.childCount <= 0)
                return;
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            var skin = GameOptionsSave.Instance.GetSkin(0);
            Instantiate(skin, transform);
        }
        /// <summary>
        /// Atualiza a posição do cursor ao receber o evento.
        /// </summary>
        private void HandleCursorMove(float targetZ)
        {
            MoveObject(targetZ, moveDuration, rotateDuration);
        }
        private void MoveObject(float targetZ, float duration, float timeRotate)
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
            var audioSfxEvent = AudioManager.instance.GetAudioSfxEvent(EnumSfxSound.SfxEngineAccelerate);
            // Create DoTween sequences for animation
            DOTween.Sequence()
                .OnStart(() =>
                {
                    audioSfxEvent.PlayOnShot(_audioSource);
                })
                .Append(gameObject.transform.DOMoveZ(targetZ, duration).SetEase(enterAnimation))  // Slow down at start (InQuad)
                .Join(gameObject.transform.DORotate(initialRotation, duration).SetEase(enterRotationAnimation))
                .Append(gameObject.transform.DORotate(finalRotation, timeRotate).SetEase(exitRotationAnimation))  // Speed up at end (OutQuad)
                .OnComplete(() =>
                {
                    audioSfxEvent.Stop(_audioSource);
                })
                .Play();
        }
        
    }
}
