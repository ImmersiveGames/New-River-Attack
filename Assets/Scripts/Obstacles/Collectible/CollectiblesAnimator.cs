using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class CollectiblesAnimator : MonoBehaviour
    {
        [SerializeField] string collectTrigger;
        [SerializeField] float timeCollectAnimation;

        CollectiblesMaster m_CollectiblesMaster;
        Animator m_Animator;

        #region UNITY METHODS
        protected void OnEnable()
        {
            SetInitialReferences();
            m_CollectiblesMaster.EventCollectItem += CollectAnimation;
        }
        void OnDisable()
        {
            m_CollectiblesMaster.EventCollectItem -= CollectAnimation;
        }
  #endregion

        void CollectAnimation(PlayerSettings playerSettings)
        {
            if (m_Animator == null)
            {
                m_Animator = GetComponentInChildren<Animator>();
            }
            if (m_Animator == null || string.IsNullOrEmpty(collectTrigger)) return;
            m_Animator.SetTrigger(collectTrigger);
            Invoke(nameof(DisableChildren), timeCollectAnimation);

        }
        void SetInitialReferences()
        {
            m_CollectiblesMaster = GetComponent<CollectiblesMaster>();
            m_Animator = GetComponentInChildren<Animator>();

        }
        protected void DisableChildren()
        {
            Tools.ToggleChildren(transform, false);
        }
    }
}
