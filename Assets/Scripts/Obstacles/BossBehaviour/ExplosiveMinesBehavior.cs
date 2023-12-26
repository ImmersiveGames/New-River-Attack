using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;
namespace RiverAttack
{
    public class ExplosiveMinesBehavior : IBossBehavior
    {

        //Shoot Variaveis
        float m_Cadence;
        float m_ShootCycles;

        readonly IHasPool m_IHasPool;
        Transform m_MyPool;
        
        Transform m_Target;
        float m_DelayInstances = 1f;

        //IBossBehavior
        bool m_Finished;
        readonly BossMaster m_BossMaster;
        readonly BossMinesShoot m_BossMinesShoot;
        
        internal ExplosiveMinesBehavior(BossMaster bossMaster)
        {
            m_BossMaster = bossMaster;
            m_IHasPool = m_BossMinesShoot = m_BossMaster.GetBossMines();
        }
        public void Enter()
        {
            Debug.Log("Entrando no comportamento ExplosiveMines");
            
            m_Finished = false;
            m_Target = m_BossMaster.targetPlayer;
            m_MyPool = PoolObjectManager.GetPool(m_IHasPool);
            
            //Animação de colocar as bombas na agua
            Fire();
        }
        public void Update()
        {
            Debug.Log("Atualizando comportamento ExplosiveMines");
            // Demarcar as areas onde serão colocadas as bombas
            //Animação de sombra das bombas.
            // tempo minimo para elas permanecerem na area
            
            
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
            MinesInQuadrants(m_BossMinesShoot.minesQuantity, m_BossMinesShoot.numLines, m_BossMinesShoot.numColumns, m_BossMinesShoot.quadrantsBlocked);
        }
        
        async void  MinesInQuadrants(int quantity, int lines, int columns, IReadOnlyCollection<Vector2Int> quadrantBlocked)
        {
            var camera = Camera.main;
            float height = camera!.orthographicSize * 2.0f;
            float weight = height * camera.aspect;

            float sizeQuadrantX = weight / columns;
            float sizeQuadrantZ = height / lines;

            var usedQuadrants = new List<Vector2Int>();

            for (int i = 0; i < quantity; i++)
            {
                int indexQuadrantsX, indexQuadrantsZ;

                do
                {
                    indexQuadrantsX = Random.Range(0, columns);
                    indexQuadrantsZ = Random.Range(0, lines);
                }
                while (BossMinesShoot.QuadrantAlreadyOccupied(usedQuadrants, indexQuadrantsX, indexQuadrantsZ) || 
                       BossMinesShoot.QuadrantsBlocked(indexQuadrantsX, indexQuadrantsZ, quadrantBlocked));

                usedQuadrants.Add(new Vector2Int(indexQuadrantsX, indexQuadrantsZ));

                float posX = ((indexQuadrantsX + 0.5f) * sizeQuadrantX) - (weight / 2.0f) + BossMinesShoot.OffsetX;
                float posZ = ((indexQuadrantsZ + 0.5f) * sizeQuadrantZ) - (height / 2.0f) + BossMinesShoot.OffsetZ;

                var randomPosition = new Vector3(posX, BossMinesShoot.OffsetY, posZ);
                m_BossMinesShoot.PlayMineShoot();
                var myShoot = await PoolObjectManager.GetObjectAsync(m_IHasPool);
                myShoot.transform.position = new Vector3(randomPosition.x, randomPosition.y, randomPosition.z);
               // myShoot.GetComponent<MineMaster>().Initialization(m_MyPool);
                await Task.Delay(Random.Range(500, 1000));
            }
            m_Finished = true;
        }
    }
}
