using UnityEngine;
namespace RiverAttack
{
    public class EnemiesBridges : MonoBehaviour
    {
        EnemiesMaster m_EnemiesMaster;

        public void IsFinish()
        {
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
            //Debug.Log($"Enemie Master: {m_EnemiesMaster}");
            if(m_EnemiesMaster)
                m_EnemiesMaster.isFinishLevel = true;
        }
    }
}
