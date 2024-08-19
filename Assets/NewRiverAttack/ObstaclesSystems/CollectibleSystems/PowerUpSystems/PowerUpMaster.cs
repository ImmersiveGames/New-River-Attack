namespace NewRiverAttack.ObstaclesSystems.CollectibleSystems.PowerUpSystems
{
    public class PowerUpMaster : CollectibleMaster
    {
        public float timeToDestroy = 5f;

        protected override void OnEnable()
        {
            base.OnEnable();
            GamePlayManagerRef.EventGameRestart += DestroyMe;
        }

        private void Start()
        {
            if (timeToDestroy > 0)
            {
                Invoke(nameof(DestroyMe), timeToDestroy);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GamePlayManagerRef.EventGameRestart -= DestroyMe;
        }

        private void RemovePowerUp()
        {
            DestroyImmediate(gameObject); 
        }

        public void DestroyMe()
        {
            gameObject.SetActive(false);
            Invoke(nameof(RemovePowerUp), 0.5f); 
        }
    }
}