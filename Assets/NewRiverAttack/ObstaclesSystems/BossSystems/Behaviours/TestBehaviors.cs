using System.Threading;
using System.Threading.Tasks;
using ImmersiveGames.BehaviorsManagers;
using ImmersiveGames.BehaviorsManagers.Interfaces;
using ImmersiveGames.DebugManagers;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class TestSubBehaviorsA : Behavior
    {
        public TestSubBehaviorsA(BehaviorManager behaviorManager, IBehavior[] subBehaviors, params object[] data) : base(subBehaviors, string.Join("_", data))
        {
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Sub Enter: {GetType().Name}.");
        }
        public override void UpdateAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Sub Update: {GetType().Name}.");
        }
        public override async Task ExitAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Sub Exit: {GetType().Name}.");
        }
    }
    public class TestSubBehaviorsB : Behavior
    {
        public TestSubBehaviorsB(BehaviorManager behaviorManager, IBehavior[] subBehaviors, params object[] data) : base(subBehaviors, string.Join("_", data))
        {
        }
        public override async Task EnterAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Sub Enter: {GetType().Name}.");
        }
        public override void UpdateAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Sub Update: {GetType().Name}.");
        }
        public override async Task ExitAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Sub Exit: {GetType().Name}.");
        }
    }
    public class TestSubBehaviorsC : Behavior
    {
        public TestSubBehaviorsC(BehaviorManager behaviorManager, IBehavior[] subBehaviors, params object[] data) : base(subBehaviors, string.Join("_", data))
        {
        }
        public override async Task EnterAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Sub Enter: {GetType().Name}.");
        }
        public override void UpdateAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Sub Update: {GetType().Name}.");
        }
        public override async Task ExitAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Sub Exit: {GetType().Name}.");
        }
    }
    public class TestSubBehaviorsD : Behavior
    {
        public TestSubBehaviorsD(BehaviorManager behaviorManager, IBehavior[] subBehaviors, params object[] data) : base(subBehaviors, string.Join("_", data))
        {
        }

        public override async Task EnterAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Sub Enter: {GetType().Name}.");
        }
        public override void UpdateAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Sub Update: {GetType().Name}.");
        }
        public override async Task ExitAsync(CancellationToken token)
        {
            DebugManager.Log<Behavior>($"Sub Exit: {GetType().Name}.");
        }
    }
}