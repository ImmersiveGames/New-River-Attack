using System;
using System.Collections.Generic;
using ImmersiveGames.BehaviorTreeSystem;
using ImmersiveGames.BehaviorTreeSystem.Decorations;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using ImmersiveGames.BehaviorTreeSystem.Nodes;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossBehaviorHandle : MonoBehaviour
    {
        private BossCollider _bossCollider;
        
        private BehaviorTree _tree;
        private BossMaster _bossMaster;

        private void Awake()
        {
            SetInitialReferences();
        }

        private void Start()
        {
            GamePlayManager.Instance.EventGameReload += ResetAll;
            _bossCollider = GetComponent<BossCollider>();
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
            #region Providers Creators

            var enterScene = GetComponent<BossBehaviorEnterScene>();
            var singleShoot = GetComponentByID<BossBehaviorSingleShoot>(1);
            var coneShoot = GetComponentByID<BossBehaviorConeShoot>(1);
            var coneShoot02 = GetComponentByID<BossBehaviorConeShoot>(2);
            var mineSpawn = GetComponentByID<BossBehaviorRandomSpawn>(1);
            var movement = GetComponent<BossBehaviorMovement>();
            var emerge = GetComponent<BossBehaviorEmerge>();
            var submerge = GetComponent<BossBehaviorSubmerge>();
            var death = GetComponent<BossBehaviorDeath>();
            var finish = GetComponent<BossBehaviorFinishGame>();
            
            #endregion

            #region Node Factory

            // Cria Nodes
            var nodeEnterScene = NodeFactory.CreateNodeFromFunctionProvider(enterScene);
            var nodeSingleShoot = NodeFactory.CreateNodeFromFunctionProvider(singleShoot);
            var nodeConeShoot01 = NodeFactory.CreateNodeFromFunctionProvider(coneShoot);
            var nodeConeShoot02 = NodeFactory.CreateNodeFromFunctionProvider(coneShoot02);
            var nodeMineShoot = NodeFactory.CreateNodeFromFunctionProvider(mineSpawn);
            var nodeMovement = NodeFactory.CreateNodeFromFunctionProvider(movement);
            var nodeEmerge = NodeFactory.CreateNodeFromFunctionProvider(emerge);
            var nodeSubmerge = NodeFactory.CreateNodeFromFunctionProvider(submerge);
            var nodeDeath = NodeFactory.CreateNodeFromFunctionProvider(death);
            var nodeFinish = NodeFactory.CreateNodeFromFunctionProvider(finish);
            
            var nodeWaitSec = NodeFactory.CreateNode(NodeTypes.WaitNode, new Dictionary<NodeParam, object>
            {
                { NodeParam.WaitTime, 1f }
            });
            var nodeWait5Sec = NodeFactory.CreateNode(NodeTypes.WaitNode, new Dictionary<NodeParam, object>
            {
                { NodeParam.WaitTime, 5f }
            });

            #endregion

            #region Decoration Apply

            // Aplica Decorators
            var onEnterEnterScene = NodeFactory.ApplyDecorator(nodeEnterScene, NodeDecorations.OnEnterDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.OnEnter, (Action)enterScene.OnEnter }
                }
            );
            
            var onEnterExitEmerge = NodeFactory.ApplyDecorator(nodeEmerge, NodeDecorations.OnEnterExitDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.OnEnter, (Action)emerge.OnEnter },
                    { NodeDecorationsParam.OnExit, (Action)emerge.OnExit }
                }
            );
            var onEnterSubmerge = NodeFactory.ApplyDecorator(nodeSubmerge, NodeDecorations.OnEnterDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.OnEnter, (Action)submerge.OnEnter }
                }
            );
            
            var onEnterMineShoot = NodeFactory.ApplyDecorator(nodeMineShoot, NodeDecorations.OnEnterDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.OnEnter, (Action)mineSpawn.ResetBehavior }
                }
            );
            var repeatConeShot01X5 = NodeFactory.ApplyDecorator(nodeConeShoot01, NodeDecorations.RepeatDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.Times, 5 }
                }
            );
            var repeatConeShot01X3 = NodeFactory.ApplyDecorator(nodeConeShoot01, NodeDecorations.RepeatDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.Times, 3 }
                }
            );
            var repeatConeShot02X4 = NodeFactory.ApplyDecorator(nodeConeShoot02, NodeDecorations.RepeatDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.Times, 4 }
                }
            );
            var repeatConeShot02X6 = NodeFactory.ApplyDecorator(nodeConeShoot02, NodeDecorations.RepeatDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.Times, 6 }
                }
            );
            var onEnterDeath = NodeFactory.ApplyDecorator(nodeDeath, NodeDecorations.OnEnterDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.OnEnter, (Action)death.OnEnter }
                }
            );
            var onEnterExitFinish = NodeFactory.ApplyDecorator(nodeFinish, NodeDecorations.OnEnterExitDecorator,
                new Dictionary<NodeDecorationsParam, object>
                {
                    { NodeDecorationsParam.OnEnter, (Action)finish.OnEnter },
                    { NodeDecorationsParam.OnExit, (Action)finish.OnExit }
                }
            );

            #endregion

            #region Sequences of Shoots
            
            var sequenceNorth = new SequenceNode(new List<INode>
            {
                repeatConeShot01X3,
                nodeWaitSec,
                onEnterMineShoot,
                nodeWaitSec,
                repeatConeShot01X5,
                nodeWaitSec,
                onEnterSubmerge,
                nodeSingleShoot,
                nodeMovement,
                onEnterExitEmerge,
                nodeWaitSec
            });
            var sequenceSouth = new SequenceNode(new List<INode>
            {
                repeatConeShot02X6,
                nodeWaitSec,
                onEnterSubmerge,
                nodeSingleShoot,
                nodeMovement,
                onEnterExitEmerge,
                nodeWaitSec,
            });
            var sequenceSide = new SequenceNode(new List<INode>
            {
                repeatConeShot02X4,
                nodeWaitSec,
                repeatConeShot02X6,
                nodeWaitSec,
                onEnterSubmerge,
                nodeSingleShoot,
                nodeMovement,
                onEnterExitEmerge,
                nodeWaitSec,
            });

            #endregion

            var randomPositionShoot = new RandomSelectorNode(new List<INode>
            {
                sequenceNorth,sequenceSouth, sequenceSide
            },int.MaxValue);

            var globalCondition = new GlobalConditionDecorator(
                randomPositionShoot,
                GlobalStopCondition, 
                new ActionNode(StopTree)
            );

            // Sequência de Nodes
            var listNodesSequencial = new List<INode>
            {
                nodeWaitSec,
                onEnterEnterScene,
                nodeWaitSec,
                globalCondition,
                nodeWaitSec,
                onEnterDeath,
                nodeWait5Sec,
                onEnterExitFinish
            };

            _tree = new BehaviorTree(new SequenceNode(listNodesSequencial));
            return;

            bool GlobalStopCondition() 
            {
                // Verifica uma condição de interrupção global (por exemplo, uma variável de jogo)
                if (_bossCollider == null) return false;
                var hp = _bossCollider.GetHp();
                return hp <= 0;
            }

            NodeState StopTree()
            {
                //Placeholder: Aqui vai a ação de finalização no caso morte.
                return NodeState.Success;
            }
        }

        // Método para resetar a árvore
        private void ResetTree()
        {
            if (_tree == null) return;
            Debug.Log("Resetando a árvore de comportamento");
            _tree = null;  // Limpa a árvore atual
            CreateBossBehaviors();  // Recria a árvore de comportamento do zero
        }

        // Método para resetar todo o comportamento, incluindo os nós e a árvore
        public void ResetAll()
        {
            // Resetar comportamentos individuais
            var enterScene = GetComponent<BossBehaviorEnterScene>();
            var singleShoot = GetComponentByID<BossBehaviorSingleShoot>(1) as BossBehaviorSingleShoot;
            var coneShoot = GetComponentByID<BossBehaviorConeShoot>(1) as BossBehaviorConeShoot;
            var coneShoot02 = GetComponentByID<BossBehaviorConeShoot>(2) as BossBehaviorConeShoot;
            var mineSpawn = GetComponentByID<BossBehaviorRandomSpawn>(1)as BossBehaviorRandomSpawn;
            var movement = GetComponent<BossBehaviorMovement>();
            var emerge = GetComponent<BossBehaviorEmerge>();
            var submerge = GetComponent<BossBehaviorSubmerge>();
            var death = GetComponent<BossBehaviorDeath>();
            var finish = GetComponent<BossBehaviorFinishGame>();

            enterScene?.ResetBehavior();  // Resetar o EnterScene
            singleShoot?.ResetBehavior();
            coneShoot?.ResetBehavior();
            coneShoot02?.ResetBehavior();
            mineSpawn?.ResetBehavior();
            movement?.ResetBehavior();
            emerge?.ResetBehavior();
            submerge?.ResetBehavior();
            death?.ResetBehavior();
            finish?.ResetBehavior();

            // Resetar a árvore de comportamento
            ResetTree();
        }

    }
}
