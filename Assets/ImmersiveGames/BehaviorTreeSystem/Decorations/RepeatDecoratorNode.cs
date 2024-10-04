using ImmersiveGames.BehaviorTreeSystem.Interface;

namespace ImmersiveGames.BehaviorTreeSystem.Decorations
{
    public class RepeatDecoratorNode : INode
    {
        private readonly INode _node;  // O nó encapsulado
        private readonly int _times;   // Número de repetições
        private int _counter;          // Contador para rastrear quantas vezes o nó foi repetido

        public RepeatDecoratorNode(INode node, int times)
        {
            _node = node;
            _times = times;
            _counter = 0;
        }

        public void OnEnter()
        {
            _counter = 0;  // Reinicializa o contador quando o nó começa
            _node.OnEnter();
        }

        public NodeState Tick()
        {
            // Se o nó já foi repetido o número máximo de vezes, retorna sucesso
            if (_counter >= _times) return NodeState.Success;

            var result = _node.Tick();

            // Se o nó terminar com sucesso ou falha, incrementa o contador e reinicia o nó
            if (result != NodeState.Success && result != NodeState.Failure)
                return NodeState.Running; // Enquanto estiver repetindo, retorna Running
            _node.OnExit();
            _counter++;
            if (_counter < _times)
            {
                _node.OnEnter();
            }

            return NodeState.Running;  // Enquanto estiver repetindo, retorna Running
        }

        public void OnExit()
        {
            _node.OnExit();  // Executa o OnExit do nó encapsulado quando sair
        }
    }
}