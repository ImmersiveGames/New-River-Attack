using UnityEngine;

namespace RiverAttack
{
    
public class LevelObstacleSpawnMaster : MonoBehaviour
{
    [SerializeField]
    private bool persistPrefab = true;
    [SerializeField]
    private GameObject[] prefab;
    [SerializeField]
    private bool ignoreWall;
    //[SerializeField]
    //private bool ignoreEnemys;

    [Header("Show prefab ID -Only View")]
    public int viewIdPrefab;
    public Color wireColor = new Color(255, 0, 0, 050);

    public GameObject GetPrefab { get { return prefab[viewIdPrefab]; } }

    private void Awake()
    {
        this.gameObject.SetActive(true);
        SpawnObstacles();
        this.gameObject.SetActive(false);
    }

    private void SpawnObstacles()
    {
        if (prefab != null && prefab.Length > 0)
        {
            SetGameObjectsActive(prefab, false);
            int sort = Random.Range(0, prefab.Length - 1);
            
            GameObject enemy = Instantiate(prefab[sort], this.transform.position, this.transform.rotation, this.transform.parent);
            if (!persistPrefab)
            {
                enemy.GetComponent<EnemiesMaster>().ignoreWall = ignoreWall;
                //enemy.GetComponent<EnemyMaster>().ignoreEnemys = ignoreEnemys;
                ObstacleMoviment om = enemy.GetComponent<ObstacleMoviment>();
                ObstacleMoveByApproach omba = enemy.GetComponent<ObstacleMoveByApproach>();
                ObstacleSkins oskin = enemy.GetComponent<ObstacleSkins>();
                ObstacleShoot oShoot = enemy.GetComponent<ObstacleShoot>();
                ObstacleDetectApproach oShootApp = enemy.GetComponent<ObstacleDetectApproach>();
                if (GetComponent<LevelObstacleSpawnMovement>() && om)
                {
                    LevelObstacleSpawnMovement spawnMovement = GetComponent<LevelObstacleSpawnMovement>();
                    om.canMove = spawnMovement.canMove;
                    om.MovementSpeed = spawnMovement.MovementSpeed;
                    om.MoveDirection = spawnMovement.MoveDirection;
                    om.FreeDirection = spawnMovement.FreeDirection;
                    om.CurveMoviment = spawnMovement.CurveMoviment;
                }
                if (GetComponent<LevelObstacleSpawnMovApproch>() && omba)
                {
                    LevelObstacleSpawnMovApproch spawnMovimentApp = GetComponent<LevelObstacleSpawnMovApproch>();
                    omba.RadiusPlayerProximity = spawnMovimentApp.RadiusPlayerProximity;
                    omba.TimeToCheck = spawnMovimentApp.TimeToCheck;
                    omba.dificultType = spawnMovimentApp.dificultType;
                    omba.RandomPlayerDistanceNear = spawnMovimentApp.RandomPlayerDistanceNear;
                    //omba.enDifList = spawnMovimentApp.enDifList;
                }
                if (GetComponent<LevelObstacleSpawnSkins>() && oskin)
                {
                    LevelObstacleSpawnSkins spawnSkin = GetComponent<LevelObstacleSpawnSkins>();
                    oskin.IndexSkin = spawnSkin.IndexSkin;
                    oskin.RandomSkin = spawnSkin.RandomSkin;
                    oskin.EnemySkins = (spawnSkin.EnemySkins.Length > 0) ? spawnSkin.EnemySkins : oskin.EnemySkins;
                }
                if (GetComponent<LevelObstacleSpawnShoot>() && oShoot)
                {
                    LevelObstacleSpawnShoot spawnShoot = GetComponent<LevelObstacleSpawnShoot>();
                    oShoot.bulletSpeedy = spawnShoot.bulletSpeedy;
                    oShoot.cadencyShoot = spawnShoot.cadencyShoot;
                    EnemyShoot enemyShoot = (EnemyShoot)oShoot;
                    enemyShoot.startpool = spawnShoot.startpool;
                    enemyShoot.playerTarget = spawnShoot.playerTarget;
                }
                if (GetComponent<LevelObstacleSpawnShootApproach>() && oShootApp)
                {
                    LevelObstacleSpawnShootApproach spawnShootApp = GetComponent<LevelObstacleSpawnShootApproach>();
                    oShootApp.radiusPlayerProximity = spawnShootApp.radiusPlayerProximity;
                    oShootApp.randomPlayerDistanceNear = spawnShootApp.randomPlayerDistanceNear;
                    oShootApp.dificultType = spawnShootApp.dificultType;
                    //oShootApp.enDifList = spawnShootApp.enDifList;
                    EnemyShootApproach enemyShootApp = (EnemyShootApproach)oShootApp;
                    enemyShootApp.timeToCheck = spawnShootApp.timeToCheck;
                }
            }
            enemy.SetActive(true);
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }

    private void SetGameObjectsActive(GameObject[] objects, bool active)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(active);
        }
    }

    [ContextMenu("LoadPrefab")]
    private void LoadPrefab()
    {
        if (prefab != null && prefab.Length > 0)
        {
            ignoreWall = prefab[viewIdPrefab].GetComponent<EnemiesMaster>().ignoreWall;
            //ignoreEnemys = prefab[viewIdPrefab].GetComponent<EnemyMaster>().ignoreEnemys;
        }
    }

    #region DrawGizmos
    void OnDrawGizmos()
    {
        if (prefab != null && prefab.Length > 0)
        {
            Gizmos.color = wireColor;
            Mesh mesh = prefab[viewIdPrefab].GetComponentInChildren<MeshFilter>().sharedMesh;
            Gizmos.DrawWireMesh(mesh, transform.position, transform.rotation, transform.localScale);
        }
    }
    #endregion
}
}