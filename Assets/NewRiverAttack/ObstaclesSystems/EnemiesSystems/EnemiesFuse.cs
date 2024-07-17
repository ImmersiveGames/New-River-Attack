using GD.MinMaxSlider;
using ImmersiveGames.StateManagers;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesFuse : MonoBehaviour
    {
        [Header("Patrol")]
        [MinMaxSlider(0f, 5f)]
        public Vector2 approachMovement;

        public float timeInAlert;
        public GameObject detonationVfx;
        [Range(1f, 15f)] public float radiusExpendSize = 10f;
        [Range(1f, 2f)] public float expansionDuration = 1f;
        [Range(1f, 10f)] public float shakeForce = 5f;
        [Range(0.01f, 0.1f)] public float shakeTime = 0.02f;
        [Range(100, 1000)] public long millisecondsVibrate = 300;
        
        private SimpleStateMachine<ISimpleState> _stateMachine;
        
        private EnemiesMaster _enemiesMaster;
        
        private void OnEnable()
        {
            SetInitialReferences();
            _enemiesMaster.EventSpawnObject += UpdatePosition;
            
        }

        private void Update()
        {
            if (_enemiesMaster.ObjectIsReady)
            {
                _stateMachine?.UpdateState();
            }
        }

        private void OnDisable()
        {
            _enemiesMaster.EventSpawnObject -= UpdatePosition;
        }

        private void SetInitialReferences()
        {
            _enemiesMaster = GetComponent<EnemiesMaster>();
        }
        private void UpdatePosition(Vector2 position)
        {
            _stateMachine = new SimpleStateMachine<ISimpleState>(this);
            _stateMachine.ChangeState(new MineIdle());
        }

        public void ChangeState(ISimpleState simpleState)
        {
            _stateMachine.ChangeState(simpleState);
        }

        public EnemiesMaster GetEnemiesMaster => _enemiesMaster;
        
        public float GetMoveApproach => approachMovement is { x: 0, y: > 0 } ? approachMovement.y : Random.Range(approachMovement.x, approachMovement.y);
        
    }
}
