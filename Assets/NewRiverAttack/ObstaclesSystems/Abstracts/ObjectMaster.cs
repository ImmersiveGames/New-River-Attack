using ImmersiveGames.DebugManagers;
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
            GamePlayManagerRef.EventGameReady += ReadyObject;
            IsDisable = true;
            IsDead = false;
        }

        private void Start()
        {
            SetInitialPosition();
        }

        protected virtual void OnDisable()
        {
            GamePlayManagerRef.EventPostStateGameInitialize -= InitializeObject;
            GamePlayManagerRef.EventGameReady -= ReadyObject;
        }

        #endregion
        protected internal void InitializeObject()
        {
            IsDisable = false;
            DebugManager.Log<ObjectMaster>($"Initialize: {transform.position.z}");
        }

        private void ReadyObject()
        {
            IsDisable = false;
        }

        protected virtual void SetInitialReferences()
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
        protected internal virtual void SavePosition(Vector3 myPosition)
        {
            _savePosition = myPosition;
        }
        
        
    }
}