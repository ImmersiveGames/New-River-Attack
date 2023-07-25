using UnityEngine;
namespace RiverAttack
{
    public class EnemiesSkins : ObstacleSkins
    {
        EnemiesMaster m_EnemiesMaster;
        #region UNITY METHODS
        void Awake()
                {
                    SetInitialReferences();
                    SetLayers(GameManager.instance.layerEnemies);
                    m_EnemiesMaster.CallEventChangeSkin();
                }
  #endregion
        void SetInitialReferences()
        {
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
        }
    }
}
