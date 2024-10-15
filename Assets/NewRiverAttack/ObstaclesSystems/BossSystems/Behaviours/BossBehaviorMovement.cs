using System;
using System.Linq;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorMovement : MonoBehaviour, INodeFunctionProvider
    {
        private BossMaster _bossMaster;
        private BossDirections _myDirections;
        private Vector2 _limitX;
        private Vector2 _limitZ;

        #region Unity Methods

        private void Awake()
        {
            _bossMaster = GetComponent<BossMaster>();
            _myDirections = BossDirections.North;
            _limitX = GamePlayBossManager.instance.bossAreaX;
            _limitZ = GamePlayBossManager.instance.bossAreaZ;
        }

        private void OnEnable()
        {
            _bossMaster.EventBossResetForEnter += ResetBehavior;
        }

        private void OnDisable()
        {
            _bossMaster.EventBossResetForEnter -= ResetBehavior;
        }

        #endregion

        private NodeState ChooseNewPosition()
        {
            var playerPosition = GetReferencePosition();
            var newDirection = GetRandomDirection(_myDirections);
            var newPosition = GetNewPosition(newDirection, playerPosition);
            transform.position = newPosition;
            return NodeState.Success;
        }

        public void ResetBehavior()
        {
            _myDirections = BossDirections.North;
        }

        private Vector3 GetNewPosition(BossDirections direction, Vector3 playerPosition, float distance = 10f)
        {
            var newPosition = playerPosition;

            switch (direction)
            {
                case BossDirections.North:
                    newPosition.z += distance;
                    if (newPosition.z > _limitZ.y)
                    {
                        newPosition.z = _limitZ.y;
                    }
                    break;
                case BossDirections.South:
                    newPosition.z -= distance;
                    if (newPosition.z < _limitZ.x)
                    {
                        newPosition.z = _limitZ.x;
                    }
                    break;
                case BossDirections.East:
                    newPosition.x += distance;
                    if (newPosition.x > _limitX.y)
                    {
                        newPosition.x = _limitX.y;
                    }
                    break;
                case BossDirections.West:
                    newPosition.x -= distance;
                    if (newPosition.x < _limitX.x)
                    {
                        newPosition.x = _limitX.x;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            return newPosition;
        }
        private static BossDirections GetRandomDirection(BossDirections exclude)
        {
            var directions = Enum.GetValues(typeof(BossDirections))
                .Cast<BossDirections>()
                .Where(dir => dir != exclude)
                .ToArray();
            var randomIndex = UnityEngine.Random.Range(0, directions.Length);
            return directions[randomIndex];
        }

        private Vector3 GetReferencePosition()
        {
            var target = GamePlayManager.Instance.GetPlayerMaster(0);
            return target.transform.position;
        }
        public Func<NodeState> GetNodeFunction()
        {
            return ChooseNewPosition;
        }

        private enum BossDirections
        {
            North,
            South,
            West,
            East
        }
        public string NodeName => "BossMovement";
        public int NodeID => 0;
    }
}