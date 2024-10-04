using ImmersiveGames.BehaviorTreeSystem.Interface;

namespace ImmersiveGames.BehaviorTreeSystem
{
    public class BehaviorTree
    {
        private readonly INode _rootNode;

        public BehaviorTree(INode rootNode)
        {
            _rootNode = rootNode;
        }

        public void Tick()
        {
            _rootNode.Tick();
        }
    }
}