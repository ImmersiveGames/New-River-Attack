using System.Collections.Generic;
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
        readonly int m_Mines;
        readonly IHasPool m_IHasPool;
        Transform m_MyPool;
        readonly Camera m_Camera = Camera.main;
        //IBossBehavior
        bool m_Finished;
        readonly BossMinesShoot m_BossMinesShoot;
        
        internal ExplosiveMinesBehavior(BossMaster bossMaster, int minesQuantity)
        {
            m_IHasPool = m_BossMinesShoot = bossMaster.GetBossMines();
            m_Mines = minesQuantity;
        }
        public void Enter()
        {
            //Debug.Log("Entrando no comportamento ExplosiveMines");
            
            m_Finished = false;
            m_MyPool = PoolObjectManager.GetPool(m_IHasPool);
            
            //Animação de colocar as bombas na agua
            MinesInQuadrants(m_Mines, m_BossMinesShoot.numLines, m_BossMinesShoot.numColumns, m_BossMinesShoot.quadrantsBlocked);
        }
        public void Update()
        {
            //Debug.Log("Atualizando comportamento ExplosiveMines");
        }
        public void Exit()
        {
            //Debug.Log("Saindo do comportamento ExplosiveMines");
        }
        public void FinishBehavior()
        {
            m_Finished = true;
        }
        public bool IsFinished()
        {
            return m_Finished;
        }
        async void  MinesInQuadrants(int quantity, int lines, int columns, IEnumerable<Vector2Int> quadrantBlocked)
        {
            var viewSize = new Vector2(m_Camera!.orthographicSize * 2.0f * m_Camera.aspect, m_Camera!.orthographicSize * 2.0f);
            var sizeQuadrant = new Vector2(viewSize.x / columns, viewSize.y / lines);
            var usedQuadrants = new List<Vector2Int>();
            usedQuadrants.AddRange(quadrantBlocked);

            for (int i = 0; i < quantity; i++)
            {
                var indexQuadrants = new Vector2Int(0,0);
                if (usedQuadrants.Count >= columns * lines)
                {
                    m_Finished = true;
                    break;
                }
                do
                {
                    indexQuadrants.x = Random.Range(0, columns);
                    indexQuadrants.y = Random.Range(0, lines);
                }
                while (BossMinesShoot.QuadrantAlreadyOccupied(indexQuadrants, sizeQuadrant, viewSize, ref usedQuadrants) ||
                       BossMinesShoot.QuadrantAlreadySort(usedQuadrants, indexQuadrants.x, indexQuadrants.y)
                       );
                usedQuadrants.Add(new Vector2Int(indexQuadrants.x, indexQuadrants.y));

                float posX = ((indexQuadrants.x + 0.5f) * sizeQuadrant.x) - (viewSize.x / 2.0f) + BossMinesShoot.OffsetX;
                float posZ = ((indexQuadrants.y + 0.5f) * sizeQuadrant.y) - (viewSize.y / 2.0f) + BossMinesShoot.OffsetZ;

                var randomPosition = new Vector3(posX, BossMinesShoot.OffsetY, posZ);
                m_BossMinesShoot.PlayMineShoot();
                var myShoot = await PoolObjectManager.GetObjectAsync(m_IHasPool);
                myShoot.transform.position = new Vector3(randomPosition.x, randomPosition.y, randomPosition.z); 
                myShoot.GetComponent<MinesBoss>().Initialization(m_MyPool);
                await Task.Delay(Random.Range(500, 1000));
            }
            m_Finished = true;
        }
    }
}
