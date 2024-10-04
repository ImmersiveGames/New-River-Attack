using System;
using System.Collections.Generic;
using ImmersiveGames.BehaviorTreeSystem;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using ImmersiveGames.BehaviorTreeSystem.Nodes;
using NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours;
using UnityEngine;
using UnityEngine.Serialization;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossBehaviorHandle : MonoBehaviour
    {
        //public List<MonoBehaviour> functionProviders;  // Referências aos scripts que fornecem funções
        private BehaviorTree _tree;
        private BossMaster _bossMaster;

        private void Awake()
        {
            SetInitialReferences();
        }

        private void Start()
        {
            CreateBossBehaviors();
        }

        private void Update()
        {
            if (!_bossMaster.ObjectIsReady) return;
            _tree?.Tick();  // Atualiza a árvore de comportamento a cada frame
        }

        private void SetInitialReferences()
        {
            _bossMaster = GetComponent<BossMaster>(); 
        }

        private void CreateBossBehaviors()
        {
            var enterScene = GetComponent<BossBehaviorEnterScene>();
            //Cria Nodes
            var nodeEnterScene = NodeFactory.CreateNodeFromFunctionProvider(enterScene);
            var nodeWaitSec = NodeFactory.CreateNode(NodeTypes.WaitNode, new Dictionary<NodeParam, object>
            {
                { NodeParam.WaitTime, 1 }
            });
            //Aplica Decorators
            var enterSceneOnEnter = NodeFactory.ApplyDecorator(nodeEnterScene, NodeDecorations.OnEnterDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    {NodeDecorationsParam.OnEnter,(Action)enterScene.StartSetup}
                });
           
            var listNodesSequencial = new List<INode>
            {
                enterSceneOnEnter,
                nodeWaitSec
            };
            _tree = new BehaviorTree(new SequenceNode(listNodesSequencial));
        }

    }
    
}