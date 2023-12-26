using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class StateDetonate : IShoot
    {
        MineMaster m_MineMaster;
        MinesDetonator m_MinesDetonator;

        bool m_Alert;
        public StateDetonate(MinesDetonator minesDetonator)
        {
            m_MinesDetonator = minesDetonator;
        }

        public void EnterState(ObstacleMaster enemiesMaster)
        {
            m_MineMaster = enemiesMaster as MineMaster;
        }
        public void UpdateState()
        {
            if (m_Alert)
            {
                Debug.Log("DETONATE!!!");
            }
            m_Alert = true;

        }
        public void ExitState()
        {
            Debug.Log("EXIT DETONATE");
        }
    }
}
