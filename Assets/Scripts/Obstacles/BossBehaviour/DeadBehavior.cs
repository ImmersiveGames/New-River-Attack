using Utils;
namespace RiverAttack
{
    public class DeadBehavior: IBossBehavior
    {
        private readonly BossMaster m_BossMaster;
        private bool m_Finished;
        internal DeadBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
        }
        public void Enter()
        {
            //Debug.Log("Enter in DeadBehavior");
            GameSteamManager.UnlockAchievement("ACH_BEAT_SUBMARINE");
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
