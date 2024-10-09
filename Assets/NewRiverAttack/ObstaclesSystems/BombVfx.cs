using ImmersiveGames.CameraManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems
{
    public class BombVfx : MonoBehaviour
    {
        [SerializeField]private ParticleSystem particle;

        private SphereCollider _collider;
        private float _startRadius;
        private float _targetRadius;
        private float _expansionDuration;
        private float _timerParam;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
        }

        private void OnDisable()
        {
            FinishExplosion();
        }

        private void FixedUpdate()
        {
            if (_collider != null && _collider.enabled)
            {
                UpdateExpansion();
                // Verifica se o tempo de expansão terminou
                if (_timerParam >= _expansionDuration)
                {
                    FinishExplosion();
                }
            }
            
            if (particle.isStopped)
            {
                DestroyImmediate(gameObject);
            }
        }

        public void InitializeBomb(float targetRadius, float expansionDuration, float shakeForce, float shakeTime)
        {
            if (_collider == null) return;
            _collider.enabled = true;
            _startRadius = _collider.radius;
            _targetRadius = targetRadius;
            _expansionDuration = expansionDuration;
            _timerParam = 0f;
            CameraShake.Instance?.ShakeCamera(shakeForce, shakeTime);
        }
        
        private void UpdateExpansion()
        {
            _timerParam += Time.deltaTime;
            var normalizedTime = Mathf.Clamp01(_timerParam / _expansionDuration);
            _collider.radius = Mathf.Lerp(_startRadius, _targetRadius, normalizedTime);
        }
        // Finaliza o processo de explosão
        private void FinishExplosion()
        {
            CameraShake.Instance?.StopShake();
            if (_collider == null) return;
            _collider.radius = _startRadius;
            _collider.enabled = false;
            _timerParam = 0f;
        }
    }
}