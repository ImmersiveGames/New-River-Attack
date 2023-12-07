using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public enum BattleBossSubState { Top, Base, Left, Right }
    public class GameStatePlayGameBoss : GameState
    {
        BattleBossSubState m_CurrentSubState;
        readonly Dictionary<BattleBossSubState, IBossBehavior[]> m_Behaviors;

        IBossBehavior[] m_CurrentBehaviors;
        int m_CurrentBehaviorIndex;
        bool m_BehaviorCompleted;
        bool m_BehaviorEnterExecuted;

        public GameStatePlayGameBoss() {
            // Inicializa o estado do BattleBoss como "Topo"
            m_CurrentSubState = BattleBossSubState.Top;
            m_CurrentBehaviorIndex = 0;
            m_BehaviorCompleted = false;

            // Inicializa os comportamentos para cada subestado
            m_Behaviors = new Dictionary<BattleBossSubState, IBossBehavior[]> {
                { BattleBossSubState.Top, new IBossBehavior[] {
                    new EmergeBehavior(),
                    new MissileAttackBehavior(),
                    new SubEmergeBehavior()
                    // Adicionar os outros comportamentos para o subestado "Topo"
                }},
                { BattleBossSubState.Base, new IBossBehavior[] {
                    new EmergeBehavior(),
                    new MissileAttackBehavior(),
                    new SubEmergeBehavior()
                    // Adicionar os outros comportamentos para o subestado "Base"
                }},
                { BattleBossSubState.Left, new IBossBehavior[] {
                    new EmergeBehavior(),
                    new MissileAttackBehavior(),
                    new SubEmergeBehavior()
                    // Adicionar os outros comportamentos para o subestado "Left"
                }},
                { BattleBossSubState.Right, new IBossBehavior[] {
                    new EmergeBehavior(),
                    new MissileAttackBehavior(),
                    new SubEmergeBehavior()
                    // Adicionar os outros comportamentos para o subestado "Right"
                }}
            };
            
        }
        public override IEnumerator OnLoadState()
        {
            yield return null;
        }
        public override void EnterState()
        {
            Debug.Log($"Entra no Estado: Boss Fight");
            GamePlayManager.instance.panelMenuGame.StartMenuGame();
            GamePlayManager.instance.OnStartGame();
        }
        public override void UpdateState()
        {
            Debug.Log("Boss Fight!");
            ExecuteCurrentBehavior();
            //Verificar gameover
        }
        public override void ExitState()
        {
            PlayerManager.instance.ActivePlayers(false);
            GamePlayManager.instance.OnEventDeactivateEnemiesMaster();
           
            Debug.Log($"Sai do Estado: Boss Fight");
            System.GC.Collect();
        }

        BattleBossSubState GetNextSubState() {
            // Verifica se todos os comportamentos do subestado atual foram concluídos
            if (m_CurrentBehaviors == null)
            {
                m_CurrentBehaviors = m_Behaviors[m_CurrentSubState];
                return m_CurrentSubState;
            }
            if (m_CurrentBehaviorIndex < m_CurrentBehaviors.Length)
                return m_CurrentSubState;
            
            
            int randomIndex = Random.Range(0, 4); // Sorteia um número aleatório entre 0 e 3 (4 possíveis estados)

            // Converte o número aleatório em um BattleBossSubState correspondente
            var nextState = (BattleBossSubState)randomIndex;

            return nextState;
        }
        void ChangeSubState(BattleBossSubState newSubState)
        {
            Debug.Log($"ChangeSubState - newSubState: {newSubState}");
            Debug.Log($"ChangeSubState - m_CurrentSubState: {m_CurrentSubState}");

            if (m_CurrentSubState != newSubState)
            {
                // Se for um substatus diferente deve trocar e reiniciar
                m_CurrentSubState = newSubState;
                m_BehaviorCompleted = false;
                m_BehaviorEnterExecuted = false;
                m_CurrentBehaviorIndex = 0;
                m_CurrentBehaviors = m_Behaviors[m_CurrentSubState];
            }
            if (!m_BehaviorEnterExecuted)
            {
                m_CurrentBehaviors[m_CurrentBehaviorIndex].Enter();
                m_BehaviorEnterExecuted = true;
            }
            
            /*if (m_CurrentSubState != newSubState || !m_Behaviors.ContainsKey(newSubState))
            {
                m_CurrentSubState = newSubState;
                m_BehaviorCompleted = false;
                m_BehaviorEnterExecuted = false;
                m_CurrentBehaviorIndex = 0;

                if (m_Behaviors.TryGetValue(m_CurrentSubState, out m_CurrentBehaviors))
                {
                    if (m_CurrentBehaviors.Length > 0)
                    {
                        var currentBehavior = m_CurrentBehaviors[m_CurrentBehaviorIndex];
                        currentBehavior.Enter();
                        m_BehaviorEnterExecuted = true;
                        Debug.Log($"BattleBossSubState: {m_CurrentSubState}");
                    }
                    else
                    {
                        Debug.LogError("Comportamentos atuais não estão definidos ou vazios.");
                    }
                }
                else
                {
                    Debug.LogError($"O subestado {m_CurrentSubState} não foi encontrado nos comportamentos.");
                }
            }*/
        }
        
        void ExecuteCurrentBehavior() {
            Debug.Log($"m_CurrentSubState: {m_CurrentSubState}");
            Debug.Log($"m_CurrentBehaviorIndex: {m_CurrentBehaviorIndex}");
            
            //Se for o primeiro e tiver null atualiza os comportamentos, se não for o primeiro verifica se é o ultimo comportamento apra alterar
            ChangeSubState(GetNextSubState());
            
            if (m_BehaviorEnterExecuted)
            {
                m_CurrentBehaviors[m_CurrentBehaviorIndex].Update();
            }
            if (m_CurrentBehaviors[m_CurrentBehaviorIndex].IsFinished())
            {
                // Se o comportamento se encerra executa o ultimo update e sua saida.
                m_CurrentBehaviors[m_CurrentBehaviorIndex].Exit();
                m_CurrentBehaviorIndex++;
                m_BehaviorEnterExecuted = false;
            }

            Debug.Log($"N# Behavior: {m_CurrentBehaviors.Length}");
            Debug.Log($"m_BehaviorCompleted: {m_BehaviorCompleted}");
            Debug.Log($"m_BehaviorEnterExecuted: {m_BehaviorEnterExecuted}");
            
            /*if (m_CurrentBehaviors == null || m_CurrentBehaviorIndex >= m_CurrentBehaviors.Length) {
                var nextSubState = GetNextSubState();
                ChangeSubState(nextSubState);
                return;
            }

            var currentBehavior = m_CurrentBehaviors[m_CurrentBehaviorIndex];

            if (m_BehaviorCompleted) return;

            if (!m_BehaviorEnterExecuted) {
                currentBehavior.Enter();
                m_BehaviorEnterExecuted = true;
            } else {
                currentBehavior.Update();
                m_BehaviorCompleted = currentBehavior.IsFinished();

                if (!m_BehaviorCompleted)
                    return;

                m_CurrentBehaviorIndex++;

                // Verifica se todos os comportamentos do subestado atual foram concluídos
                if (m_CurrentBehaviorIndex >= m_CurrentBehaviors.Length) {
                    var nextSubState = GetNextSubState();
                    ChangeSubState(nextSubState);
                } else {
                    currentBehavior.Exit();
                    m_BehaviorEnterExecuted = false;
                    m_BehaviorCompleted = false;
                }
            }*/
        }
    }
}
