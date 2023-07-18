using UnityEngine;
namespace RiverAttack
{
    
    [RequireComponent(typeof(CollectiblesMaster))]
    public class CollectiblesSound : EnemiesSound
    {
        [SerializeField]
        private AudioEventSample collectSound;
        [SerializeField]
        private AudioEventSample showSound;
        protected CollectiblesMaster collectiblesMaster;

        protected override void OnEnable()
        {
            base.OnEnable();
            collectiblesMaster.CollectibleEvent += CollectSound;
            collectiblesMaster.ShowOnScreen += ShowSound;
        }

        private void ShowSound()
        {
            if (m_AudioSource != null && showSound != null)
                showSound.Play(m_AudioSource);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            collectiblesMaster.CollectibleEvent -= CollectSound;
            collectiblesMaster.ShowOnScreen -= ShowSound;
        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            collectiblesMaster = GetComponent<CollectiblesMaster>();
        }

        private void CollectSound(PlayerMaster playerMaster)
        {
            if (m_AudioSource != null && collectSound != null)
                collectSound.Play(m_AudioSource);
        }
    }
}

