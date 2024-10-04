using System;
using System.Collections.Generic;
using ImmersiveGames.BehaviorTreeSystem;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using ImmersiveGames.BehaviorTreeSystem.Nodes;
using UnityEngine;
using UnityEngine.Serialization;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossBehaviorHandle : MonoBehaviour
    {
        public List<MonoBehaviour> functionProviders;  // Referências aos scripts que fornecem funções
        private BehaviorTree _tree;
        private void Start()
        {
            // Lista para armazenar os nós criados
            var nodes = new List<INode>();

            // Criar nós a partir dos providers de funções (ex: AttackProvider, MoveProvider)
            foreach (var provider in functionProviders)
            {
                try
                {
                    if (provider is not INodeFunctionProvider nodeProvider) continue;
                    var node = NodeFactory.CreateNodeFromFunctionProvider(nodeProvider);
                    nodes.Add(node);

                }
                catch (ArgumentException ex)
                {
                    Debug.LogError(ex.Message);
                }
            }

            // Exemplo de criação de um ParallelNode com dois WaitNodes
            var parallelParams = new Dictionary<string, object>
            {
                { "nodes", new List<INode>
                    {
                        NodeFactory.CreateNode("WaitNode", new Dictionary<string, object> { { "waitTime", 2.0f }, { "name", "WaitNode1" } }),
                        NodeFactory.CreateNode("WaitNode", new Dictionary<string, object> { { "waitTime", 3.0f }, { "name", "WaitNode2" } })
                    }
                },
                { "requireAllSuccess", true },
                { "name", "ParallelNode" }
            };
            var parallelNode = NodeFactory.CreateNode("ParallelNode", parallelParams);
            nodes.Add(parallelNode);

            // Exemplo de aplicar um DelayDecorator ao ParallelNode
            NodeFactory.ApplyDecorator("ParallelNode", "DelayDecorator", new Dictionary<string, object>
            {
                { "delay", 1.5f }  // Aplica um atraso de 1.5 segundos
            });

            // Criar a árvore de comportamento com SequenceNode
            _tree = new BehaviorTree(new SequenceNode(nodes));
        }

        private void Update()
        {
            _tree?.Tick();  // Atualiza a árvore de comportamento a cada frame
        } 

    }
    
}