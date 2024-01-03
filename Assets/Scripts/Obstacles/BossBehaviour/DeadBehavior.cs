using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class DeadBehavior: IBossBehavior
    {
        readonly BossMaster m_BossMaster;
        bool m_Finished;
        internal DeadBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
        }
        public void Enter()
        {
            Debug.Log("Enter in DeadBehavior");
        }
        public void Update()
        {
            Debug.Log("DeadBehavior!!");
        }
        public void Exit()
        {
            Debug.Log("Leave the DeadBehavior");
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
