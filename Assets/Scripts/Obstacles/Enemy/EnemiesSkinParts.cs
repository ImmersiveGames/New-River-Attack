using UnityEngine;

namespace RiverAttack 
{
    public class EnemiesSkinParts : MonoBehaviour
    {
        // Identifica uma parte movel da skin
        private EnemiesSound m_EnemiesSound;
        private BossMaster m_BossMaster;

        private void Start()
        {
            m_EnemiesSound = GetComponentInParent<EnemiesSound>();
            m_BossMaster = GetComponentInParent<BossMaster>();
        }

        public void BossInvulnerabilityOn()
        {
            m_BossMaster.BossInvulnerability(true);
        }
        public void BossInvulnerabilityOff()
        {
            m_BossMaster.BossInvulnerability(false);
        }

        public void PlayExplosionSound()
        {
            m_EnemiesSound.ExplodeSound();
        }

    }    
}

