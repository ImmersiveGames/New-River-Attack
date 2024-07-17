using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class MissileSuperBehavior : MissileBehavior
    {
        public MissileSuperBehavior(BehaviorManager behaviorManager, IBehavior[] subBehaviors, params object[] data) : base(behaviorManager, subBehaviors, data)
        {
        }
    }
}