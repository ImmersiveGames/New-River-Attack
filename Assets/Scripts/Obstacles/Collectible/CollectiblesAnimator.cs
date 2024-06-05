using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class CollectiblesAnimator : MonoBehaviour
    {
        [SerializeField] private string collectTrigger;
        [SerializeField] private float timeCollectAnimation;

        private CollectiblesMasterOld _mCollectiblesMasterOld;
        private Animator m_Animator;

        #region UNITY METHODS
        protected void OnEnable()
        {
            SetInitialReferences();
            _mCollectiblesMasterOld.EventCollectItem += CollectAnimation;
        }

        private void OnDisable()
        {
            _mCollectiblesMasterOld.EventCollectItem -= CollectAnimation;
        }
  #endregion

  private void CollectAnimation(PlayerSettings playerSettings)
        {
            if (m_Animator == null)
            {
                m_Animator = GetComponentInChildren<Animator>();
            }
            if (m_Animator == null || string.IsNullOrEmpty(collectTrigger)) return;
            m_Animator.SetTrigger(collectTrigger);
            Invoke(nameof(DisableChildren), timeCollectAnimation);

        }

        private void SetInitialReferences()
        {
            _mCollectiblesMasterOld = GetComponent<CollectiblesMasterOld>();
            m_Animator = GetComponentInChildren<Animator>();

        }
        protected void DisableChildren()
        {
            Tools.ToggleChildren(transform, false);
        }
    }
}
