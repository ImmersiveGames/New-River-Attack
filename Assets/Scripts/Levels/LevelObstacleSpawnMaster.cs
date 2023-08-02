using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RiverAttack
{
    public class LevelObstacleSpawnMaster : MonoBehaviour
    {
        [SerializeField]
        bool persistPrefab = true;
        [SerializeField]
        GameObject[] prefab;
        [SerializeField]
        bool ignoreWall;
        [SerializeField]
        bool ignoreEnemies;

        [Header("Show prefab ID -Only View")]
        public int viewIdPrefab;
        public Color wireColor = new Color(255, 0, 0, 050);

        public GameObject getPrefab { get { return prefab[viewIdPrefab]; } }
#region UNITY METHODS
        void Awake()
        {
            gameObject.SetActive(true);
            SpawnObstacles();
            gameObject.SetActive(false);
        }
  #endregion
        
        void SpawnObstacles()
        {
            if (prefab is not { Length: > 0 })  return;
            SetGameObjectsActive(prefab, false);
            int sort = Random.Range(0, prefab.Length - 1);

            var transform1 = this.transform;
            var enemy = Instantiate(prefab[sort], transform1.position, transform1.rotation, transform1.parent);
            if (persistPrefab)
            {
                /*enemy.GetComponent<EnemiesMaster>().ignoreWall = ignoreWall;
                enemy.GetComponent<EnemiesMaster>().ignoreEnemies = ignoreEnemies;*/
                var obstacleMovement = enemy.GetComponent<ObstacleMovement>();
                //var obstacleMoveByApproach = enemy.GetComponent<ObstacleMoveByApproach>();
                var obstacleSkins = enemy.GetComponent<ObstacleSkins>();
                var obstacleShoot = enemy.GetComponent<EnemiesShoot>();
                var obstacleDetectApproach = enemy.GetComponent<ObstacleDetectApproach>();
                if (GetComponent<LevelObstacleSpawnMovement>() && obstacleMovement)
                {
                    /*var spawnMovement = GetComponent<LevelObstacleSpawnMovement>();
                    obstacleMovement.canMove = spawnMovement.canMove;
                    obstacleMovement.moveVelocity = spawnMovement.moveVelocity;
                    obstacleMovement.moveFreeDirection = spawnMovement.moveFreeDirection;
                    obstacleMovement.animationCurve = spawnMovement.animationCurve;*/
                }
                /*if (GetComponent<LevelObstacleSpawnMoveApproach>() && obstacleMoveByApproach)
                {
                    var spawnMoveApproach = GetComponent<LevelObstacleSpawnMoveApproach>();
                    obstacleMoveByApproach.radiusPlayerProximity = spawnMoveApproach.radiusPlayerProximity;
                    obstacleMoveByApproach.timeToCheck = spawnMoveApproach.timeToCheck;
                    obstacleMoveByApproach.difficultType = spawnMoveApproach.difficultType;
                    obstacleMoveByApproach.randomPlayerDistanceNear = spawnMoveApproach.randomPlayerDistanceNear;
                    obstacleMoveByApproach.enemiesEnemiesSetDifficultyListSo = spawnMoveApproach.enemiesEnemiesSetDifficultyListSo;
                }*/
                if (GetComponent<LevelObstacleSpawnSkins>() && obstacleSkins)
                {
                    var spawnSkin = GetComponent<LevelObstacleSpawnSkins>();
                    obstacleSkins.indexSkin = spawnSkin.indexSkin;
                    obstacleSkins.randomSkin = spawnSkin.randomSkin;
                    obstacleSkins.enemiesSkins = (spawnSkin.enemiesSkins.Length > 0) ? spawnSkin.enemiesSkins : obstacleSkins.enemiesSkins;
                }
                if (GetComponent<LevelObstacleSpawnShoot>() && obstacleShoot)
                {
                    /*var spawnShoot = GetComponent<LevelObstacleSpawnShoot>();
                    obstacleShoot.bulletSpeed = spawnShoot.bulletSpeedy;
                    obstacleShoot.shootCadence = spawnShoot.cadenceShoot;
                    var enemiesShoot = obstacleShoot;
                    enemiesShoot.startPool = spawnShoot.startPool;
                    enemiesShoot.hasTarget = spawnShoot.playerTarget;*/
                }
                /*if (GetComponent<LevelObstacleSpawnShootApproach>() && obstacleDetectApproach)
                {
                    var spawnShootApp = GetComponent<LevelObstacleSpawnShootApproach>();
                    obstacleDetectApproach.radiusPlayerProximity = spawnShootApp.radiusPlayerProximity;
                    obstacleDetectApproach.randomPlayerDistanceNear = spawnShootApp.randomPlayerDistanceNear;
                    obstacleDetectApproach.difficultType = spawnShootApp.difficultType;
                    obstacleDetectApproach.enemiesesEnemiesSetDifficultyListSo = spawnShootApp.enemiesesEnemiesSetDifficultyListSo;
                    var enemiesShootApp = (EnemiesShootApproach)obstacleDetectApproach;
                    enemiesShootApp.timeToCheck = spawnShootApp.timeToCheck;
                }*/
            }
            enemy.SetActive(true);
            gameObject.SetActive(false);
            if (gameObject != null)
                Destroy(gameObject);
        }

        static void SetGameObjectsActive([NotNull] IEnumerable<GameObject> objects, bool active)
        {
            foreach (var t in objects)
            {
                t.SetActive(active);
            }
        }

        [ContextMenu("LoadPrefab")]
        void LoadPrefab()
        {
            if (prefab is not { Length: > 0 }) return;
            /*ignoreWall = prefab[viewIdPrefab].GetComponent<EnemiesMaster>().ignoreWall;
            ignoreEnemies = prefab[viewIdPrefab].GetComponent<EnemiesMaster>().ignoreEnemies;*/
        }

    #region DrawGizmos
        void OnDrawGizmos()
        {
            if (prefab is not { Length: > 0 }) return;
            Gizmos.color = wireColor;
            var mesh = prefab[viewIdPrefab].GetComponentInChildren<MeshFilter>().sharedMesh;
            var transform1 = transform;
            Gizmos.DrawWireMesh(mesh, transform1.position, transform1.rotation, transform1.localScale);
        }
    #endregion
    }
}
