using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class CollectiblesMaster : ObstacleMaster
    {
        internal CollectibleScriptable collectibleScriptable;

        #region Delagates
        protected internal event PlayerSettingsEventHandler EventCollectItem;
        public delegate void CollectableEventHandler(CollectibleScriptable collectable);
        protected internal event CollectableEventHandler EventObstacleMaxCollectReached;
  #endregion

        #region UNITYMETHODS
        
        internal override void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Bullets>())
            {
                base.OnTriggerEnter(other);
                return;
            }
            ComponentToCollect(other.GetComponentInParent<PlayerMaster>(), CollisionType.Collected);
        }
        
  #endregion
        
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            collectibleScriptable = enemy as CollectibleScriptable;
        }

        void ComponentToCollect(Component other, CollisionType collisionType)
        {
            if (other == null) return;
            var playerMaster = WhoHit(other);
            // pode coletar?
            var collectableList = playerMaster.collectableList;
            var results = collectableList.Find(x => x.collectable == collectibleScriptable);

            if (results != null && collectibleScriptable.maxCollectible != 0 && results.quantity >= collectibleScriptable.maxCollectible)
            {
                OnEventObstacleMaxCollectReached(collectibleScriptable);
                return;
            }
            OnEventCollectItem(playerMaster.getPlayerSettings);
            ShouldSavePoint(playerMaster.getPlayerSettings);
            AddCollectList(collectableList, collectibleScriptable, collectibleScriptable.amountCollectables);
            CollectWealth(playerMaster.getPlayerSettings, collectibleScriptable.amountCollectables);
            GamePlayManager.AddResultList(gamePlaySettings.hitEnemiesResultsList, playerMaster.getPlayerSettings, enemy,collectibleScriptable.amountCollectables, CollisionType.Collected);
        }
        static void AddCollectList(List<LogPlayerCollectables> list, CollectibleScriptable collectible, int qnt)
        {
            var itemResults = list.Find(x => x.collectable == collectible);
            if (itemResults != null)
            {
                if (collectible.maxCollectible == 0 || itemResults.quantity + qnt < collectible.maxCollectible)
                    itemResults.quantity += qnt;
                else
                    itemResults.quantity = collectible.maxCollectible;
            }
            else
            {
                var newItemResults = new LogPlayerCollectables(collectible, qnt);
                list.Add(newItemResults);
            }
        }

        void CollectWealth(PlayerSettings playerSettings, int collect)
        {
            playerSettings.wealth += collect;
            gamePlayManager.OnEventUpdateRefugees(playerSettings.wealth);
        }

        void CollectObstacle()
        {
            isDestroyed = true;
            isActive = false;
            Tools.ToggleChildren(transform, false);
        }

        #region Calls
        protected virtual void OnEventCollectItem(PlayerSettings playerSettings)
        {
            CollectObstacle();
            EventCollectItem?.Invoke(playerSettings);
        }
        protected virtual void OnEventObstacleMaxCollectReached(CollectibleScriptable collectable)
        {
            EventObstacleMaxCollectReached?.Invoke(collectable);
        }
  #endregion
        
        /*internal CollectibleScriptable collectibleScriptable;
        public event GeneralEventHandler ShowOnScreen;
        public event EnemyEventHandler CollectibleEvent;

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
        }*/

        
    }

    [System.Serializable]
    public class LogPlayerCollectables
    {
        public CollectibleScriptable collectable;
        public int quantity;
        public LogPlayerCollectables(CollectibleScriptable collectable, int quantity)
        {
            this.collectable = collectable;
            this.quantity = quantity;
        }
    }
}
