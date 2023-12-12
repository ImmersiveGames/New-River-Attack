using System;
using UnityEngine;
namespace RiverAttack
{
    public class BossAnimator: MonoBehaviour
    {
        public string onEmerge;

        BossMaster m_BossMaster;
        Animator m_Animator;
        
        GamePlayManager m_GamePlayManager;

        void OnEnable()
        {
            SetInitialReferences();
            m_BossMaster.EventBossEmerge += AnimateEmerge;
        }

        void OnDisable()
        {
            m_BossMaster.EventBossEmerge -= AnimateEmerge;
        }

        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_BossMaster = GetComponent<BossMaster>();
            m_Animator = GetComponentInChildren<Animator>();
        }

        void AnimateEmerge()
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();
            if (string.IsNullOrEmpty(onEmerge)) 
                return;
            m_Animator.SetTrigger(onEmerge);
        }
    }
}
