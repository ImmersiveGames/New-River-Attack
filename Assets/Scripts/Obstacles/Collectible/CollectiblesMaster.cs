using UnityEngine;
namespace RiverAttack
{

    public class CollectiblesMaster : EnemiesMaster
    {
        public event GeneralEventHandler ShowOnScreen;
        public event EnemyEventHandler CollectibleEvent;
        internal CollectibleScriptable collectibles { get; set; }

    #region UNITYMETHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            m_GamePlayManager.EventResetEnemies += DestroyCollectable;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            m_GamePlayManager.EventResetEnemies -= DestroyCollectable;
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            //this.tag = GameSettings.instance.collectionTag;
            this.gameObject.layer = LayerMask.NameToLayer("Collections");
            collectibles = (CollectibleScriptable)enemy;
            if (collectibles.getPowerUp)
            {
                name += "(" + collectibles.getPowerUp.name + ")";
            }
        }

        public void CallCollectibleEvent(PlayerMaster playerMaster)
        {
            CollectibleEvent?.Invoke(playerMaster);
        }
        public void CallShowOnScreen()
        {
            ShowOnScreen?.Invoke();
        }
        void DestroyCollectable()
        {
            if (collectibles.getPowerUp)
                Destroy(this.gameObject);
        }

    }
}
