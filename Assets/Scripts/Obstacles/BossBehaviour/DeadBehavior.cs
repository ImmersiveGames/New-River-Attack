using NewRiverAttack.SteamGameManagers;
using Utils;
namespace RiverAttack
{
    public class DeadBehavior: IBossBehavior
    {
        private readonly BossMasterOld _mBossMasterOld;
        private bool m_Finished;
        internal DeadBehavior(BossMasterOld bossMasterOld)
        {
            _mBossMasterOld = bossMasterOld;
        }
        public void Enter()
        {
            //Debug.Log("Enter in DeadBehavior");
            SteamGameManager.UnlockAchievement("ACH_BEAT_SUBMARINE");
        }
        public void Update()
        {
            //Debug.Log("DeadBehavior!!");
        }
        public void Exit()
        {
            //Debug.Log("Leave the DeadBehavior");
        }
        public void FinishBehavior()
        {
            m_Finished = true;
            //Fazer o GameOver
        }
        public bool IsFinished()
        {
            return m_Finished;
        }
    }
}
