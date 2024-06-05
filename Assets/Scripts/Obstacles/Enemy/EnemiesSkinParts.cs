using UnityEngine;

namespace RiverAttack 
{
    public class EnemiesSkinParts : MonoBehaviour
    {
        // Identifica uma parte movel da skin
        private EnemiesSound m_EnemiesSound;
        private BossMasterOld _mBossMasterOld;

        private void Start()
        {
            m_EnemiesSound = GetComponentInParent<EnemiesSound>();
            _mBossMasterOld = GetComponentInParent<BossMasterOld>();
        }

        public void BossInvulnerabilityOn()
        {
            _mBossMasterOld.BossInvulnerability(true);
        }
        public void BossInvulnerabilityOff()
        {
            _mBossMasterOld.BossInvulnerability(false);
        }

        public void PlayExplosionSound()
        {
            m_EnemiesSound.ExplodeSound();
        }

    }    
}

