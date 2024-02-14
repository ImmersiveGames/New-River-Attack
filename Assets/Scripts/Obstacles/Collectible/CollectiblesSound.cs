using UnityEngine;
namespace RiverAttack
{
    public class CollectiblesSound : EnemiesSound
    {
        [SerializeField] private AudioEventSample collectSound;
        [SerializeField] private AudioEventSample showSound;
        private CollectiblesMaster m_CollectiblesMaster;

        #region UNITY METHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            m_CollectiblesMaster.EventCollectItem += CollectSound;
        }

        private void OnBecameVisible()
        {
            ShowSound();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            m_CollectiblesMaster.EventCollectItem -= CollectSound;
        }
  #endregion

  private void ShowSound()
        {
            if (audioSource != null && showSound != null)
                showSound.Play(audioSource);
        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_CollectiblesMaster = GetComponent<CollectiblesMaster>();
        }

        private void CollectSound(PlayerSettings playerSettings)
        {
            if (audioSource != null && collectSound != null)
                collectSound.Play(audioSource);
        }
    }
}
