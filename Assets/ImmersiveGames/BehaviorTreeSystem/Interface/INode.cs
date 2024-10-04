namespace ImmersiveGames.BehaviorTreeSystem.Interface
{
    public interface INode
    {
        void OnEnter();  // Chamado quando o nó é ativado
        NodeState Tick();  // Chamado a cada frame para verificar a execução
        void OnExit();   // Chamado quando o nó é desativado (quando finaliza ou falha)
    }
    public enum NodeState { Running, Success, Failure }
}