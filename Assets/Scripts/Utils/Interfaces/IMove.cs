using UnityEngine;
namespace  Utils
{
    public interface IMove
    {
        abstract void Move(Vector3 direction, float velocity);
        abstract void StopMove();
        abstract bool ShouldMoving();
        abstract void ResetMovement();
    }
}

