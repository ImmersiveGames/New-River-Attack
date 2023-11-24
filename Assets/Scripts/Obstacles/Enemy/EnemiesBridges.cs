using UnityEngine;
namespace RiverAttack
{
    public class EnemiesBridges : MonoBehaviour
    {
        EnemiesMaster m_EnemiesMaster;
        void Start()
        {
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
        }

        public void IsFinish()
        {
            if(m_EnemiesMaster)
                m_EnemiesMaster.isFinishLevel = true;
        }
    }
}
