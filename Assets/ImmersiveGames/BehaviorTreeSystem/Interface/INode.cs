namespace ImmersiveGames.BehaviorTreeSystem.Interface
{
    public interface INode
    {
        void OnEnter(); // Chamado quando o nó é ativado
        NodeState Tick(); // Chamado a cada frame para verificar a execução
        void OnExit(); // Chamado quando o nó é desativado (quando finaliza ou falha)
    }

    public enum NodeState
    {
        Running,
        Success,
        Failure
    }

    public enum NodeTypes
    {
        ActionNode,
        SequenceNode,
        RandomSelectorNode,
        WaitNode
    }

    public enum NodeParam
    {
        Action,
        NodeName,
        Nodes,
        RandomMaxTimes,
        WaitTime
    }

    public enum NodeDecorations
    {
        OnEnterDecorator,
        OnExitDecorator,
        DelayDecorator,
        RepeatDecorator,
        ParallelConditionDecorator,
        GlobalConditionDecorator
    }

    public enum NodeDecorationsParam
    {
        OnEnter,
        OnExit,
        Delay,
        Times,
        ParallelCondition,
        ParallelActionNode,
        GlobalCondition,
        GlobalOverrideNode
    }
}