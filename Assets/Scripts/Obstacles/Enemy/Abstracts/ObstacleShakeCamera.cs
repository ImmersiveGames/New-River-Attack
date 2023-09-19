using System;
using RiverAttack;
using UnityEngine;

public class ObstacleShakeCamera : MonoBehaviour
{
    [SerializeField]
    float shakeIntensity;
    [SerializeField]
    float shakeTime;
    ObstacleMaster m_ObstacleMaster;
    #region UNITYMETHODS
    void Awake()
    {
        m_ObstacleMaster = GetComponent<ObstacleMaster>();
    }
    void OnEnable()
    {
        m_ObstacleMaster.EventObstacleMasterHit += ShakeCamOnExplode;
    }

    void OnDisable()
    {
        m_ObstacleMaster.EventObstacleMasterHit -= ShakeCamOnExplode;
    }
    #endregion

    void ShakeCamOnExplode()
    {
        CameraShake.instance.ShakeCamera(shakeIntensity, shakeTime);
    }
}
