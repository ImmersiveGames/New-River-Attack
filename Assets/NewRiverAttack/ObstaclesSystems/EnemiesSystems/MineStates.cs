using ImmersiveGames.CameraManagers;
using ImmersiveGames.ObjectManagers.DetectManagers;
using ImmersiveGames.StateManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class MineIdle : ISimpleState
    {
        private Transform _target;
        private DetectPlayerApproach _detectPlayerApproach;
        private float _approachRange;

        private EnemiesFuse _enemiesFuse;

        public void EnterState(Component fuse)
        {
            _enemiesFuse = fuse as EnemiesFuse;
            if (_enemiesFuse == null) return;
            //Debug.Log($"Entra no Idle {fuse.transform.position}");
            _detectPlayerApproach =
                new DetectPlayerApproach(_enemiesFuse.transform.position, _enemiesFuse.GetMoveApproach);
        }

        public void UpdateState()
        {
            //Debug.Log($"Atualiza no Idle");
            _target = _detectPlayerApproach.TargetApproach<PlayerMaster>(_enemiesFuse.GetEnemiesMaster.layerPlayer);
            if (_target == null) return;
            _enemiesFuse.ChangeState(new MineAlert());
        }

        public void ExitState()
        {
            //Debug.Log($"Sai no Idle");
        }
    }

    public class MineAlert : ISimpleState
    {
        private EnemiesFuse _enemiesFuse;
        private MineMaster _mineMaster;
        private float _timeExpire;

        public void EnterState(Component fuse)
        {
            _enemiesFuse = fuse as EnemiesFuse;
            if (_enemiesFuse == null) return;
            _mineMaster = _enemiesFuse.GetEnemiesMaster as MineMaster;
            //if (_mineMaster != null) _mineMaster.OnEventAlertApproach();
            //_timeExpire = _enemiesFuse.timeInAlert;
            //Debug.Log($"Entra no Alert");
        }

        public void UpdateState()
        {
            //Debug.Log($"Atualiza no Alert");
            _timeExpire -= Time.deltaTime;
            if (_timeExpire <= 0)
            {
                _enemiesFuse.ChangeState(new MineExplode());
            }
        }
        public void ExitState()
        {
            //Debug.Log($"Sai no Alert");
            //if (_mineMaster != null) _mineMaster.OnEventAlertStop();
            //_enemiesFuse.GetComponent<ObstacleSkin>().DesativeSkin();
        }
    }

    public class MineExplode : ISimpleState
    {
        private EnemiesFuse _enemiesFuse;
        private MineMaster _mineMaster;
        private GameObject _effect;
        private SphereCollider _collider;
        private float _startRadius;
        private float _timerParam;
        private bool _isExploding;

        public void EnterState(Component fuse)
        {
            _enemiesFuse = fuse as EnemiesFuse;
            if (_enemiesFuse == null) return;

            _mineMaster = _enemiesFuse.GetEnemiesMaster as MineMaster;
            _effect = Object.Instantiate(_enemiesFuse.detonationVfx, _enemiesFuse.transform.position, _enemiesFuse.transform.rotation);
            _collider = _effect.GetComponentInChildren<SphereCollider>(true);
            _collider.enabled = true;
            _startRadius = _collider.radius;
            _timerParam = 0f;
            _isExploding = true;

            //if (_mineMaster != null)
                //_mineMaster.OnEventDetonate();
        }

        public void UpdateState()
        {
            if (!_isExploding || _enemiesFuse.radiusExpendSize == 0 || _effect == null) return;

            _timerParam += Time.deltaTime;

            float normalizedTime = _timerParam / _enemiesFuse.expansionDuration;
            normalizedTime = Mathf.Clamp01(normalizedTime); // Ensure it doesn't exceed 1

            CameraShake.Instance?.StopShake();
            CameraShake.Instance?.ShakeCamera(_enemiesFuse.shakeForce, _enemiesFuse.shakeTime);
            HardWereVibration.Vibration(_enemiesFuse.millisecondsVibrate);

            if (_collider && _collider.GetType() == typeof(SphereCollider))
            {
                _collider.radius = Mathf.Lerp(_startRadius, _enemiesFuse.radiusExpendSize, normalizedTime);
            }

            if (normalizedTime >= 1f)
            {
                ExitState();
            }
        }

        public void ExitState()
        {
            Object.DestroyImmediate(_effect);
            CameraShake.Instance?.StopShake();
            _enemiesFuse.gameObject.SetActive(false);
            _isExploding = false;
        }
    }

}