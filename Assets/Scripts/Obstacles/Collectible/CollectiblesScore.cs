namespace RiverAttack
{
    public class CollectiblesScore : EnemiesScore
    {
        private CollectiblesMaster m_CollectiblesMaster;

        #region UNITY METHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            m_CollectiblesMaster.EventCollectItem += SetCollScore;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            m_CollectiblesMaster.EventCollectItem -= SetCollScore;
        }
  #endregion

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_CollectiblesMaster = GetComponent<CollectiblesMaster>();
        }

        private void SetCollScore(PlayerSettings playerSettings)
        {
            float score = m_CollectiblesMaster.collectibleScriptable.collectValuable;
            if (score == 0) return;
            if (EnemiesMaster.myDifficulty.multiplyScore > 0)
            {
                var myDifficulty = EnemiesMaster.myDifficulty;
                if (myDifficulty.multiplyScore > 0)
                    score *= myDifficulty.multiplyScore;
            }
            if (playerSettings == null) return;
            playerSettings.score += (int)(score);
            gamePlayManager.OnEventUpdateScore(playerSettings.score);
            LogGamePlay(playerSettings.score);
        }
    }
}
