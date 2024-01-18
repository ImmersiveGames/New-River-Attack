using UnityEngine;

namespace RiverAttack 
{
    public class EnemiesSkinParts : MonoBehaviour
    {
        // Identifica uma parte movel da skin
        EnemiesSound m_EnemiesSound;
        BossMaster m_BossMaster;

        void Start()
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

