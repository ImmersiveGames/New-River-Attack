using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public enum BattleBossSubState { Top, Base, Left, Right, Dead }
    public class GameStatePlayGameBoss : GameState
    {
        BattleBossSubState m_CurrentSubState;
        readonly Dictionary<BattleBossSubState, IBossBehavior[]> m_Behaviors;

        IBossBehavior[] m_CurrentBehaviors;
        int m_CurrentBehaviorIndex;
        bool m_BehaviorEnterExecuted;
        BossMaster m_BossMaster;

        public GameStatePlayGameBoss() {
            // Inicializa o estado do BattleBoss como "Topo"
            m_CurrentSubState = BattleBossSubState.Top;
            m_CurrentBehaviorIndex = 0;

            m_BossMaster = GamePlayManager.instance.bossMaster;
    
            // Inicializa os comportamentos para cada subestado
            m_Behaviors = new Dictionary<BattleBossSubState, IBossBehavior[]> {
                { BattleBossSubState.Top, new IBossBehavior[] {
                    new EmergeBehavior(m_BossMaster),
                    new ExplosiveMinesBehavior(m_BossMaster,m_BossMaster.GetBossMines().numMines[0]),
                    new MissileAttackBehavior(m_BossMaster, m_BossMaster.GetBossMissileShoot().numMissiles[0], m_BossMaster.GetBossMissileShoot().angleCones[0]),
                    new SubEmergeBehavior(m_BossMaster),
                    new DropGasStationsBehavior(m_BossMaster, m_BossMaster.GetBossGasStationDrop().dropGasStation[0])
                    // Adicionar os outros comportamentos para o subestado "Topo"
                }},
                { BattleBossSubState.Base, new IBossBehavior[] {
                    new EmergeBehavior(m_BossMaster),
                    new MissileAttackBehavior(m_BossMaster, m_BossMaster.GetBossMissileShoot().numMissiles[1], m_BossMaster.GetBossMissileShoot().angleCones[1]),
                    new SubEmergeBehavior(m_BossMaster)
                    // Adicionar os outros comportamentos para o subestado "Base"
                }},
                { BattleBossSubState.Left, new IBossBehavior[] {
                    new EmergeBehavior(m_BossMaster),
                    new MissileAttackBehavior(m_BossMaster,m_BossMaster.GetBossMissileShoot().numMissiles[0], m_BossMaster.GetBossMissileShoot().angleCones[0]),
                    new SubEmergeBehavior(m_BossMaster),
                    new DropGasStationsBehavior(m_BossMaster, m_BossMaster.GetBossGasStationDrop().dropGasStation[0])
                    // Adicionar os outros comportamentos para o subestado "Left"
                }},
                { BattleBossSubState.Right, new IBossBehavior[] {
                    new EmergeBehavior(m_BossMaster),
                    new MissileAttackBehavior(m_BossMaster,m_BossMaster.GetBossMissileShoot().numMissiles[0], m_BossMaster.GetBossMissileShoot().angleCones[0]),
                    new SubEmergeBehavior(m_BossMaster),
                    new DropGasStationsBehavior(m_BossMaster, m_BossMaster.GetBossGasStationDrop().dropGasStation[0])
                    // Adicionar os outros comportamentos para o subestado "Right"
                }},
                { BattleBossSubState.Dead, new IBossBehavior[] {
                    new DeadBehavior(m_BossMaster)
                    //new SubEmergeBehavior(m_BossMaster),
                    // Adicionar os outros comportamentos para o subestado "Right"
                }}
            };
            
        }
        public override IEnumerator OnLoadState()
        {
            //TODO: Play BGM COMBAT
            yield return null;
        }
        public override void EnterState()
        {
            //Debug.Log($"Entra no Estado: Boss Fight");
            GamePlayManager.instance.panelMenuGame.StartMenuGame();
            GamePlayManager.instance.OnStartGame();
        }
        public override void UpdateState()
        {
            Debug.Log($"Boss Fight! {GamePlayManager.instance.bossFightPause}");
            /*Debug.Log($"m_CurrentSubState: {m_CurrentSubState}");
            Debug.Log($"m_CurrentBehaviorIndex: {m_CurrentBehaviorIndex}");*/
            if (!m_BossMaster.shouldObstacleBeReady)
            {
                return;
            }
                
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
            //Verificar gameover
        }
        public override void ExitState()
        {
            int score = PlayerManager.instance.playerSettingsList[0].score;
            GameSteamManager.UpdateScore(score, false);
            PlayerManager.instance.ActivePlayers(false);
            GamePlayManager.instance.OnEventDeactivateEnemiesMaster();
            //Debug.Log($"Sai do Estado: Boss Fight");
            m_BossMaster = null;
            GameSaveManager.instance.SavePlayerSaves();
            System.GC.Collect();
        }
        public static void PauseState(bool active)
        {
            GamePlayManager.instance.PauseBossBattle(active);
        }

        BattleBossSubState GetNextSubState() {
            
            BattleBossSubState nextState;

            // Verifica se todos os comportamentos do subestado atual foram concluídos
            if (m_CurrentBehaviors == null)
            {
                m_CurrentBehaviors = m_Behaviors[m_CurrentSubState];
                return m_CurrentSubState;
            }
            if (m_CurrentBehaviorIndex < m_CurrentBehaviors.Length)
                return m_CurrentSubState;

            do
            {
                int randomIndex = Random.Range(0, 4);
                nextState = (BattleBossSubState)randomIndex;
            }
            while (nextState == m_CurrentSubState); // Garante que o próximo estado seja diferente do atual

            m_BossMaster.actualPosition = nextState;
           // Debug.Log($"RANDOM Position: {nextState}");
            return nextState;
        }
        void ChangeSubState(BattleBossSubState newSubState)
        {
            if (m_CurrentSubState != newSubState)
            {
                // Se for um substatus diferente deve trocar e reiniciar
                m_CurrentSubState = newSubState;
                m_BehaviorEnterExecuted = false;
                m_CurrentBehaviorIndex = 0;
                m_CurrentBehaviors = m_Behaviors[m_CurrentSubState];
            }
            if (m_BehaviorEnterExecuted)
                return;
            m_CurrentBehaviors[m_CurrentBehaviorIndex].Enter();
            m_BehaviorEnterExecuted = true;
        }

        internal void FinishBehavior()
        {
            m_CurrentBehaviors[m_CurrentBehaviorIndex].FinishBehavior();
        }
    }
}
