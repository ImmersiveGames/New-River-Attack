using System;
using RiverAttack;
using UnityEngine;

public class ObstacleShakeCamera : MonoBehaviour
{
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float shakeTime;
    private ObstacleMaster m_ObstacleMaster;
    #region UNITYMETHODS

    private void Awake()
    {
        m_ObstacleMaster = GetComponent<ObstacleMaster>();
    }

    private void OnEnable()
    {
        m_ObstacleMaster.EventObstacleMasterHit += ShakeCamOnExplode;
    }

    private void OnDisable()
    {
        m_ObstacleMaster.EventObstacleMasterHit -= ShakeCamOnExplode;
    }
    #endregion

    private void ShakeCamOnExplode()
    {
        CameraShake.ShakeCamera(shakeIntensity, shakeTime);
    }
}
