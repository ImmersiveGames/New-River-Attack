using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public enum BattleBossSubState { Top, Base, Left, Right }
    public class GameStatePlayGameBoss : GameState
    {
        internal BattleBossSubState currentSubState;
        readonly Dictionary<BattleBossSubState, IBossBehavior[]> m_Behaviors;

        IBossBehavior[] m_CurrentBehaviors;
        int m_CurrentBehaviorIndex;
        bool m_BehaviorEnterExecuted;

        BossMaster m_BossMaster;
        PlayerMaster m_PlayerMaster;

        public GameStatePlayGameBoss() {
            // Inicializa o estado do BattleBoss como "Topo"
            currentSubState = BattleBossSubState.Top;
            m_CurrentBehaviorIndex = 0;

            m_BossMaster = GamePlayManager.instance.bossMaster;
            m_PlayerMaster = PlayerManager.instance.initializedPlayerMasters[0];

            // Inicializa os comportamentos para cada subestado
            m_Behaviors = new Dictionary<BattleBossSubState, IBossBehavior[]> {
                { BattleBossSubState.Top, new IBossBehavior[] {
                    new EmergeBehavior(m_BossMaster),
                    new MissileAttackBehavior(m_BossMaster),
                    new SubEmergeBehavior(m_BossMaster)
                    // Adicionar os outros comportamentos para o subestado "Topo"
                }},
                { BattleBossSubState.Base, new IBossBehavior[] {
                    new EmergeBehavior(m_BossMaster),
                    new MissileAttackBehavior(m_BossMaster),
                    new SubEmergeBehavior(m_BossMaster)
                    // Adicionar os outros comportamentos para o subestado "Base"
                }},
                { BattleBossSubState.Left, new IBossBehavior[] {
                    new EmergeBehavior(m_BossMaster),
                    new MissileAttackBehavior(m_BossMaster),
                    new SubEmergeBehavior(m_BossMaster)
                    // Adicionar os outros comportamentos para o subestado "Left"
                }},
                { BattleBossSubState.Right, new IBossBehavior[] {
                    new EmergeBehavior(m_BossMaster),
                    new MissileAttackBehavior(m_BossMaster),
                    new SubEmergeBehavior(m_BossMaster)
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
            //Debug.Log($"Entra no Estado: Boss Fight");
            GamePlayManager.instance.panelMenuGame.StartMenuGame();
            GamePlayManager.instance.OnStartGame();
            m_PlayerMaster = PlayerManager.instance.initializedPlayerMasters[0];
        }
        public override void UpdateState()
        {
            //Debug.Log("Boss Fight!");
            ExecuteCurrentBehavior();
            //Verificar gameover
        }
        public override void ExitState()
        {
            PlayerManager.instance.ActivePlayers(false);
            GamePlayManager.instance.OnEventDeactivateEnemiesMaster();
           
            //Debug.Log($"Sai do Estado: Boss Fight");
            m_BossMaster = null;
            m_PlayerMaster = null;
            System.GC.Collect();
        }

        BattleBossSubState GetNextSubState() {
            // Verifica se todos os comportamentos do subestado atual foram concluídos
            if (m_CurrentBehaviors == null)
            {
                m_CurrentBehaviors = m_Behaviors[currentSubState];
                return currentSubState;
            }
            if (m_CurrentBehaviorIndex < m_CurrentBehaviors.Length)
                return currentSubState;

            int randomIndex = Random.Range(0, 4); // Sorteia um número aleatório entre 0 e 3 (4 possíveis estados)

            // Converte o número aleatório em um BattleBossSubState correspondente
            var nextState = (BattleBossSubState)randomIndex;
            m_BossMaster.actualPosition = nextState;
            Debug.Log($"RANDOM Position: {nextState}");
            return nextState;
        }
        void ChangeSubState(BattleBossSubState newSubState)
        {
            if (currentSubState != newSubState)
            {
                // Se for um substatus diferente deve trocar e reiniciar
                currentSubState = newSubState;
                m_BehaviorEnterExecuted = false;
                m_CurrentBehaviorIndex = 0;
                m_CurrentBehaviors = m_Behaviors[currentSubState];
            }
            if (m_BehaviorEnterExecuted)
                return;
            m_CurrentBehaviors[m_CurrentBehaviorIndex].Enter();
            m_BehaviorEnterExecuted = true;
        }
        
        void ExecuteCurrentBehavior() {
            /*Debug.Log($"m_CurrentSubState: {m_CurrentSubState}");
            Debug.Log($"m_CurrentBehaviorIndex: {m_CurrentBehaviorIndex}");*/

            //Se for o primeiro e tiver null atualiza os comportamentos, se não for o primeiro verifica se é o ultimo comportamento apra alterar
            ChangeSubState(GetNextSubState());
            
            if (m_BehaviorEnterExecuted)
            {
                m_CurrentBehaviors[m_CurrentBehaviorIndex].Update();
            }
            if (!m_CurrentBehaviors[m_CurrentBehaviorIndex].IsFinished())
                return;
            // Se o comportamento se encerra executa o ultimo update e sua saida.
            m_CurrentBehaviors[m_CurrentBehaviorIndex].Exit();
            m_CurrentBehaviorIndex++;
            m_BehaviorEnterExecuted = false;

            /*Debug.Log($"N# Behavior: {m_CurrentBehaviors.Length}");
            Debug.Log($"m_BehaviorCompleted: {m_BehaviorCompleted}");
            Debug.Log($"m_BehaviorEnterExecuted: {m_BehaviorEnterExecuted}");*/
        }

        internal void FinishBehavior()
        {
            m_CurrentBehaviors[m_CurrentBehaviorIndex].FinishBehavior();
        }
    }
}
