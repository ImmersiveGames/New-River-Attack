using UnityEngine;
using Utils;

namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster))]
    public class EnemiesParticle : MonoBehaviour
    {
        [SerializeField]
        GameObject particlePrefab;
        [SerializeField]
        float timeoutDestroy;

        EnemiesMaster m_EnemiesMaster;

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_EnemiesMaster.EventDestroyEnemy += ParticleExplosion;
        }
        void OnDisable()
        {
            m_EnemiesMaster.EventDestroyEnemy -= ParticleExplosion;
        }
  #endregion

        void SetInitialReferences()
        {
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
            // find children with Particles
        }

        void ParticleExplosion()
        {
            Tools.ToggleChildren(this.transform, false);
            var go = Instantiate(particlePrefab, transform);
            Destroy(go, timeoutDestroy);
        }
    }
}
