using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    public class EffectAreaMaster : ObstacleMaster
    {
        float m_Timer;
        float m_TimeToAccess;
        EffectAreaScriptable m_EffectArea;

        readonly List<PlayerMaster> m_PlayerMasterColliderList = new List<PlayerMaster>();
        Collider m_Collider;
        Collider[] m_Colliders;

        #region Events
        public event GeneralEventHandler EventEnterAreaEffect;
        public event GeneralEventHandler EventExitAreaEffect;
  #endregion

        #region UNITYMETHODS
        void Start()
        {
            m_Collider = GetComponentInChildren<Collider>();
            m_Colliders = new Collider[5];
        }
        internal override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            playerMaster = other.GetComponentInParent<PlayerMaster>();
            if(playerMaster == null) return;
            m_PlayerMasterColliderList.Add(playerMaster);
        }

        void Update()
        {
            if (m_PlayerMasterColliderList.Count < 1) return;
            if (!playerMaster.inEffectArea) return;
            for (int i = m_PlayerMasterColliderList.Count - 1; i >= 0; i--)
            {
                if (StillCollider(m_PlayerMasterColliderList[i]))
                    continue;
                OnEventExitAreaEffect();
                m_PlayerMasterColliderList.RemoveAt(i);
            }
        }
        void OnTriggerExit(Collider collision)
        {
            if(playerMaster == null)
                playerMaster = collision.GetComponentInParent<PlayerMaster>();
            if (!playerMaster || !playerMaster.inEffectArea) return;
            playerMaster.inEffectArea = false;
            if (!playerMaster.shouldPlayerBeReady) return;
            OnEventExitAreaEffect();
        }
        void OnTriggerStay(Collider collision)
        {
            if(playerMaster == null)
                playerMaster = collision.GetComponentInParent<PlayerMaster>();
            if (!playerMaster || !playerMaster.shouldPlayerBeReady) return;
            if (!playerMaster.inEffectArea) playerMaster.inEffectArea = true;
            CollectThis(playerMaster);
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_EffectArea = enemy as EffectAreaScriptable;
            if (m_EffectArea != null)
                m_TimeToAccess = m_EffectArea.timeToAccess;
        }
        void CollectThis(PlayerMaster collision)
        {
            var player = collision.getPlayerSettings;
            if (m_Timer <= 0 && playerMaster.inEffectArea)
            {
                m_EffectArea.EffectAreaStart(player);
                OnEventAreaEffect();
                m_Timer = m_TimeToAccess;
            }
            m_Timer -= Time.deltaTime;
        }

        bool StillCollider(PlayerMaster pMaster)
        {
            var sizeCollider = m_Collider.bounds.size;
            float radio = Mathf.Max(sizeCollider.x, sizeCollider.y, sizeCollider.z) / 2.0f;

            // Executa o overlap com base no raio calculado
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radio, m_Colliders,GameManager.instance.layerPlayer);
            //Debug.Log($"Numero de colidder: {numColliders}");
            for (int i = 0; i < numColliders; i++)
            {
                var colliderPlayerMaster = m_Colliders[i].gameObject.GetComponentInParent<PlayerMaster>();
                if (colliderPlayerMaster == pMaster)
                {
                    return true; // O objeto ainda está colidindo
                }
            }
            return false; // O objeto não está mais colidindo
        }

        protected override void DestroyObstacle()
        {
            if (playerMaster.inEffectArea)
            {
                playerMaster.inEffectArea = false;
                //OnEventExitAreaEffect();
            }
            OnEventExitAreaEffect();
            base.DestroyObstacle();
        }

        #region Calls
        void OnEventAreaEffect()
        {
            EventEnterAreaEffect?.Invoke();
        }

        void OnEventExitAreaEffect()
        {
            EventExitAreaEffect?.Invoke();
        }
  #endregion

    }
}
