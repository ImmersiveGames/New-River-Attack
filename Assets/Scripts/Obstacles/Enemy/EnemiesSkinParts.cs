using UnityEngine;

namespace RiverAttack 
{
    public class EnemiesSkinParts : MonoBehaviour
    {
        // Identifica uma parte movel da skin
        EnemiesSound enemiesSound;

        void Start()
        {
            enemiesSound = GetComponentInParent<EnemiesSound>();
        }

        public void PlayExplosionSound()
        {
            enemiesSound.ExplodeSound();
        }

    }    
}

