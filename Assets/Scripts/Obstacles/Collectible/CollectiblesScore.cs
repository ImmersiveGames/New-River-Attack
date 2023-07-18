using UnityEngine;
namespace RiverAttack
{
    
    public class CollectiblesScore : EnemiesScore
    {

        [SerializeField]
        protected int scoreCollect;
        protected CollectiblesMaster collectiblesMaster;

        protected override void OnEnable()
        {
            base.OnEnable();
            collectiblesMaster.CollectibleEvent += SetCollScore;
        }
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            collectiblesMaster = GetComponent<CollectiblesMaster>();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            collectiblesMaster.CollectibleEvent -= SetCollScore;
        }

        private void SetCollScore(PlayerMaster playerMaster)
        {
            playerMaster.PlayersSettings().score += scoreCollect;
        }
    }
}

