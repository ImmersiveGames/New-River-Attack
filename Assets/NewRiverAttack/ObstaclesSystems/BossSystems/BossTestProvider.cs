using System;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossTestProvider : MonoBehaviour, INodeFunctionProvider
    {
        public Func<NodeState> GetNodeFunction()
        {
            return Teste;  // Função de movimento
        }

        private NodeState Teste()
        {
            Debug.Log("Teste o objeto...");
            return NodeState.Success;
        }

        public string NodeName => "Teste";
    }
}