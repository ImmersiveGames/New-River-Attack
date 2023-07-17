using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace RiverAttack
{
    
public abstract class ObstacleDetectApproach : MonoBehaviour
{
    [SerializeField]
    public float radiusPlayerProximity;
    [SerializeField]
    [Tools.MinMaxRangeAttribute(0f, 10f)]
    public Tools.FloatRanged randomPlayerDistanceNear;

    #region GizmoSettings
    [Header("Gizmo Settings")]
    /*[SerializeField]
    public DifficultyList enDifList;*/
    [HideInInspector]
    public string dificultType;
    [SerializeField]
    private Color gizmoColor = new Color(255, 0, 0, 150);
    #endregion

    //private EnemyDifficulty enemyDifficulties;
    protected float playerDistance;

    protected float RandomRangeDetect
    {
        get { return Random.Range(randomPlayerDistanceNear.minValue, randomPlayerDistanceNear.maxValue); }
    }
    
    protected virtual void OnEnable()
    {
        SetInitialReferences();
    }
    protected virtual void SetInitialReferences()
    {
        /*if (GetComponent<EnemyDifficulty>() != null)
            enemyDifficulties = GetComponent<EnemyDifficulty>();*/
    }
    protected Collider[] ApproachPlayer(LayerMask layerMask)
    {
        //implementar no concreto
        playerDistance = GetPlayerDistance();
        if (randomPlayerDistanceNear.maxValue > 0)
            playerDistance = RandomRangeDetect;
        return Physics.OverlapSphere(transform.position, playerDistance, layerMask);
    }

    protected float GetPlayerDistance()
    {
        return 1f;
        //return (enemyDifficulties.MyDifficulty.multplyPlayerDistance > 0) ? radiusPlayerProximity * enemyDifficulties.MyDifficulty.multplyPlayerDistance : radiusPlayerProximity;
    }
    void OnDrawGizmosSelected()
    {
        if (radiusPlayerProximity > 0 || randomPlayerDistanceNear.maxValue > 0)
        {
            float circleDistance = radiusPlayerProximity;
            if (randomPlayerDistanceNear.maxValue > 0)
                circleDistance = randomPlayerDistanceNear.maxValue;
            /*if (GetComponent<EnemyDifficulty>() != null && radiusPlayerProximity != circleDistance)
            {
                EnemySetDifficulty MyDifficulty = GetComponent<EnemyDifficulty>().GetDifficult(dificultType);
                circleDistance *= MyDifficulty.multplyPlayerDistance;
            }*/
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.localPosition, circleDistance);
        }
    }
}
}

