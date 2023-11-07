using UnityEngine;
namespace RiverAttack
{
    public class EnemiesBridges : MonoBehaviour
    {
        EnemiesMaster m_EnemiesMaster;
        void OnEnable()
        {
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
        }

        public void IsFinish()
        {
            m_EnemiesMaster.isFinishLevel = true;
            m_EnemiesMaster.enemy.isCheckInPoint = false;
        }
    }
}
