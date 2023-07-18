using UnityEngine;
namespace RiverAttack
{
    [RequireComponent(typeof(EnemiesMaster))]
    public abstract class ObstacleColliders : MonoBehaviour
    {
        protected EnemiesMaster enemiesMaster;
        protected GamePlayManager gamePlay;
        protected GameSettings gameSettings;
        protected GameManager gameManager;

        protected virtual void OnEnable()
        {
            SetInitialReferences();
        }

        protected virtual void SetInitialReferences()
        {
            enemiesMaster = GetComponent<EnemiesMaster>();
            gameSettings = GameSettings.instance;
            gamePlay = GamePlayManager.instance;
            gameManager = GameManager.instance;
        }

        protected virtual void OnTriggerEnter(Collider collision) { }
        public virtual void HitThis(Collider collision) { }

        //public virtual void HitThis(Collider collision, PlayerMaster playerM = null) { }

        public virtual void CollectThis(Collider collision) { }
        protected PlayerMaster WhoHit(Collider collision)
        {
            if (collision.GetComponentInParent<PlayerMaster>())
            {
                collision.GetComponentInParent<PlayerMaster>().AddEnemiesHitList(enemiesMaster.enemy);
                return collision.GetComponentInParent<PlayerMaster>();
            }
            if (collision.transform.root.GetComponent<PlayerBombSet>())
            {
                var ownerBomb = collision.GetComponentInParent<PlayerBombSet>().GetOwner();
                ownerBomb.AddHitList(enemiesMaster.enemy);
                return ownerBomb;
            }
            var ownerShoot = collision.GetComponentInParent<BulletPlayer>().GetOwner();
            ownerShoot.AddEnemiesHitList(enemiesMaster.enemy);
            return ownerShoot;

            

        }

        //protected void ShouldCompleteMission()
        //{
        //    if (enemyMaster.goalLevel)
        //    {
        //        //TODO: Rervisitar aqui pra faze a animação de final de fase por enquanto so termina a fase
        //        gamePlay.levelComplete = true;
        //        if (gameManager.ActualLevel.beatGame == true)
        //            gameManager.isGameBeat = true;
        //        gamePlay.PausePlayGame();
        //        gamePlay.CallEventCompleteMission();
        //    }
        //}

        protected void ShouldSavePoint()
        {
            if (enemiesMaster.enemy.isCheckInPoint)
                gamePlay.CallEventCheckPoint(transform.position);
        }
    }
}
