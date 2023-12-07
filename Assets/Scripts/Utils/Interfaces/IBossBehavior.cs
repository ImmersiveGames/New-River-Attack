using System.Collections;
using RiverAttack;
using UnityEngine;
namespace Utils
{
    public interface IBossBehavior
    {
        void Enter();
        void Update();
        void Exit();
        bool IsFinished(); 
    }
}
