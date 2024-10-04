using System;
using System.Collections.Generic;
using ImmersiveGames.BehaviorTreeSystem.Decorations;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using ImmersiveGames.BehaviorTreeSystem.Nodes;

namespace ImmersiveGames.BehaviorTreeSystem
{
    public static class NodeFactory
    {
        // Armazena referências de nós criados
        private static readonly Dictionary<string, INode> _nodeReferences = new Dictionary<string, INode>();

        // Método para criar ActionNodes a partir de funções fornecidas por MonoBehaviour (INodeFunctionProvider)
        public static INode CreateNodeFromFunctionProvider(INodeFunctionProvider functionProvider)
        {
            if (functionProvider == null)
                throw new ArgumentException("The provided MonoBehaviour does not implement INodeFunctionProvider.");
            // Cria um ActionNode a partir da função fornecida
            var node = new ActionNode(functionProvider.GetNodeFunction());
            _nodeReferences[functionProvider.NodeName] = node; // Armazena o nó para referência futura
            return node;
        }

        // Método para criar nós genéricos com base no tipo (incluindo ParallelNode, SequenceNode, RandomSelectorNode, etc.)
        public static INode CreateNode(NodeTypes nodeType, Dictionary<NodeParam, object> parameters)
        {
            return nodeType switch
            {
                NodeTypes.ActionNode => CreateActionNode(parameters),
                NodeTypes.SequenceNode => CreateSequenceNode(parameters),
                NodeTypes.RandomSelectorNode => CreateRandomSelectorNode(parameters),
                NodeTypes.WaitNode => CreateWaitNode(parameters),
                _ => throw new ArgumentException($"Unsupported node type: {nodeType}")
            };
        }

        // Criação de ActionNode a partir de parâmetros
        private static INode CreateActionNode(Dictionary<NodeParam, object> parameters)
        {
            if (!parameters.TryGetValue(NodeParam.Action, out var actionObj) || actionObj is not Func<NodeState> action)
                throw new ArgumentException("Invalid or missing 'action' parameter for ActionNode.");

            var node = new ActionNode(action);

            if (parameters.TryGetValue(NodeParam.NodeName, out var nodeName) && nodeName is string name)
            {
                _nodeReferences[name] = node;
            }

            return node;
        }

        // Criação de SequenceNode
        private static INode CreateSequenceNode(Dictionary<NodeParam, object> parameters)
        {
            if (!(parameters.TryGetValue(NodeParam.Nodes, out var nodesObj) && nodesObj is List<INode> nodes))
                throw new ArgumentException("Invalid or missing 'nodes' parameter for SequenceNode.");

            var node = new SequenceNode(nodes);

            if (parameters.TryGetValue(NodeParam.NodeName, out var nodeName) && nodeName is string name)
            {
                _nodeReferences[name] = node;
            }

            return node;
        }

        // Criação de RandomSelectorNode
        private static INode CreateRandomSelectorNode(Dictionary<NodeParam, object> parameters)
        {
            if (!(parameters.TryGetValue(NodeParam.Nodes, out var nodesObj) && nodesObj is List<INode> nodes))
                throw new ArgumentException("Invalid or missing 'nodes' parameter for RandomSelectorNode.");

            // Validação do parâmetro 'maxTimes' com fallback para 1
            var maxTimes = parameters.TryGetValue(NodeParam.RandomMaxTimes, out var maxTimesObj) && maxTimesObj is int times ? times : 1;

            var node = new RandomSelectorNode(nodes, maxTimes);

            // Armazena a referência do nó para uso futuro
            if (parameters.TryGetValue(NodeParam.NodeName, out var nodeName) && nodeName is string name)
            {
                _nodeReferences[name] = node;
            }

            return node;
        }

        // Criação de WaitNode
        private static INode CreateWaitNode(Dictionary<NodeParam, object> parameters)
        {
            if (!parameters.TryGetValue(NodeParam.WaitTime, out var waitTimeObj) || waitTimeObj is not float waitTime)
                throw new ArgumentException("Invalid or missing 'waitTime' parameter for WaitNode.");
            var node = new WaitNode(waitTime);

            if (parameters.TryGetValue(NodeParam.NodeName, out var nodeName) && nodeName is string name)
            {
                _nodeReferences[name] = node;
            }

            return node;
        }

        // Sobrecarga para aplicar decorators usando o nome
        public static INode ApplyDecorator(string nodeName, NodeDecorations decoratorType, Dictionary<NodeDecorationsParam, object> parameters)
        {
            if (!_nodeReferences.TryGetValue(nodeName, out var node))
                throw new ArgumentException($"Node with name {nodeName} does not exist.");
        
            return ApplyDecorator(node, decoratorType, parameters);  // Reutiliza a lógica que passa o nó diretamente
        }

        // Sobrecarga para aplicar decorators passando diretamente o nó
        public static INode ApplyDecorator(INode node, NodeDecorations decoratorType, Dictionary<NodeDecorationsParam, object> parameters)
        {
            return decoratorType switch
            {
                NodeDecorations.OnEnterDecorator => CreateOnEnterDecoratorNode(node, parameters),
                NodeDecorations.OnExitDecorator => CreateOnExitDecoratorNode(node, parameters),
                NodeDecorations.DelayDecorator => CreateDelayDecoratorNode(node, parameters),
                NodeDecorations.RepeatDecorator => CreateRepeatDecoratorNode(node, parameters),
                NodeDecorations.ParallelConditionDecorator => CreateParallelConditionDecorator(node, parameters),
                NodeDecorations.GlobalConditionDecorator => CreateGlobalConditionDecorator(node, parameters),
                _ => throw new ArgumentException($"Unsupported decorator type: {decoratorType}")
            };
        }

        // Criação de OnEnterDecoratorNode
        private static INode CreateOnEnterDecoratorNode(INode node, Dictionary<NodeDecorationsParam, object> parameters)
        {
            if (!parameters.TryGetValue(NodeDecorationsParam.OnEnter, out var onEnterObj) ||
                onEnterObj is not Action onEnter)
                throw new ArgumentException("Missing 'onEnter' parameter for OnEnterDecoratorNode.");

            return new OnEnterDecoratorNode(node, onEnter);
        }

        // Criação de OnExitDecoratorNode
        private static INode CreateOnExitDecoratorNode(INode node, Dictionary<NodeDecorationsParam, object> parameters)
        {
            if (!parameters.TryGetValue(NodeDecorationsParam.OnExit, out var onExitObj) ||
                onExitObj is not Action onExit)
                throw new ArgumentException("Missing 'onExit' parameter for OnExitDecoratorNode.");

            return new OnExitDecoratorNode(node, onExit);
        }

        // Criação de DelayDecoratorNode
        private static INode CreateDelayDecoratorNode(INode node, Dictionary<NodeDecorationsParam, object> parameters)
        {
            if (!parameters.TryGetValue(NodeDecorationsParam.Delay, out var delayObj) || delayObj is not float delay)
                throw new ArgumentException("Missing 'delay' parameter for DelayDecoratorNode.");

            return new DelayDecoratorNode(node, delay);
        }

        // Criação de RepeatDecoratorNode
        private static INode CreateRepeatDecoratorNode(INode node, Dictionary<NodeDecorationsParam, object> parameters)
        {
            if (!parameters.TryGetValue(NodeDecorationsParam.Times, out var timesObj) || timesObj is not int times)
                throw new ArgumentException("Missing 'times' parameter for RepeatDecoratorNode.");

            return new RepeatDecoratorNode(node, times);
        }

        // Criação do ParallelConditionDecorator
        private static INode CreateParallelConditionDecorator(INode node,
            Dictionary<NodeDecorationsParam, object> parameters)
        {
            if (!(parameters.TryGetValue(NodeDecorationsParam.ParallelCondition, out var conditionObj) &&
                  conditionObj is Func<bool> condition) ||
                !(parameters.TryGetValue(NodeDecorationsParam.ParallelActionNode, out var actionNodeObj) &&
                  actionNodeObj is INode actionNode))
                throw new ArgumentException(
                    "Missing 'ParallelCondition' or 'ParallelActionNode' parameter for ParallelConditionDecorator.");

            return new ParallelConditionDecorator(node, condition, actionNode);
        }

        // Criação do GlobalConditionDecorator
        private static INode CreateGlobalConditionDecorator(INode node,
            Dictionary<NodeDecorationsParam, object> parameters)
        {
            if (!(parameters.TryGetValue(NodeDecorationsParam.GlobalCondition, out var conditionObj) &&
                  conditionObj is Func<bool> condition) ||
                !(parameters.TryGetValue(NodeDecorationsParam.GlobalOverrideNode, out var overrideNodeObj) &&
                  overrideNodeObj is INode overrideNode))
                throw new ArgumentException(
                    "Missing 'GlobalCondition' or 'GlobalOverrideNode' parameter for GlobalConditionDecorator.");

            return new GlobalConditionDecorator(node, condition, overrideNode);
        }
    }
}