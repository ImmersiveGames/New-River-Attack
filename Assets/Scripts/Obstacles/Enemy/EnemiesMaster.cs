using System;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    public class EnemiesMaster : MonoBehaviour
    {
        public EnemiesScriptable enemy;
        public bool isDestroyed;
        public bool goalLevel;
        public bool ignoreWall;
        public bool ignoreEnemies;
        public Vector3 enemyStartPosition
        {
            get;
            private set;
        }

        private protected GamePlayManager gamePlayManager;

        #region Delegates
        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventDestroyEnemy;
        public event GeneralEventHandler EventChangeSkin;
        public delegate void MovementEventHandler(Vector3 pos);
        public event MovementEventHandler EventMovementEnemy;
        public event MovementEventHandler EventFlipEnemy;
        public delegate void EnemyEventHandler(PlayerMaster playerMaster);
        public event EnemyEventHandler EventPlayerDestroyEnemy;
  #endregion

        #region UNITY METHODS
        void Awake()
        {
            gameObject.name = enemy.name;

            enemyStartPosition = transform.position;
            isDestroyed = false;
        }
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            gamePlayManager.EventResetEnemies += OnInitializeEnemy;
        }
        protected virtual void OnDisable()
        {
            gamePlayManager.EventResetEnemies -= OnInitializeEnemy;
        }
  #endregion
        
        protected virtual void SetInitialReferences()
        {
            gamePlayManager = GamePlayManager.instance;
            Debug.Log(GameManager.instance.layerEnemies.value);
            int novoLayer = Mathf.RoundToInt(Mathf.Log(GameManager.instance.layerEnemies.value, 2));
            gameObject.layer = novoLayer;
        }

        void OnInitializeEnemy()
        {
            if (!enemy.canRespawn && isDestroyed)
                Destroy(gameObject, 0.1f);
            else
            {
                Utils.Tools.ToggleChildren(this.transform, true);
                transform.position = enemyStartPosition;
                isDestroyed = false;
            }
        }

    #region Calls
        public void CallEventDestroyEnemy()
        {
            EventDestroyEnemy?.Invoke();
        }
        public void CallEventMovementEnemy(Vector3 pos)
        {
            EventMovementEnemy?.Invoke(pos);
        }
        public void CallEventFlipEnemy(Vector3 pos)
        {
            EventFlipEnemy?.Invoke(pos);
        }
        public void CallEventDestroyEnemy(PlayerMaster playerMaster)
        {
            EventDestroyEnemy?.Invoke();
            EventPlayerDestroyEnemy?.Invoke(playerMaster);
        }

        public void CallEventChangeSkin()
        {
            EventChangeSkin?.Invoke();
        }
    #endregion
    }
}
