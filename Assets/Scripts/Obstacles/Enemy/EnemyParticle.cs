﻿using UnityEngine;
using Utils;

namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster))]
    public class EnemyParticle : MonoBehaviour
    {
        [SerializeField]
        private GameObject particlePrefab;
        [SerializeField]
        private float timeoutDestroy;
        private EnemiesMaster m_EnemiesMaster;

        private void OnEnable()
        {
            SetInitialReferences();
            m_EnemiesMaster.EventDestroyEnemy += ParticleExplosion;
        }

        private void SetInitialReferences()
        {
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
            // find children with Particles
        }

        private void ParticleExplosion()
        {
            Tools.ToggleChildrens(this.transform, false);
            GameObject go = Instantiate(particlePrefab, transform);
            Destroy(go, timeoutDestroy);
        }

        private void OnDisable()
        {
            m_EnemiesMaster.EventDestroyEnemy -= ParticleExplosion;
        }
    }
}
