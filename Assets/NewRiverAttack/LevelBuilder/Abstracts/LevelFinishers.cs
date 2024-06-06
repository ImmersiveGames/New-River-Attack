using System;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.LevelBuilder.Abstracts
{
    public abstract class LevelFinishers : MonoBehaviour
    {
        protected bool InFinisher;
        protected GamePlayManager GamePlayManagerRef;
        protected Vector3 GetTilePosition { get; private set; }

        protected virtual void OnEnable()
        {
            SetInitialReferences();
        }

        protected virtual void Start()
        {
            GetTilePosition = gameObject.transform.parent.localPosition;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            var playerMaster = other.GetComponentInParent<PlayerMaster>();
            if( playerMaster == null || playerMaster.IsDisable || InFinisher) return;
            InFinisher = true;
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            var playerMaster = other.GetComponentInParent<PlayerMaster>();
            if( playerMaster == null || playerMaster.IsDisable || !InFinisher) return;
            InFinisher = false;
            
        }
        
        protected virtual void SetInitialReferences()
        {
            GamePlayManagerRef = GamePlayManager.instance;
        }

        
    }
}