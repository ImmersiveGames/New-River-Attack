using UnityEngine;
using Utils;
namespace RiverAttack
{
    public abstract class ObstacleShoot : ObstacleDetectApproach, IShoot
    {
        public GameObject prefab;
        public float bulletSpeedy;
        public float cadenceShoot;
        public bool holdShoot;
        Renderer m_MyRenderer;
        GamePlayManager m_GamePlayManager;
        protected virtual void SetInitialReferences()
        {
            m_MyRenderer = GetComponentInChildren<Renderer>();
            m_GamePlayManager = GamePlayManager.instance;
        }
        public void SetTarget(Transform toTarget)
        {
            throw new System.NotImplementedException();
        }

        public virtual bool ShouldFire()
        {
            return m_GamePlayManager.shouldBePlayingGame && m_GamePlayManager.haveAnyPlayMasterBeReady && gameObject.activeInHierarchy && m_MyRenderer.isVisible && !holdShoot;
        }
        public void Fire()
        {
            throw new System.NotImplementedException();
        }
    }
}
