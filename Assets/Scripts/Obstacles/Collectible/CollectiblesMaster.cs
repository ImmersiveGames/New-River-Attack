using System.Collections.Generic;
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
            ComponentToCollect(other.GetComponentInParent<PlayerMaster>());
        }
  #endregion

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            collectibleScriptable = enemy as CollectibleScriptable;
        }

        private void ComponentToCollect(Component other)
        {
            if (other == null) return;
            var whoHit = WhoHit(other);
            // pode coletar?
            var collectableList = whoHit.collectableList;
            if (collectableList.Count != 0)
            {
                var results = collectableList.Find(x => x.collectable == collectibleScriptable);
                if (collectibleScriptable.maxCollectible > 0 && results.quantity >= collectibleScriptable.maxCollectible)
                {
                    OnEventObstacleMaxCollectReached(collectibleScriptable);
                    return;
                }
                // Ja tem algo na lista
            }
            if(collectibleScriptable == gamePlayManager.refillBomb && whoHit.getPlayerSettings.bombs >= GameSettings.instance.maxBombs)
                return;
            
            OnEventCollectItem(whoHit.getPlayerSettings);
            ShouldSavePoint(whoHit.getPlayerSettings);
            AddCollectList(collectableList, collectibleScriptable, collectibleScriptable.amountCollectables);
            if(collectibleScriptable.powerUp == null)
                CollectWealth(whoHit.getPlayerSettings, collectibleScriptable.amountCollectables);
            GamePlayManager.AddResultList(gamePlayingLog.GetEnemiesResult(), whoHit.getPlayerSettings, enemy, collectibleScriptable.amountCollectables, CollisionType.Collected);
        }

        private static void AddCollectList(List<LogPlayerCollectables> list, CollectibleScriptable collectible, int qnt)
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

        private void CollectWealth(PlayerSettings playerSettings, int collect)
        {
            playerSettings.wealth += collect;
            GameSteamManager.SetStat("stat_CollectRefugee", playerSettings.wealth, true);
            gamePlayManager.OnEventUpdateRefugees(playerSettings.wealth);
        }

        private void CollectObstacle()
        {
            isDestroyed = true;
            isActive = false;
        }

        protected void ToggleChildren(bool active)
        {
            Tools.ToggleChildren(transform, active);
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
