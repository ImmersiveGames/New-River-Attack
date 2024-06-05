using UnityEngine;
namespace RiverAttack
{
    public class EnemiesBridges : MonoBehaviour
    {
        private EnemiesMasterOld _mEnemiesMasterOld;
        

        public void IsFinish()
        {
            _mEnemiesMasterOld = GetComponent<EnemiesMasterOld>();
            //Debug.Log($"Enemie Master: {m_EnemiesMaster}");
            if(_mEnemiesMasterOld)
                _mEnemiesMasterOld.isFinishLevel = true;
        }

        public void SetBridgeType()
        {
            
        }
    }
}
