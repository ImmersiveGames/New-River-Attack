using System;
using System.Linq;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorMovement : MonoBehaviour, INodeFunctionProvider
    {
        [SerializeField] private float bossDistance = 12f;
        private BossDirections _myDirections;
        private Vector2 _limitX;
        private Vector2 _limitZ;
        private const float AdditionalZ = 3f; // Valor adicional no eixo Z

        #region Unity Methods

        private void Awake()
        {
            _myDirections = BossDirections.North;
            _limitX = GamePlayBossManager.instance.bossAreaX;
            _limitZ = GamePlayBossManager.instance.bossAreaZ;
        }

        #endregion

        private NodeState ChooseNewPosition()
        {
            var playerPosition = GetReferencePosition();
            var newDirection = GetRandomDirection(_myDirections);
            var newPosition = GetNewPosition(newDirection, playerPosition, bossDistance);
            transform.position = newPosition;
            return NodeState.Success;
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

                case BossDirections.East:
                    newPosition.x += distance;
                    if (newPosition.x > _limitX.y)
                    {
                        newPosition.x = _limitX.y;
                    }

                    newPosition.z += AdditionalZ; // Adiciona valor extra no Z
                    if (newPosition.z > _limitZ.y) // Checa o limite superior do eixo Z
                    {
                        newPosition.z = _limitZ.y;
                    }

                    break;

                case BossDirections.West:
                    newPosition.x -= distance;
                    if (newPosition.x < _limitX.x)
                    {
                        newPosition.x = _limitX.x;
                    }

                    newPosition.z += AdditionalZ; // Adiciona valor extra no Z
                    if (newPosition.z > _limitZ.y) // Checa o limite superior do eixo Z
                    {
                        newPosition.z = _limitZ.y;
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
            var target = PlayersManager.Instance.GetPlayerMaster(0);
            return target.transform.position;
        }

        public Func<NodeState> GetNodeFunction()
        {
            return ChooseNewPosition;
        }

        private enum BossDirections
        {
            North,
            West,
            East
        }

        public string NodeName => "BossMovement";
        public int NodeID => 0;
    }
}