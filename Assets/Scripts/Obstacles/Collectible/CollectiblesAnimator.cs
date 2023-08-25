using UnityEngine;

namespace RiverAttack
{
    public class CollectiblesAnimator : MonoBehaviour
    {
        [SerializeField] string collectTrigger;

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
            //TODO:implementar animação de coletar
            if (m_Animator == null)
            {
                m_Animator = GetComponentInChildren<Animator>();
            }
            if (m_Animator != null && !string.IsNullOrEmpty(collectTrigger))
                m_Animator.SetTrigger(collectTrigger);
        }
        void SetInitialReferences()
        {
            m_CollectiblesMaster = GetComponent<CollectiblesMaster>();
            m_Animator = GetComponentInChildren<Animator>();

        }
    }
}
