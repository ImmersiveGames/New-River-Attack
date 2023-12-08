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
        void Update()
        {
            if (m_Target == null)
            {
                m_Target = m_BossMaster.targetPlayer;
                return;
            }
            // Obtém a posição atual do objeto à frente do alvo
            var positionAhead = transform.position;

            // Calcula a posição desejada à frente do alvo apenas no eixo Z
            var targetPosition = m_Target.position;
            var newPosition = new Vector3(positionAhead.x, positionAhead.y, targetPosition.z + distanceAhead);

            // Suaviza o movimento do objeto para corrigir a posição à frente do alvo apenas no eixo Z
            var lerpPosition = Vector3.Lerp(positionAhead, newPosition, Time.deltaTime * speed);

            // Define a posição do objeto à frente do alvo apenas no eixo Z
            transform.position = lerpPosition;
        }
    }
}
