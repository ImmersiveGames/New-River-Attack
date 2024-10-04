using System;
using System.Collections.Generic;
using ImmersiveGames.BehaviorTreeSystem.Decorations;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using ImmersiveGames.BehaviorTreeSystem.Nodes;
using UnityEngine;

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
            _nodeReferences[functionProvider.NodeName] = node;  // Armazena o nó para referência futura
            return node;
        }

        // Método para criar nós genéricos com base no tipo (incluindo ParallelNode, SequenceNode, RandomSelectorNode, etc.)
        public static INode CreateNode(string nodeType, Dictionary<string, object> parameters)
        {
            return nodeType switch
            {
                "ActionNode" => CreateActionNode(parameters),
                "SequenceNode" => CreateSequenceNode(parameters),
                "ParallelNode" => CreateParallelNode(parameters),
                "GlobalConditionNode" => CreateGlobalConditionNode(parameters),
                "RandomSelectorNode" => CreateRandomSelectorNode(parameters),
                "WaitNode" => CreateWaitNode(parameters),
                _ => throw new ArgumentException($"Unsupported node type: {nodeType}")
            };
        }

        // Criação de ActionNode a partir de parâmetros
        private static INode CreateActionNode(Dictionary<string, object> parameters)
        {
            if (!parameters.TryGetValue("action", out var actionObj) || actionObj is not Func<NodeState> action)
                throw new ArgumentException("Invalid or missing 'action' parameter for ActionNode.");

            var node = new ActionNode(action);

            if (parameters.TryGetValue("name", out var nodeName) && nodeName is string name)
            {
                _nodeReferences[name] = node;
            }

            return node;
        }

        // Criação de SequenceNode
        private static INode CreateSequenceNode(Dictionary<string, object> parameters)
        {
            if (!(parameters.TryGetValue("nodes", out var nodesObj) && nodesObj is List<INode> nodes))
                throw new ArgumentException("Invalid or missing 'nodes' parameter for SequenceNode.");

            var node = new SequenceNode(nodes);

            if (parameters.TryGetValue("name", out var nodeName) && nodeName is string name)
            {
                _nodeReferences[name] = node;
            }

            return node;
        }

        // Criação de ParallelNode
        private static INode CreateParallelNode(Dictionary<string, object> parameters)
        {
            if (!(parameters.TryGetValue("nodes", out var nodesObj) && nodesObj is List<INode> nodes))
                throw new ArgumentException("Invalid or missing 'nodes' parameter for ParallelNode.");

            var requireAllSuccess = parameters.TryGetValue("requireAllSuccess", out var requireAllSuccessObj) && (requireAllSuccessObj as bool? ?? false);
            var interruptOnSuccess = parameters.TryGetValue("interruptOnSuccess", out var interruptOnSuccessObj) && (interruptOnSuccessObj as bool? ?? false);

            var node = new ParallelNode(nodes, requireAllSuccess, interruptOnSuccess);

            if (parameters.TryGetValue("name", out var nodeName) && nodeName is string name)
            {
                _nodeReferences[name] = node;
            }

            return node;
        }

        // Criação de GlobalConditionNode
        private static INode CreateGlobalConditionNode(Dictionary<string, object> parameters)
        {
            if (!(parameters.TryGetValue("condition", out var conditionObj) && conditionObj is Func<bool> condition) ||
                !(parameters.TryGetValue("overrideNode", out var overrideNodeObj) && overrideNodeObj is INode overrideNode))
                throw new ArgumentException("Invalid or missing 'condition' or 'overrideNode' parameter for GlobalConditionNode.");

            var globalNode = new GlobalConditionNode(condition, overrideNode);

            if (parameters.TryGetValue("name", out var nodeName) && nodeName is string name)
            {
                _nodeReferences[name] = globalNode;
            }

            return globalNode;
        }

        // Criação de RandomSelectorNode
        private static INode CreateRandomSelectorNode(Dictionary<string, object> parameters)
        {
            if (!(parameters.TryGetValue("nodes", out var nodesObj) && nodesObj is List<INode> nodes))
                throw new ArgumentException("Invalid or missing 'nodes' parameter for RandomSelectorNode.");

            var maxTimes = parameters.TryGetValue("maxTimes", out var maxTimesObj) && maxTimesObj is int times ? times : 1;

            var node = new RandomSelectorNode(nodes, maxTimes);

            if (parameters.TryGetValue("name", out var nodeName) && nodeName is string name)
            {
                _nodeReferences[name] = node;
            }

            return node;
        }

        // Criação de WaitNode
        private static INode CreateWaitNode(Dictionary<string, object> parameters)
        {
            if (!parameters.TryGetValue("waitTime", out var waitTimeObj) || waitTimeObj is not float waitTime)
                throw new ArgumentException("Invalid or missing 'waitTime' parameter for WaitNode.");
            var node = new WaitNode(waitTime);

            if (parameters.TryGetValue("name", out var nodeName) && nodeName is string name)
            {
                _nodeReferences[name] = node;
            }

            return node;

        }

        // Método para aplicar Decorators a um nó existente
        public static INode ApplyDecorator(string nodeName, string decoratorType, Dictionary<string, object> parameters)
        {
            if (!_nodeReferences.TryGetValue(nodeName, out var node))
                throw new ArgumentException($"Node with name {nodeName} does not exist.");

            return decoratorType switch
            {
                "OnEnterDecorator" => CreateOnEnterDecoratorNode(node, parameters),
                "OnExitDecorator" => CreateOnExitDecoratorNode(node, parameters),
                "DelayDecorator" => CreateDelayDecoratorNode(node, parameters),
                "RepeatDecorator" => CreateRepeatDecoratorNode(node, parameters),
                _ => throw new ArgumentException($"Unsupported decorator type: {decoratorType}")
            };
        }

        // Criação de OnEnterDecoratorNode
        private static INode CreateOnEnterDecoratorNode(INode node, Dictionary<string, object> parameters)
        {
            if (!parameters.TryGetValue("onEnter", out var onEnterObj) || onEnterObj is not Action onEnter)
                throw new ArgumentException("Missing 'onEnter' parameter for OnEnterDecoratorNode.");

            return new OnEnterDecoratorNode(node, onEnter);
        }

        // Criação de OnExitDecoratorNode
        private static INode CreateOnExitDecoratorNode(INode node, Dictionary<string, object> parameters)
        {
            if (!parameters.TryGetValue("onExit", out var onExitObj) || onExitObj is not Action onExit)
                throw new ArgumentException("Missing 'onExit' parameter for OnExitDecoratorNode.");

            return new OnExitDecoratorNode(node, onExit);
        }

        // Criação de DelayDecoratorNode
        private static INode CreateDelayDecoratorNode(INode node, Dictionary<string, object> parameters)
        {
            if (!parameters.TryGetValue("delay", out var delayObj) || delayObj is not float delay)
                throw new ArgumentException("Missing 'delay' parameter for DelayDecoratorNode.");

            return new DelayDecoratorNode(node, delay);
        }

        // Criação de RepeatDecoratorNode
        private static INode CreateRepeatDecoratorNode(INode node, Dictionary<string, object> parameters)
        {
            if (!parameters.TryGetValue("times", out var timesObj) || timesObj is not int times)
                throw new ArgumentException("Missing 'times' parameter for RepeatDecoratorNode.");

            return new RepeatDecoratorNode(node, times);
        }
    }
}
