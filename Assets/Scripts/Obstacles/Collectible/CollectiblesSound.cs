using UnityEngine;
namespace RiverAttack
{
    [RequireComponent(typeof(CollectiblesMaster))]
    public class CollectiblesSound : EnemiesSound
    {/*
        [SerializeField]
        private AudioEventSample collectSound;
        [SerializeField]
        private AudioEventSample showSound;
        CollectiblesMaster m_CollectiblesMaster;

        #region UNITY METHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            m_CollectiblesMaster.CollectibleEvent += CollectSound;
            m_CollectiblesMaster.ShowOnScreen += ShowSound;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            m_CollectiblesMaster.CollectibleEvent -= CollectSound;
            m_CollectiblesMaster.ShowOnScreen -= ShowSound;
        }
  #endregion

        void ShowSound()
        {
            if (audioSource != null && showSound != null)
                showSound.Play(audioSource);
        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_CollectiblesMaster = GetComponent<CollectiblesMaster>();
        }

        void CollectSound(PlayerMaster playerMaster)
        {
            if (audioSource != null && collectSound != null)
                collectSound.Play(audioSource);
        }*/
    }
}

