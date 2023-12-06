using System.Collections;
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
