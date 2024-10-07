using System;
using DG.Tweening;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorEnterScene : MonoBehaviour, INodeFunctionProvider
    {
        public float initialDelay = 3f;
        public float moveDuration = 5f;
        public float distanceOffset = 50f;

        private NodeState _currentState = NodeState.Running; // O estado começa como Running
        private BossMaster _bossMaster;
        private Tween _moveTween;  // Armazena o Tween para reutilizar e reiniciar
        private bool _hasCompleted; // Controle para evitar que OnKill e OnComplete se conflitem

        private void Start()
        {
            _bossMaster = GetComponent<BossMaster>();
        }

        // Método para reinicializar o estado do nó, usado no reset
        public void OnEnter()
        { 
            _currentState = NodeState.Running;  // Reinicia o estado para Running ao entrar
            _moveTween?.Kill();  // Cancela qualquer Tween anterior, se estiver ativo
            _moveTween = null;   // Reseta o Tween
            _hasCompleted = false; // Reseta o controle de conclusão
        }

        // Método para resetar o comportamento (será chamado no ResetAll)
        public void ResetEnterScene()
        {
            OnEnter();  // Reutiliza o método OnEnter para resetar o estado e a animação
            StartSetup();  // Reposiciona o boss no início
        }

        // Método que controla a entrada do Boss na cena
        private NodeState EnterScene()
        {
            // Se a animação já tiver concluído ou falhado, retorna o estado final
            if (_currentState is NodeState.Success or NodeState.Failure)
            {
                return _currentState;  // Sai com o estado final
            }

            // Se o estado for Running e não houver Tween ativo, inicia a animação
            if (_currentState == NodeState.Running && _moveTween == null)
            {
                _bossMaster.IsEmerge = false;

                // Configurar e iniciar a sequência de animação
                var playerPosition = _bossMaster.PlayerMaster.transform.position;
                var distance = playerPosition.z + distanceOffset;

                var mySequence = DOTween.Sequence();
                mySequence.AppendInterval(initialDelay);

                // Callback para atualizar a posição antes de mover
                mySequence.AppendCallback(() =>
                {
                    transform.position = new Vector3(playerPosition.x, transform.position.y, transform.position.z);
                });

                // Movimento principal com DOTween e armazenar o Tween
                _moveTween = transform.DOMoveZ(distance, moveDuration).SetEase(Ease.Linear);

                // Adicionar o Tween ao Sequence
                mySequence.Append(_moveTween);

                // Ao concluir a animação, definir o estado como Success
                mySequence.OnComplete(() =>
                {
                    if (_hasCompleted) return; // Garante que só seja chamado uma vez
                    _currentState = NodeState.Success;
                    _bossMaster.IsEmerge = true;
                    _moveTween = null;  // Resetar o Tween
                    _hasCompleted = true;  // Marca que a animação foi completada
                });

                // Caso a animação seja interrompida (seu mySequence for "Killed"), definir o estado como Failure
                mySequence.OnKill(() =>
                {
                    if (_hasCompleted) return;  // Garante que só seja chamado se OnComplete não foi chamado
                    _currentState = NodeState.Failure;
                    _moveTween = null;  // Resetar o Tween
                });

                // Inicia a sequência
                mySequence.Play();
            }

            // Enquanto a animação está em andamento, retorna Running
            return NodeState.Running;
        }

        // Método para configurar o início do comportamento
        private void StartSetup()
        {
            _bossMaster.IsEmerge = false;
            transform.position = Vector3.zero;  // Reposiciona o Boss no início
            _bossMaster.OnEventBossResetForEnter();
        }

        // Retorna a função que será chamada pelo nó de comportamento
        public Func<NodeState> GetNodeFunction()
        {
            return EnterScene;
        }

        // Nome do nó, conforme a interface INodeFunctionProvider
        public string NodeName => "EnterScene";
        public int NodeID => 0;
    }
}
