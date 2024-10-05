using System;

namespace ImmersiveGames.BehaviorTreeSystem.Interface
{
    public interface INodeFunctionProvider
    {
        // Retorna a função que será usada pelo nó
        Func<NodeState> GetNodeFunction();

        // Nome identificador para o nó, caso seja necessário para referências futuras
        string NodeName { get; }
        int NodeID { get; }
    }
}