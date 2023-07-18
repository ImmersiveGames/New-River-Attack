using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class PlayerParticles : MonoBehaviour
    {

        [SerializeField]
        private GameObject particlePrefab;
        [SerializeField]
        private float timeoutDestroy;

        private PlayerMaster m_PlayerMaster;

        private void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerDestroy += ExplodeParticle;
            m_PlayerMaster.EventPlayerReload += RestoreChildren;
        }

        private void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            // find children with Particles
        }

        private void ExplodeParticle()
        {
            Tools.ToggleChildren(this.transform, false);
            GameObject go = Instantiate(particlePrefab, this.transform);
            Destroy(go, timeoutDestroy);
        }

        private void RestoreChildren()
        {
            Tools.ToggleChildren(this.transform, true);
        }

        private void OnDisable()
        {
            m_PlayerMaster.EventPlayerDestroy -= ExplodeParticle;
            m_PlayerMaster.EventPlayerReload -= RestoreChildren;
        }
    }
}
