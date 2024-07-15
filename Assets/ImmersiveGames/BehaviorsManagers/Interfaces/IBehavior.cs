using System.Threading;
using System.Threading.Tasks;
using NewRiverAttack.ObstaclesSystems.BossSystems;
using UnityEngine;

namespace ImmersiveGames.BehaviorsManagers.Interfaces
{
    public interface IBehavior
    {
        string Name { get; }
        IBehavior[] SubBehaviors { get; }
        bool Initialized { get; set; }
        bool Finalized { get; set; }
        
        Task EnterAsync(CancellationToken token);
        Task UpdateAsync(CancellationToken token);
        Task ExitAsync(CancellationToken token);
        /*
        
        
        IBehavior NextBehavior { get; set; }
        
        BehaviorManager SubBehaviorManager { get; set; }
        
        IChangeBehaviorStrategy ChangeBehaviorStrategy { get; }
        IUpdateStrategy UpdateStrategy { get; }
        int CurrentSubBehaviorIndex { get; set; }
        Task FinalizeAllSubBehavior(CancellationToken cancellationToken);
        void Pause();
        void Resume();
        void Stop();*/
    }
}