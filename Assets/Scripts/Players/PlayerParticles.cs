using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class PlayerParticles : MonoBehaviour
    {

        [SerializeField]
        GameObject particlePrefab;
        [SerializeField]
        float timeoutDestroy;

        PlayerMaster m_PlayerMaster;

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerDestroy += ExplodeParticle;
            m_PlayerMaster.EventPlayerReload += RestoreChildren;
        }
        void OnDisable()
        {
            m_PlayerMaster.EventPlayerDestroy -= ExplodeParticle;
            m_PlayerMaster.EventPlayerReload -= RestoreChildren;
        }
  #endregion
        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            // find children with Particles
        }

        void ExplodeParticle()
        {
            Tools.ToggleChildren(this.transform, false);
            var go = Instantiate(particlePrefab, transform);
            Destroy(go, timeoutDestroy);
        }

        void RestoreChildren()
        {
            Tools.ToggleChildren(transform, true);
        }

        
    }
}
