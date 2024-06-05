using System;
using RiverAttack;
using UnityEngine;

public class ObstacleShakeCamera : MonoBehaviour
{
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float shakeTime;
    private ObstacleMasterOld _mObstacleMasterOld;
    #region UNITYMETHODS

    private void Awake()
    {
        _mObstacleMasterOld = GetComponent<ObstacleMasterOld>();
    }

    private void OnEnable()
    {
        _mObstacleMasterOld.EventObstacleMasterHit += ShakeCamOnExplode;
    }

    private void OnDisable()
    {
        _mObstacleMasterOld.EventObstacleMasterHit -= ShakeCamOnExplode;
    }
    #endregion

    private void ShakeCamOnExplode()
    {
        CameraShake.ShakeCamera(shakeIntensity, shakeTime);
    }
}
