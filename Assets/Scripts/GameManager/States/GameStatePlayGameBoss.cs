using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class GameStatePlayGameBoss : GameState
    {
        public enum BattleBossSubState { Top, Base, Left, Right }
        BattleBossSubState m_CurrentSubState;
        readonly Dictionary<BattleBossSubState, IBossBehavior[]> m_Behaviors;

        readonly IBossBehavior[] m_CurrentBehaviors;
        int m_CurrentBehaviorIndex;
        bool m_BehaviorCompleted;
        bool m_BehaviorEnterExecuted = false;

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

            if (m_Behaviors.TryGetValue(m_CurrentSubState, out m_CurrentBehaviors)) {
                m_CurrentBehaviors[m_CurrentBehaviorIndex].Enter();
            }
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
            if (m_CurrentBehaviorIndex < m_CurrentBehaviors.Length)
                return m_CurrentSubState;
            int randomIndex = Random.Range(0, 4); // Sorteia um número aleatório entre 0 e 3 (4 possíveis estados)

            // Converte o número aleatório em um BattleBossSubState correspondente
            var nextState = (BattleBossSubState)randomIndex;

            return nextState;
        }
        
        void ChangeSubState(BattleBossSubState newSubState)
        {
            if (m_CurrentSubState != newSubState || !m_Behaviors.ContainsKey(newSubState))
            {
                m_CurrentSubState = newSubState;
                m_BehaviorCompleted = false;
                m_BehaviorEnterExecuted = false;
                m_CurrentBehaviorIndex = 0;

                if (m_CurrentBehaviors is not { Length: > 0 })
                    return;
                var currentBehavior = m_CurrentBehaviors[m_CurrentBehaviorIndex];
                currentBehavior.Enter();
                m_BehaviorEnterExecuted = true;
            }
        }
        void ExecuteCurrentBehavior() {
            Debug.Log($"BattleBossSubState: {m_CurrentSubState}");
            Debug.Log($"m_CurrentBehaviorIndex: {m_CurrentBehaviorIndex}");
            
            if (m_CurrentBehaviors == null || m_CurrentBehaviorIndex >= m_CurrentBehaviors.Length) {
                var nextSubState = GetNextSubState();
                ChangeSubState(nextSubState);
                return;
            }

            var currentBehavior = m_CurrentBehaviors[m_CurrentBehaviorIndex];

            if (m_BehaviorCompleted) return;

            if (!m_BehaviorEnterExecuted) {
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
            }
        }
    }
}
