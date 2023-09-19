using UnityEngine;
namespace RiverAttack
{
    public class CollectiblesSound : EnemiesSound
    {
        [SerializeField]
        AudioEventSample collectSound;
        [SerializeField]
        AudioEventSample showSound;
        CollectiblesMaster m_CollectiblesMaster;

        #region UNITY METHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            m_CollectiblesMaster.EventCollectItem += CollectSound;
        }
        void OnBecameVisible()
        {
            ShowSound();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            m_CollectiblesMaster.EventCollectItem -= CollectSound;
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

        void CollectSound(PlayerSettings playerSettings)
        {
            if (audioSource != null && collectSound != null)
                collectSound.Play(audioSource);
        }
    }
}
