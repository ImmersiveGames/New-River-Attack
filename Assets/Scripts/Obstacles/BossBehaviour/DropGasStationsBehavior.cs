using Utils;
using UnityEngine;
namespace RiverAttack
{
    public class DropGasStationsBehavior: IBossBehavior
    {
        readonly BossMaster m_BossMaster;
        Transform m_SpawnPoint;
        readonly BossGasStationDrop m_BossGasStationDrop;
        readonly IHasPool m_MyPool;
        //IBossBehavior
        bool m_Finished;
        internal DropGasStationsBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
            m_MyPool = m_BossGasStationDrop = m_BossMaster.GetBossGasStationDrop();
        }

        public void Enter()
        {
            m_SpawnPoint = m_BossGasStationDrop.spawnPoint;
            var myShoot = PoolObjectManager.GetObject(m_MyPool);
            var transformPosition = m_SpawnPoint.position;
            var transformRotation = m_SpawnPoint.rotation;

            myShoot.transform.position = new Vector3(transformPosition.x, transformPosition.y, transformPosition.z);
            myShoot.transform.rotation = new Quaternion(transformRotation.x, transformRotation.y, transformRotation.z, transformRotation.w);
            FinishBehavior();
        }
        public void Update()
        {
            //
        }
        public void Exit()
        {
            //
        }
        public void FinishBehavior()
        {
            m_Finished = true;
        }
        public bool IsFinished()
        {
            return m_Finished;
        }
    }
}
