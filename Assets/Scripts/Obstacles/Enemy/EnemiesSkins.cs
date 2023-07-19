namespace RiverAttack
{
    public class EnemiesSkins : ObstacleSkins
    {
        EnemiesMaster m_EnemiesMaster;
        #region UNITY METHODS
        void Start()
                {
                    SetInitialReferences();
                    EnemiesMaster.SetTagLayer(skins, GameSettings.instance.layerEnemies);
                    m_EnemiesMaster.CallEventChangeSkin();
                }
  #endregion
        void SetInitialReferences()
        {
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
        }
    }
}
