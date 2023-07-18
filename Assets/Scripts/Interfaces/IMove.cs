using UnityEngine;

public interface IMove
{

    void Move(Vector3 directionV3);
    bool ShouldMove();
}
