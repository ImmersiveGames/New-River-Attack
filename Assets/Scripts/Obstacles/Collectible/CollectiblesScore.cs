namespace RiverAttack
{
    public class CollectiblesScore : EnemiesScore
    {
        private CollectiblesMasterOld _mCollectiblesMasterOld;

        #region UNITY METHODS
        protected override void OnEnable()
        {
            base.OnEnable();
            _mCollectiblesMasterOld.EventCollectItem += SetCollScore;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _mCollectiblesMasterOld.EventCollectItem -= SetCollScore;
        }
  #endregion

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _mCollectiblesMasterOld = GetComponent<CollectiblesMasterOld>();
        }

        private void SetCollScore(PlayerSettings playerSettings)
        {
            float score = _mCollectiblesMasterOld.collectibleScriptable.collectValuable;
            if (score == 0) return;
            if (EnemiesMasterOld.myDifficulty.multiplyScore > 0)
            {
                var myDifficulty = EnemiesMasterOld.myDifficulty;
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
