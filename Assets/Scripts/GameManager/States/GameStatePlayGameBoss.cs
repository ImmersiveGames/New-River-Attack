using System.Collections;
using System.Collections.Generic;
using NewRiverAttack.SteamGameManagers;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public enum BattleBossSubState { Top, Base, Left, Right, Dead }
    public class GameStatePlayGameBoss : GameState
    {
        private BattleBossSubState m_CurrentSubState;
        private readonly Dictionary<BattleBossSubState, IBossBehavior[]> m_Behaviors;

        private IBossBehavior[] m_CurrentBehaviors;
        private int m_CurrentBehaviorIndex;
        private bool m_BehaviorEnterExecuted;
        private BossMasterOld _mBossMasterOld;

        public GameStatePlayGameBoss() {
            // Inicializa o estado do BattleBoss como "Topo"
            m_CurrentSubState = BattleBossSubState.Top;
            m_CurrentBehaviorIndex = 0;

            _mBossMasterOld = GamePlayManager.instance.BossMasterOld;
    
            // Inicializa os comportamentos para cada subestado
            m_Behaviors = new Dictionary<BattleBossSubState, IBossBehavior[]> {
                { BattleBossSubState.Top, new IBossBehavior[] {
                    new EmergeBehavior(_mBossMasterOld),
                    new ExplosiveMinesBehavior(_mBossMasterOld,_mBossMasterOld.GetBossMines().numMines[0]),
                    new MissileAttackBehavior(_mBossMasterOld, _mBossMasterOld.GetBossMissileShoot().numMissiles[0], _mBossMasterOld.GetBossMissileShoot().angleCones[0]),
                    new SubEmergeBehavior(_mBossMasterOld),
                    new DropGasStationsBehavior(_mBossMasterOld, _mBossMasterOld.GetBossGasStationDrop().dropGasStation[0])
                    // Adicionar os outros comportamentos para o subestado "Topo"
                }},
                { BattleBossSubState.Base, new IBossBehavior[] {
                    new EmergeBehavior(_mBossMasterOld),
                    new MissileAttackBehavior(_mBossMasterOld, _mBossMasterOld.GetBossMissileShoot().numMissiles[1], _mBossMasterOld.GetBossMissileShoot().angleCones[1]),
                    new SubEmergeBehavior(_mBossMasterOld)
                    // Adicionar os outros comportamentos para o subestado "Base"
                }},
                { BattleBossSubState.Left, new IBossBehavior[] {
                    new EmergeBehavior(_mBossMasterOld),
                    new MissileAttackBehavior(_mBossMasterOld,_mBossMasterOld.GetBossMissileShoot().numMissiles[0], _mBossMasterOld.GetBossMissileShoot().angleCones[0]),
                    new SubEmergeBehavior(_mBossMasterOld),
                    new DropGasStationsBehavior(_mBossMasterOld, _mBossMasterOld.GetBossGasStationDrop().dropGasStation[0])
                    // Adicionar os outros comportamentos para o subestado "Left"
                }},
                { BattleBossSubState.Right, new IBossBehavior[] {
                    new EmergeBehavior(_mBossMasterOld),
                    new MissileAttackBehavior(_mBossMasterOld,_mBossMasterOld.GetBossMissileShoot().numMissiles[0], _mBossMasterOld.GetBossMissileShoot().angleCones[0]),
                    new SubEmergeBehavior(_mBossMasterOld),
                    new DropGasStationsBehavior(_mBossMasterOld, _mBossMasterOld.GetBossGasStationDrop().dropGasStation[0])
                    // Adicionar os outros comportamentos para o subestado "Right"
                }},
                { BattleBossSubState.Dead, new IBossBehavior[] {
                    new DeadBehavior(_mBossMasterOld)
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
            GameManager.instance.inputSystem.Player.Enable();
            GameManager.instance.inputSystem.UiControls.Disable();
            GameManager.instance.inputSystem.BriefingRoom.Disable();
            GamePlayManager.instance.panelMenuGame.StartMenuGame();
            GamePlayManager.instance.OnStartGame();
        }
        public override void UpdateState()
        {
            //Debug.Log($"Boss Fight! {GamePlayManager.instance.bossFightPause}");
            /*Debug.Log($"m_CurrentSubState: {m_CurrentSubState}");
            Debug.Log($"m_CurrentBehaviorIndex: {m_CurrentBehaviorIndex}");*/
            if (!_mBossMasterOld.shouldObstacleBeReady)
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
            var score = PlayerManager.instance.playerSettingsList[0].score;
            //SteamGameManager.UpdateScore(score, false);
            //PlayerManager.instance.ActivePlayers(false);
            //GamePlayManager.instance.OnEventDeactivateEnemiesMaster();
            //Debug.Log($"Sai do Estado: Boss Fight");
            //m_BossMaster = null;
            GameSaveManager.instance.SavePlayerSaves();
            System.GC.Collect();
        }
        /*public static void PauseState(bool active)
        {
            GamePlayManager.instance.PauseBossBattle(active);
        }*/

        private BattleBossSubState GetNextSubState() {
            
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
                var randomIndex = Random.Range(0, 4);
                nextState = (BattleBossSubState)randomIndex;
            }
            while (nextState == m_CurrentSubState); // Garante que o próximo estado seja diferente do atual

            _mBossMasterOld.actualPosition = nextState;
           // Debug.Log($"RANDOM Position: {nextState}");
            return nextState;
        }

        private void ChangeSubState(BattleBossSubState newSubState)
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
