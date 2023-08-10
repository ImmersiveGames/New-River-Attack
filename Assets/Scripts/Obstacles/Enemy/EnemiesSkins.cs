using System;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class EnemiesSkins : ObstacleSkins
    {
        ObstacleMaster m_ObstacleMaster;
        GamePlayManager m_GamePlayManager;
        #region UNITY METHODS
        void Awake()
                {
                    SetInitialReferences();
                    //Tools.SetLayersRecursively(GameManager.instance.layerEnemies, transform);
                    m_ObstacleMaster.CallEventChangeSkin();
                }
        void OnEnable()
        {
            LoadDefaultSkin();
            m_GamePlayManager.EventResetEnemies += ResetSkinPosition;
        }
        void OnDisable()
        {
            m_GamePlayManager.EventResetEnemies -= ResetSkinPosition;
        }
  #endregion
        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_ObstacleMaster = GetComponent<ObstacleMaster>();
        }
        void ResetSkinPosition()
        {
            var go = transform.GetChild(0);
            var transform1 = go.transform;
            transform1.localPosition = Vector3.zero;
            transform1.localRotation = Quaternion.identity;
        }
    }
}
