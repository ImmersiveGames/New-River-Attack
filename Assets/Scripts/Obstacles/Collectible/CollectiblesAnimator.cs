using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(CollectiblesMaster))]
    public class CollectiblesAnimator : EnemiesAnimator
    {

        public string collectTrigger;
        CollectiblesMaster m_CollectiblesMaster;

        #region UNITY METHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            m_CollectiblesMaster.CollectibleEvent += CollectAnimation;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            m_CollectiblesMaster.CollectibleEvent -= CollectAnimation;
        }
  #endregion

        void CollectAnimation(PlayerMaster playerMaster)
        {
            //implementar animação de coletar
            RemoveAnimation();
        }
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_CollectiblesMaster = GetComponent<CollectiblesMaster>();
        }
    }
}
