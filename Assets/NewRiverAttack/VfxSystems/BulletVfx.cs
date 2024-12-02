using System;
using UnityEngine;

namespace NewRiverAttack.VfxSystems
{
    public class BulletVfx : MonoBehaviour
    {
        private float _lifeTime = 1f;
        [SerializeField]private ParticleSystem particle;

        private void OnEnable()
        {
            _lifeTime = particle.main.duration;
        }

        private void Start()
        {
            Destroy(gameObject,_lifeTime);
        }
    }
}