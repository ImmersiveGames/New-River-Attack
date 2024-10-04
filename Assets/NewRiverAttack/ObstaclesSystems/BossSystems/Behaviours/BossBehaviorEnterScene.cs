using System;
using DG.Tweening;
using ImmersiveGames.BehaviorTreeSystem.Interface;
using NewRiverAttack.PlayerManagers.Tags;
using UnityEngine;
using UnityEngine.Serialization;

namespace NewRiverAttack.ObstaclesSystems.BossSystems.Behaviours
{
    public class BossBehaviorEnterScene : MonoBehaviour, INodeFunctionProvider
    {
        public float initialDelay = 3f;
        public float moveDuration = 5f;
        public float distanceOffset = 50f;
        
        //TODO: Precisa de uma forma re reiniciar a animação sempre que o Enter scene for iniciado.

        private NodeState _currentState = NodeState.Running; // Inicialmente o estado é Idle (em vez de Running)
        private BossMaster _bossMaster;
        private Tween _moveTween; // Armazenar o Tween para reutilizar

        private void Start()
        {
            _bossMaster = GetComponent<BossMaster>();
        }
        
        

        private NodeState EnterScene()
        {
            switch (_currentState)
            {
                // Checa se o estado já foi concluído com sucesso ou falha
                case NodeState.Success or NodeState.Failure:
                    return _currentState;  // Retorna o estado final atual sem reiniciar
                // Iniciar animação apenas se estiver em Running e não houver Tween ativo
                case NodeState.Running when _moveTween == null:
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
                        _currentState = NodeState.Success;
                        _bossMaster.IsEmerge = true;
                        _moveTween = null; // Resetar o Tween
                    });

                    // Caso a animação seja interrompida (seu mySequence for "Killed"), definir o estado como Failure
                    mySequence.OnKill(() =>
                    {
                        _currentState = NodeState.Failure;
                        _moveTween = null; // Resetar o Tween
                    });

                    // Inicia a sequência
                    mySequence.Play();
                    break;
                }
            }

            // Enquanto a animação está em andamento, retorna Running
            return NodeState.Running;
        }

        public void StartSetup()
        {
            _bossMaster.IsEmerge = false;
            transform.position = Vector3.zero;
            _bossMaster.OnEventBossResetForEnter();

        }
        // Retorna a função que será chamada pelo nó de comportamento
        public Func<NodeState> GetNodeFunction()
        {
            return EnterScene;
        }

        // Nome do nó, conforme a interface INodeFunctionProvider
        public string NodeName => "EnterScene";
    }
}
