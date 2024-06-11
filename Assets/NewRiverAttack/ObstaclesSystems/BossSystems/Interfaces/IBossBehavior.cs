namespace NewRiverAttack.ObstaclesSystems.BossSystems.Interfaces
{
    public interface IBossBehavior
    {
        void Enter();
        void Update();
        void Exit();
        void FinishBehavior();
        bool IsFinished(); 
    }
}

