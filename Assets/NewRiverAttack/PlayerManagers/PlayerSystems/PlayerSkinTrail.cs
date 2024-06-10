using System.Collections;
using System.Collections.Generic;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerSkinTrail : MonoBehaviour
    {
        private const float RangeAxisY = 0.2f;
        private const float MaxTrailTime = 1.0f;
        private const float FadeTime = 1.0f;

        private bool _onAccelerate;
        private bool _displayTrail;

        private PlayerMaster _playerMaster;
        private TrailRenderer[] _trailRenderers;
        private readonly List<Coroutine> _coroutineInList = new List<Coroutine>();
        private readonly List<Coroutine> _coroutineOutList = new List<Coroutine>();

        private void OnEnable()
        {
            SetInitialReferences();
            InitializeTrails(null);
            _playerMaster.EventPlayerMasterAxisMovement += ActiveTrailsOnMovement;
            _playerMaster.EventPlayerMasterChangeSkin += InitializeTrails;
            GamePlayManager.instance.EventGameFinisher += ActiveTrails;
        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterAxisMovement -= ActiveTrailsOnMovement;
            _playerMaster.EventPlayerMasterChangeSkin -= InitializeTrails;
            GamePlayManager.instance.EventGameFinisher -= ActiveTrails;
        }

        private void InitializeTrails(ShopProductSkin productSkin)
        {
            _trailRenderers = GetComponentsInChildren<TrailRenderer>(true);
            foreach (var trail in _trailRenderers)
            {
                trail.time = 0f;
                trail.gameObject.SetActive(false);
            }
        }

        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
        }

        private void ActiveTrails()
        {
            Debug.Log("ASAS");
            foreach (var trail in _trailRenderers)
            {
                trail.time = MaxTrailTime;
                trail.gameObject.SetActive(true);
            }
        }

        private void ActiveTrailsOnMovement(Vector2 dir)
        {
            if (!_playerMaster.ObjectIsReady || _trailRenderers == null || _playerMaster.InFinishPath) return;

            _onAccelerate = dir.y > RangeAxisY;

            switch (_onAccelerate)
            {
                case true when !_displayTrail:
                {
                    // Mostrar Rastro
                    foreach (var trail in _trailRenderers)
                    {
                        _coroutineInList.Add(StartCoroutine(FadeInTrail(trail)));
                    }
                    _displayTrail = true;
                    break;
                }
                case false when _displayTrail:
                {
                    // Esconder Rastro
                    foreach (var trail in _trailRenderers)
                    {
                        _coroutineOutList.Add(StartCoroutine(FadeOutTrail(trail)));
                    }
                    _displayTrail = false;
                    break;
                }
            }
        }

        private void RemoveCoroutine(List<Coroutine> coroutines)
        {
            if (coroutines.Count <= 0) return;
            foreach (var coroutine in coroutines)
            {
                StopCoroutine(coroutine);
            }
            coroutines.Clear();
        }

        private IEnumerator FadeInTrail(TrailRenderer trailRenderer)
        {
            trailRenderer.time = 0;
            _displayTrail = true;
            trailRenderer.gameObject.SetActive(true);
            yield return FadeTrail(trailRenderer, FadeTime, MaxTrailTime);
            
        }
        private IEnumerator FadeOutTrail(TrailRenderer trailRenderer)
        {
            trailRenderer.time = MaxTrailTime;
            yield return FadeTrail(trailRenderer, FadeTime, 0);
            _displayTrail = false;
            trailRenderer.gameObject.SetActive(false);
            
        }

        private IEnumerator FadeTrail(TrailRenderer trailRenderer, float fadeDuration, float targetTime)
        {
            var initialTime = trailRenderer.time;
            var elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                trailRenderer.time = Mathf.Lerp(initialTime, targetTime, elapsedTime / fadeDuration);
                yield return null;
            }
            trailRenderer.time = targetTime;
        }
    }
}
