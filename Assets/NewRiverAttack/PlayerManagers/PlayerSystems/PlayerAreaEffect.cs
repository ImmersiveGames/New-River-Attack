using ImmersiveGames.DebugManagers;
using ImmersiveGames.ObjectManagers.Interfaces;
using NewRiverAttack.ObstaclesSystems.AreaEffectSystems;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerAreaEffect : MonoBehaviour
    {
        private PlayerMaster _playerMaster;
        private IAreaEffect _areaEffect;

        #region Unity Methods

        private void Awake()
        {
            _playerMaster = GetComponent<PlayerMaster>();
        }

        private void OnTriggerEnter(Collider other)
        {
            _areaEffect = other.GetComponentInParent<IAreaEffect>();
            if (_areaEffect == null) return;
            DebugManager.Log<PlayerAreaEffect>($"Enter Active: {other}");
            _areaEffect.EnterEffect();
            var areaEffectMaster = _areaEffect as AreaEffectMaster;
            if (areaEffectMaster == null) return;
            _playerMaster.OnEventPlayerMasterAreaEffectStart(areaEffectMaster.GetObjectScriptable<AreaEffectScriptable>());
        }

        private void OnTriggerExit(Collider other)
        {
            _areaEffect = other.GetComponentInParent<IAreaEffect>();
            if (_areaEffect == null) return;
            DebugManager.Log<PlayerAreaEffect>($"Exit Active: {other}");
            _areaEffect.ExitEffect();
            var areaEffectMaster = _areaEffect as AreaEffectMaster;
            if (areaEffectMaster == null) return;
            _playerMaster.OnEventPlayerMasterAreaEffectEnd(areaEffectMaster.GetObjectScriptable<AreaEffectScriptable>());
        }

        #endregion
    }
}