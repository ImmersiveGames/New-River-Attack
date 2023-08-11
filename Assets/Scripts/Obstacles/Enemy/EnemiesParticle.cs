using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class EnemiesParticle : MonoBehaviour
    {
        /*[SerializeField]
        GameObject particlePrefab;
        [SerializeField]
        float timeoutDestroy;

        ObstacleMaster m_ObstacleMaster;

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_ObstacleMaster.EventDestroyObject += ParticleExplosion;
        }
        void OnDisable()
        {
            m_ObstacleMaster.EventDestroyObject -= ParticleExplosion;
        }
  #endregion

        void SetInitialReferences()
        {
            m_ObstacleMaster = GetComponent<ObstacleMaster>();
            // find children with Particles
        }

        void ParticleExplosion()
        {
            Tools.ToggleChildren(this.transform, false);
            var go = Instantiate(particlePrefab, transform);
            Destroy(go, timeoutDestroy);
        }*/
    }
}
