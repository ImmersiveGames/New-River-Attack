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
/*
var action1 = NodeFactory.CreateNode(NodeTypes.ActionNode, new Dictionary<NodeParam, object>
            {
                {
                    NodeParam.Action, new Func<NodeState>(() =>
                    {
                        Debug.Log("Action 01");
                        return NodeState.Success;
                    })
                },
                { NodeParam.NodeName, "Action 01" }
            });
            var action2 = NodeFactory.CreateNode(NodeTypes.ActionNode, new Dictionary<NodeParam, object>
            {
                {
                    NodeParam.Action, new Func<NodeState>(() =>
                    {
                        Debug.Log("Action 02");
                        return NodeState.Success;
                    })
                },
                { NodeParam.NodeName, "Action 02" }
            });

            var listRandom = new List<INode> { action1, action2 };
            // Lista para armazenar os nós criados
            var nodes = new List<INode>
            {
                action1, action2,
                //Node de espera
                NodeFactory.CreateNode(NodeTypes.WaitNode, new Dictionary<NodeParam, object>
                {
                    { NodeParam.WaitTime , 5f},
                    { NodeParam.NodeName , "Action Wait"}
                }),
                NodeFactory.CreateNode(NodeTypes.ActionNode, new Dictionary<NodeParam, object>
                {
                    { NodeParam.Action , new Func<NodeState>(() =>
                    {
                        Debug.Log("Action 03");
                        return NodeState.Success;
                    })},
                    { NodeParam.NodeName , "Action 03"}
                }),
                NodeFactory.CreateNode(NodeTypes.RandomSelectorNode, new Dictionary<NodeParam, object>
                {
                    { NodeParam.Nodes,listRandom },           // Passa a lista de nós
                    { NodeParam.RandomMaxTimes, 3 },          // Executa 3 vezes no máximo
                    { NodeParam.NodeName, "RandomSelector" }
                }),
                NodeFactory.ApplyDecorator("Action 02", NodeDecorations.RepeatDecorator, new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.Times,4 }
                })
            };

            // Criar a árvore de comportamento com SequenceNode
            _tree = new BehaviorTree(new SequenceNode(nodes));
*/