﻿using UnityEngine;
namespace RiverAttack
{
    public class CollectiblesMaster : EnemiesMaster
    {
        [SerializeField] CollectibleScriptable collectibleScriptable;
        public event GeneralEventHandler ShowOnScreen;
        public event EnemyEventHandler CollectibleEvent;
        internal CollectibleScriptable collectibles
        {
            get
            {
                return collectibleScriptable;
            }
            private set
            {
                collectibleScriptable = value;
            }
        }

    #region UNITYMETHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            gamePlayManager.EventResetEnemies += DestroyCollectable;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            gamePlayManager.EventResetEnemies -= DestroyCollectable;
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            gameObject.layer = LayerMask.NameToLayer("Collections");
            collectibleScriptable = (CollectibleScriptable)enemy;
            if (collectibleScriptable.getPowerUp)
            {
                name += "(" + collectibleScriptable.getPowerUp.name + ")";
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
            if (collectibleScriptable.getPowerUp)
                Destroy(gameObject);
        }
    }
}