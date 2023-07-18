using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

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

        private protected GamePlayManager m_GamePlayManager;

        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventDestroyEnemy;
        public event GeneralEventHandler EventChangeSkin;

        public delegate void MovementEventHandler(Vector3 pos);
        public event MovementEventHandler EventMovementEnemy;
        public event MovementEventHandler EventFlipEnemy;

        public delegate void EnemyEventHandler(PlayerMaster playerMaster);
        public event EnemyEventHandler EventPlayerDestroyEnemy;

        private void Awake()
        {
            gameObject.name = enemy.name;

            enemyStartPosition = transform.position;
            isDestroyed = false;
        }

        protected virtual void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventResetEnemies += OnInitializeEnemy;
        }

        protected virtual void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            //Debug.Log(GameSettings.Instance.enemyTag);
            //gameObject.tag = GameSettings.instance.enemyTag;
            //gameObject.layer = GameSettings.instance.layerEnemies;
        }

        void OnInitializeEnemy()
        {
            if (!enemy.canRespawn && isDestroyed)
                Destroy(this.gameObject, 0.1f);
            else
            {
                Utils.Tools.ToggleChildren(this.transform, true);
                transform.position = enemyStartPosition;
                isDestroyed = false;
            }

        }

        public void SetTagLayer(IEnumerable<GameObject> objects, string myTag, int myLayer)
        {
            foreach (var t in objects)
            {
                t.tag = myTag;
                t.layer = myLayer;
            }
        }

        protected virtual void OnDisable()
        {
            m_GamePlayManager.EventResetEnemies -= OnInitializeEnemy;
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
