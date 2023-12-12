using System;
using UnityEngine;
namespace RiverAttack
{
    public class BossMovement: MonoBehaviour
    {
        Transform m_Target;
        BossMaster m_BossMaster;

        public float distanceAhead = 20.0f;  // Distância desejada à frente do alvo
        public float speed = 3.0f; // Velocidade de suavização


        void Start()
        {
            m_BossMaster = GetComponent<BossMaster>();
        }
    }
}
