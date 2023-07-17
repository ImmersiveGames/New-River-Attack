using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    
    public abstract class ObstacleShoot : MonoBehaviour, IShoot
    {
        public GameObject prefab;
        public float bulletSpeedy;
        public float cadencyShoot;
        public bool holdShoot;    
        protected Renderer myrenderer;

        protected virtual void OnEnable()
        {
            myrenderer = GetComponentInChildren<Renderer>();       
        }

        public void SetTarget(Transform toTarget){}

        public virtual bool ShouldFire()
        {
            if (GamePlayManager.instance.shouldBePlayingGame && GamePlayManager.instance.shouldPlayReady && gameObject.activeInHierarchy && myrenderer.isVisible && !holdShoot)
                return true;
            return false;
        }

        public void Fire(){}
    }
}
