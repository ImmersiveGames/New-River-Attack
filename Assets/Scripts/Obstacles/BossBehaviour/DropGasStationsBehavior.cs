using Utils;
using UnityEngine;
namespace RiverAttack
{
    public class DropGasStationsBehavior : IBossBehavior
    {
        private const float OFFSET_X = 2;
        private const float AUTO_DESTROY_TIME = 10f;

        private Transform m_SpawnPoint;
        private readonly int m_NumGas;

        private readonly BossGasStationDrop m_BossGasStationDrop;

        private readonly IHasPool m_MyPool;
        //IBossBehavior
        private bool m_Finished;
        internal DropGasStationsBehavior(BossMasterOld bossMasterOld, int quantity)
        {
            m_MyPool = m_BossGasStationDrop = bossMasterOld.GetBossGasStationDrop();
            m_NumGas = (quantity <= 0)? 2 : quantity;
        }

        public void Enter()
        {
            var screenLimitMin = GamePlayManager.ScreenLimitMin;
            var screenLimitMax = GamePlayManager.ScreenLimitMax;
            m_SpawnPoint = m_BossGasStationDrop.spawnPoint;
            m_BossGasStationDrop.PlayDropGas();
            for (int i = 0; i < m_NumGas; i++)
            {
                var myShoot = PoolObjectManager.GetObject(m_MyPool);
                var transformPosition = m_SpawnPoint.position;
                var transformRotation = m_SpawnPoint.rotation;
                
                float constrainedPositionX = Mathf.Clamp(transformPosition.x, screenLimitMin.x + OFFSET_X, screenLimitMax.x - OFFSET_X);
                float constrainedPositionZ = Mathf.Clamp(transformPosition.z, screenLimitMin.y + OFFSET_X, screenLimitMax.y- OFFSET_X);
                constrainedPositionX = (i == 0) ? constrainedPositionX - OFFSET_X : constrainedPositionX + OFFSET_X;
                var effectMovement = myShoot.GetComponent<EffectAreaMovement>();
                effectMovement.myPool = m_BossGasStationDrop.GetMyPool();
                myShoot.transform.position = new Vector3(constrainedPositionX, 0, constrainedPositionZ);
                myShoot.transform.rotation = new Quaternion(transformRotation.x, 0, transformRotation.z, transformRotation.w);
                effectMovement.AutoDestroyMe(AUTO_DESTROY_TIME);
            }

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
