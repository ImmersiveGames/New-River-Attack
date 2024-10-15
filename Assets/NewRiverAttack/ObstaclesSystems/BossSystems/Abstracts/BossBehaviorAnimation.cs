using NewRiverAttack.PlayerManagers.Tags;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Abstracts
{
    public abstract class BossBehaviorAnimation : MonoBehaviour
    {
        private SkinAttach _skin;
        protected BossMaster BossMaster;
        protected Animator Animator;
        #region Unity Methods

        protected virtual void Awake()
        {
            BossMaster = GetComponent<BossMaster>();
            UpdateSkin();
        }
        private void OnEnable()
        {
            BossMaster.EventObstacleChangeSkin += UpdateSkin;
        }

        private void OnDisable()
        {
            BossMaster.EventObstacleChangeSkin -= UpdateSkin;
        }

        #endregion
        private void UpdateSkin()
        {
            _skin = BossMaster.GetComponentInChildren<SkinAttach>();
            Animator = _skin.GetComponent<Animator>();
        }
        protected void Invulnerability(bool active)
        {
            if (_skin == null) return;
            var colliders = _skin.GetComponentsInChildren<Collider>();
            for (var i = colliders.Length - 1; i >= 0; i--)
            {
                colliders[i].enabled = !active;
            }
        }
    }
}