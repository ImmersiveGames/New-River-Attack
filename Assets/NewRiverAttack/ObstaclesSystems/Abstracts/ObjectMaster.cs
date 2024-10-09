﻿using ImmersiveGames.DebugManagers;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.Abstracts
{
    public abstract class ObjectMaster: MonoBehaviour
    {
        protected internal bool IsDead;
        protected internal bool IsDisable;
        private Vector3 _startPosition;
        private Vector3 _startScale;
        private Quaternion _startRotation;

        private Vector3 _savePosition;
        protected GamePlayManager GamePlayManagerRef;

        
        public bool ObjectIsReady => !IsDead && !IsDisable && GamePlayManagerRef.ShouldBePlayingGame;   
        
        #region Unity Methods
        

        protected virtual void OnEnable()
        {
            SetInitialReferences();
            GamePlayManagerRef.EventPostStateGameInitialize += InitializeObject;
            GamePlayManagerRef.EventGameReady += InitializeObject;
            DebugManager.Log<ObjectMaster>($"Enable - {gameObject.name}");
            IsDisable = true;
            IsDead = false;
        }

        private void Start()
        {
            SetInitialPosition();
            DebugManager.Log<ObjectMaster>($"Start - {gameObject.name}");
        }

        protected virtual void OnDisable()
        {
            GamePlayManagerRef.EventPostStateGameInitialize -= InitializeObject;
            GamePlayManagerRef.EventGameReady -= InitializeObject;
        }

        #endregion
        protected internal void InitializeObject()
        {
            IsDisable = false;
            DebugManager.Log<ObjectMaster>($"{transform.position.z}");
        }

        private void SetInitialReferences()
        {
            GamePlayManagerRef = GamePlayManager.Instance;
        }
        
        private void SetInitialPosition()
        {
            var transform1 = transform;
            _startPosition = transform1.position;
            _startRotation = transform1.rotation;
            _startScale = transform1.localScale;
            SavePosition(_startPosition);
        }
        protected void RepositionObject()
        {
            var transform1 = transform;
            transform1.position = _savePosition;
            transform1.rotation = _startRotation;
            transform1.localScale = _startScale;
        }

        protected float GetLastPositionZ => _savePosition.z;
        protected float GetLastPositionX => _savePosition.x;
        protected internal void SavePosition(Vector3 myPosition)
        {
            _savePosition = myPosition;
        }
    }
}