using UnityEngine;
namespace RiverAttack
{
    public class CollectiblesSound : EnemiesSound
    {
        [SerializeField] private AudioEventSample collectSound;
        [SerializeField] private AudioEventSample showSound;
        private CollectiblesMasterOld _mCollectiblesMasterOld;

        #region UNITY METHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            _mCollectiblesMasterOld.EventCollectItem += CollectSound;
        }

        private void OnBecameVisible()
        {
            ShowSound();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _mCollectiblesMasterOld.EventCollectItem -= CollectSound;
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
            _mCollectiblesMasterOld = GetComponent<CollectiblesMasterOld>();
        }

        private void CollectSound(PlayerSettings playerSettings)
        {
            if (audioSource != null && collectSound != null)
                collectSound.Play(audioSource);
        }
    }
}
