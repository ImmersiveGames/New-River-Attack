using ImmersiveGames.FiniteStateMachine;
using ImmersiveGames.PoolSystems;
using NewRiverAttack.BulletsManagers;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptable;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems.Mines
{
    public class MineFuse : MonoBehaviour
    {
        public float timeAnimation = 2f;
        public float timeInAlert;
        public GameObject detonationVfx;
        [Range(1f, 15f)] public float radiusExpendSize = 10f;
        [Range(1f, 2f)] public float expansionDuration = 1f;
        [Range(1f, 10f)] public float shakeForce = 5f;
        [Range(0.01f, 0.1f)] public float shakeTime = 0.02f;

        private StateMachine _stateMachine;
  
        private bool _fuseInitialize;
        private MineMaster _mineMaster;
        private Transform _target;
        private EnemiesScriptable _enemiesScriptable;
        private BulletBossMine _bulletBossMine;

        #region Unity Methods

        private void Awake()
        {
            _mineMaster = GetComponent<MineMaster>();
            _bulletBossMine = GetComponent<BulletBossMine>();
            _enemiesScriptable = _mineMaster.GetEnemySettings;
            _target = null;
            _fuseInitialize = false;
        }

        private void Start()
        {
            InitializeStateMachine();
        }

        private void OnEnable()
        {
            if (_stateMachine == null) return;
            InitializeStateMachine();
        }

        private void Update()
        {
            if (ShouldBeReady)  // Só atualiza o FSM quando a mina estiver pronta
                _stateMachine.Tick();
        }

        private void OnDisable()
        {
            _fuseInitialize = false;  // Desativa a mina
            _target = null;           // Reseta o alvo
        }

        #endregion

        public bool ShouldBeReady => _mineMaster.ShouldBeReady && _fuseInitialize;  // Verifica se a mina está pronta
        
        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine();

            var patrolState = new MinePatrolState(this, _enemiesScriptable);
            var alertState = new MineAlertState(this, timeInAlert);
            var explodeState = new MineExplodeState(this);

            _stateMachine.AddTransition(patrolState, alertState, () => _target != null && ShouldBeReady);
            _stateMachine.AddTransition(alertState, patrolState, () => alertState.RemainingTime <= 0 && ShouldBeReady);
            
            _fuseInitialize = false;
            _target = null;
            _mineMaster.OnEventShoot();
            _stateMachine.SetState(patrolState);
            Invoke(nameof(SetFuseReady), timeAnimation);
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void SetFuseReady()
        {
            _fuseInitialize = true;  // Após 2 segundos, a mina está pronta para operar
        }

        public void ReturnBullet()
        {
            if (_bulletBossMine != null)
            {
                _bulletBossMine?.Pool.ReturnObject(gameObject);
            }
        }

        public void MarkForReturn()
        {
            if (_bulletBossMine != null)
            {
                _bulletBossMine?.Pool.MarkForReturn(gameObject);
            }
        }
        
        public LayerMask PlayerLayer => _mineMaster.layerPlayer;

        #region Shortcut Event Calls

        public void OnEventAlertApproach()=> _mineMaster.OnEventAlertApproach();
        public void OnEventAlertStop()=> _mineMaster.OnEventAlertStop();
        public void OnEventDetonate()=> _mineMaster.OnEventDetonate();

        #endregion
    }
}
