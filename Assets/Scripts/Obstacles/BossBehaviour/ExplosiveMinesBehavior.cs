using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class ExplosiveMinesBehavior : IBossBehavior
    {
        public int quantidadeBombas = 10; // Número de bombas a serem espalhadas
        public float alturaFixa = 1.0f;   // Posição fixa para o eixo Y
        public float offsetZ = 10.0f;     // Offset para o eixo Z
        
        //Shoot Variaveis
        float m_Cadence;
        float m_ShootCycles;

        readonly IHasPool m_IHasPool;
        Transform m_MyPool;
        
        Transform m_Target;

        //IBossBehavior
        bool m_Finished;
        readonly BossMaster m_BossMaster;
        readonly BossMinesShoot m_BossMissileShoot;
        
        internal ExplosiveMinesBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
            m_IHasPool = m_BossMissileShoot = m_BossMaster.GetBossMines();
        }
        public void Enter()
        {
            Debug.Log("Entrando no comportamento ExplosiveMines");
            
            m_Finished = false;
            m_Target = m_BossMaster.targetPlayer;
            m_MyPool = PoolObjectManager.GetPool(m_IHasPool);
            
            //Animação de colocar as bombas na agua
        }
        public void Update()
        {
            Debug.Log("Atualizando comportamento ExplosiveMines");
            // Demarcar as areas onde serão colocadas as bombas
            //Animação de sombra das bombas.
            // tempo minimo para elas permanecerem na area
            
            Fire();
        }
        public void Exit()
        {
            Debug.Log("Saindo do comportamento ExplosiveMines");
            //Remover todas as bombas.
        }
        public void FinishBehavior()
        {
            m_Finished = true;
        }
        public bool IsFinished()
        {
            return m_Finished;
        }

        void Fire()
        {
            //Debug.Log("Shoot!");
            EspalharBombasNaTela();
        }
        void EspalharBombasNaTela()
        {
            //tranformar as posições em grids de 2x2
            Camera camera = Camera.main;

            float largura = camera.orthographicSize * 2.0f * camera.aspect;

            for (int i = 0; i < quantidadeBombas; i++)
            {
                float posX = Random.Range(-largura / 2.0f, largura / 2.0f);
                float posZ = Random.Range(camera.orthographicSize, offsetZ);

                Vector3 posicaoAleatoria = new Vector3(posX, alturaFixa, posZ);

                // Instanciar a bomba na posição aleatória
                var myShoot = PoolObjectManager.GetObject(m_IHasPool);
                myShoot.transform.position = new Vector3(posicaoAleatoria.x, posicaoAleatoria.y, posicaoAleatoria.z);
            }
            m_Finished = true;
        }
    }
}
