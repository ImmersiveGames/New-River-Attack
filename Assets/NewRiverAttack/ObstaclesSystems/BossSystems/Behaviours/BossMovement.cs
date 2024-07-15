using System;
using System.Linq;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossMovement
    {
        private readonly Vector2 _limitX;
        private readonly Vector2 _limitZ;
        private readonly Transform _playerTransform;
        private readonly Transform _bossTransform;
        public readonly Direction MyDirection;

        public BossMovement(Direction direction, Transform playerTransform)
        {
            _limitX = GamePlayBossManager.instance.bossAreaX;
            _limitZ = GamePlayBossManager.instance.bossAreaZ;
            MyDirection = direction;
            _playerTransform = playerTransform;
        }

        public enum Direction
        {
            MoveNorthBehavior,
            MoveSouthBehavior,
            MoveEastBehavior,
            MoveWestBehavior
        }

        public Vector3 GetNewPosition(Direction direction, float distance = 10f)
        {
            var playerPosition = _playerTransform.position;
            var newPosition = playerPosition;

            switch (direction)
            {
                case Direction.MoveNorthBehavior:
                    newPosition.z += distance;
                    if (newPosition.z > _limitZ.y)
                    {
                        newPosition.z = _limitZ.y;
                    }
                    break;
                case Direction.MoveSouthBehavior:
                    newPosition.z -= distance;
                    if (newPosition.z < _limitZ.x)
                    {
                        newPosition.z = _limitZ.x;
                    }
                    break;
                case Direction.MoveEastBehavior:
                    newPosition.x += distance;
                    if (newPosition.x > _limitX.y)
                    {
                        newPosition.x = _limitX.y;
                    }
                    break;
                case Direction.MoveWestBehavior:
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

        public Direction GetRelativeDirection(Vector3 bossPosition)
        {
            var playerPosition = _playerTransform.position;
            var direction = bossPosition - playerPosition;

            // Determine the primary direction based on greater distance
            if (Mathf.Abs(direction.z) > Mathf.Abs(direction.x))
            {
                return direction.z > 0 ? Direction.MoveNorthBehavior : Direction.MoveSouthBehavior;
            }
            return direction.x > 0 ? Direction.MoveEastBehavior : Direction.MoveWestBehavior;
        }

        private static Direction GetRandomDirection(Direction exclude)
        {
            var directions = Enum.GetValues(typeof(Direction))
                                 .Cast<Direction>()
                                 .Where(dir => dir != exclude)
                                 .ToArray();
            var randomIndex = UnityEngine.Random.Range(0, directions.Length);
            return directions[randomIndex];
        }
        public Direction GetRandomDirectionSelfExclude()
        {
            return GetRandomDirection(MyDirection);
        }
    }
}
