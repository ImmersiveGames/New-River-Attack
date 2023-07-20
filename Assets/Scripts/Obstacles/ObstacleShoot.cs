using UnityEngine;
using Utils;
namespace RiverAttack
{
    public abstract class ObstacleShoot : MonoBehaviour, IShoot
    {
        public GameObject prefab;
        public float bulletSpeedy;
        public float cadenceShoot;
        public bool holdShoot;
        Renderer m_MyRenderer;
#region UNITY METHODS
        protected virtual void OnEnable()
        {
            m_MyRenderer = GetComponentInChildren<Renderer>();
        }
  #endregion
        public void SetTarget(Transform toTarget) { }

        public virtual bool ShouldFire()
        {
            var gamePlayManager = GamePlayManager.instance;
            return gamePlayManager.shouldBePlayingGame && gamePlayManager.haveAnyPlayMasterBeReady && gameObject.activeInHierarchy && m_MyRenderer.isVisible && !holdShoot;
        }

        public void Fire() { }
    }
}
