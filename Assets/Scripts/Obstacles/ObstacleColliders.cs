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
            if (!collision.GetComponentInParent<BulletPlayer>()) return null;
            if (collision.GetComponentInParent<PlayerMaster>())
            {
                collision.GetComponentInParent<PlayerMaster>().AddEnemiesHitList(enemiesMaster.enemy);
                return collision.GetComponentInParent<PlayerMaster>();
            }
            collision.GetComponentInParent<BulletPlayer>().ownerShoot.AddEnemiesHitList(enemiesMaster.enemy);
            return collision.GetComponentInParent<BulletPlayer>().ownerShoot;
            
                /*if (collision.transform.root.GetComponent<PlayerBombSet>())
                {
                    collision.transform.root.GetComponent<PlayerBombSet>().OwnerShoot.AddHitList(enemiesMaster.enemy);
                    return collision.transform.root.GetComponent<PlayerBombSet>().OwnerShoot;
                }*/

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
