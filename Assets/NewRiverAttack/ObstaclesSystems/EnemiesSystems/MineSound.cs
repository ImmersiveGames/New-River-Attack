
namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class MineSound : EnemiesSound
    {
        private MineMaster _mineMaster;

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _mineMaster = ObstacleMaster as MineMaster;
        }
        /*private MineMaster _mineMaster;
        [SerializeField] private AudioEvent alertAudio;
        protected override void OnEnable()
        {
            base.OnEnable();
            _mineMaster.EventAlertApproach += AlertAudio;
            _mineMaster.EventAlertStop += StopAlertAudio;
            _mineMaster.EventDetonate += DetonateAudio;
            _mineMaster.EventObstacleDeath += StopAlertAudioOnDeath;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _mineMaster.EventAlertApproach -= AlertAudio;
            _mineMaster.EventAlertStop -= StopAlertAudio;
            _mineMaster.EventDetonate -= DetonateAudio;
            _mineMaster.EventObstacleDeath -= StopAlertAudioOnDeath;
        }

        protected override void SetInitialReferences()
        {
            ObstacleMaster = _mineMaster = GetComponent<MineMaster>();
            AudioSource = GetComponent<AudioSource>();
        }

        // Som de alerta toca apenas uma vez
        private void AlertAudio()
        {
            if (AudioSource != null && alertAudio != null && !AudioSource.isPlaying) 
            {
                alertAudio.SimplePlay(AudioSource);
            }
        }

        // Método para parar o áudio de alerta quando necessário
        private void StopAlertAudio()
        {
            if (AudioSource != null && AudioSource.isPlaying) 
            {
                AudioSource.Stop();
            }
        }

        // Parar o áudio de alerta quando a mina é destruída
        private void StopAlertAudioOnDeath(PlayerMaster playerMaster)
        {
            StopAlertAudio(); 
        }

        // Som de detonação, parando o som de alerta antes
        private void DetonateAudio()
        {
            StopAlertAudio(); 
            if (AudioSource != null && audioExplosion != null)
            {
                audioExplosion.SimplePlay(AudioSource);
            }
        }*/
    }
}
