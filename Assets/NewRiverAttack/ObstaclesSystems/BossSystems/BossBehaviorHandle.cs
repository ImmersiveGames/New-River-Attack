using System;
using System.Collections.Generic;
using ImmersiveGames.BehaviorTreeSystem;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using ImmersiveGames.BehaviorTreeSystem.Nodes;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossBehaviorHandle : MonoBehaviour
    {
        private BehaviorTree _tree;
        private BossMaster _bossMaster;

        private void Awake()
        {
            SetInitialReferences();
        }

        private void Start()
        {
            GamePlayManager.Instance.EventGameReload += ResetAll;
            CreateBossBehaviors();
        }

        private void Update()
        {
            if (!_bossMaster.ObjectIsReady) return;
            _tree?.Tick();  // Atualiza a árvore de comportamento a cada frame
        }

        private void OnDisable()
        {
            GamePlayManager.Instance.EventGameReload -= ResetAll;
        }

        private void SetInitialReferences()
        {
            _bossMaster = GetComponent<BossMaster>();
        }

        private INodeFunctionProvider GetComponentByID<T>(int idNode = 0) where T : Component
        {
            var components = GetComponents<T>();
            if (components.Length == 0) return null; // Retorna null se não houver componentes

            foreach (var mono in components)
            {
                var functionProvider = mono as INodeFunctionProvider;

                if (functionProvider?.NodeID == idNode)
                    return (INodeFunctionProvider)mono; // Retorna o componente que corresponde ao NodeID
            }

            return null; // Retorna null se nenhum componente correspondente for encontrado
        }

        private void CreateBossBehaviors()
        {
            var enterScene = GetComponent<BossBehaviorEnterScene>();
            var singleShoot = GetComponentByID<BossBehaviorSingleShoot>(1);
            var coneShoot = GetComponentByID<BossBehaviorConeShoot>(1);
            var coneShoot02 = GetComponentByID<BossBehaviorConeShoot>(2);
            var spawnShoot = GetComponent<BossBehaviorRandomSpawn>();

            // Cria Nodes
            var nodeEnterScene = NodeFactory.CreateNodeFromFunctionProvider(enterScene);
            var nodeSingleShoot = NodeFactory.CreateNodeFromFunctionProvider(singleShoot);
            var nodeConeShoot = NodeFactory.CreateNodeFromFunctionProvider(coneShoot);
            var nodeConeShoot02 = NodeFactory.CreateNodeFromFunctionProvider(coneShoot02);
            var nodeMineShoot = NodeFactory.CreateNodeFromFunctionProvider(spawnShoot);
            var nodeWaitSec = NodeFactory.CreateNode(NodeTypes.WaitNode, new Dictionary<NodeParam, object>
            {
                { NodeParam.WaitTime, 1f }
            });

            // Aplica Decorators
            var onEnterMineShoot = NodeFactory.ApplyDecorator(nodeMineShoot, NodeDecorations.OnEnterDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.OnEnter, (Action)spawnShoot.ResetShoot }
                }
            );
            var onEnterEnterScene = NodeFactory.ApplyDecorator(nodeEnterScene, NodeDecorations.OnEnterDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.OnEnter, (Action)enterScene.OnEnter }
                }
            );
            var repeatSingleShot = NodeFactory.ApplyDecorator(nodeSingleShoot, NodeDecorations.RepeatDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.Times, 3 }
                }
            );
            var repeatConeShot = NodeFactory.ApplyDecorator(nodeConeShoot02, NodeDecorations.RepeatDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.Times, 3 }
                }
            );

            // Sequência de Nodes
            var listNodesSequencial = new List<INode>
            {
                nodeWaitSec,
                onEnterEnterScene,
                nodeWaitSec,
                repeatConeShot,
                nodeWaitSec,
                onEnterMineShoot,
                
                /*nodeWaitSec,
                repeatSingleShot,
                nodeWaitSec,
                onEnterMineShoot,*/
            };

            _tree = new BehaviorTree(new SequenceNode(listNodesSequencial));
        }

        // Método para resetar a árvore
        public void ResetTree()
        {
            if (_tree == null) return;
            Debug.Log("Resetando a árvore de comportamento");
            _tree = null;  // Limpa a árvore atual
            CreateBossBehaviors();  // Recria a árvore de comportamento do zero
        }

        // Método para resetar todo o comportamento, incluindo os nós e a árvore
        private void ResetAll()
        {
            // Resetar comportamentos individuais
            var singleShoot = GetComponentByID<BossBehaviorSingleShoot>(1) as BossBehaviorSingleShoot;
            var coneShoot = GetComponentByID<BossBehaviorConeShoot>(1) as BossBehaviorConeShoot;
            var enterScene = GetComponent<BossBehaviorEnterScene>();  // Adiciona o EnterScene ao reset

            singleShoot?.ResetShoot();
            coneShoot?.ResetShoot();
            enterScene?.ResetEnterScene();  // Resetar o EnterScene

            // Resetar a árvore de comportamento
            ResetTree();
        }

    }
}
