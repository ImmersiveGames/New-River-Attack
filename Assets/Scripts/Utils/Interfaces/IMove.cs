using UnityEngine;
namespace  Utils
{
    public interface IMove
    {
        void Move(Vector3 directionV3);
        bool ShouldMove();
    }
}

