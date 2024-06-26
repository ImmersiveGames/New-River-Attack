﻿using System.Threading;
using System.Threading.Tasks;
using NewRiverAttack.ObstaclesSystems.BossSystems;
using UnityEngine;

namespace ImmersiveGames.BehaviorsManagers.Interfaces
{
    public interface IBehavior
    {
        string Name { get; }
        bool Initialized { get; set; }
        bool Finalized { get; set; }
        IBehavior NextBehavior { get; set; }
        IBehavior[] SubBehaviors { get; }
        BehaviorManager SubBehaviorManager { get; set; }
        Task EnterAsync(CancellationToken token);
        Task UpdateAsync(CancellationToken token);
        Task ExitAsync(CancellationToken token);
        IChangeBehaviorStrategy ChangeBehaviorStrategy { get; }
        IUpdateStrategy UpdateStrategy { get; }
        int CurrentSubBehaviorIndex { get; set; }
        Task FinalizeAllSubBehavior(CancellationToken cancellationToken);
        void Pause();
        void Resume();
        void Stop();
    }
}