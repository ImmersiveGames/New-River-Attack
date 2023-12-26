using UnityEngine;
namespace RiverAttack
{
    public class MinesSounds: EnemiesSound
    {
        [SerializeField]
        AudioEventSample mineAlertAudio;
        
        MineMaster m_MineMaster;
        protected override void OnEnable()
        {
            base.OnEnable();
            m_MineMaster.EventMineAlert += MineAlertSound;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            m_MineMaster.EventMineAlert -= MineAlertSound;
        }


        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_MineMaster = obstacleMaster as MineMaster;
        }

        void MineAlertSound()
        {
            if (audioSource != null && mineAlertAudio != null)
                mineAlertAudio.Play(audioSource);
        }
    }
}
