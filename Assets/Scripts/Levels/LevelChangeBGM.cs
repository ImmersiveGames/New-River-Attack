using UnityEngine;

namespace RiverAttack
{
    public class LevelChangeBGM : MonoBehaviour
    {
        public GamePlayAudio.LevelType idBgMtoChange;
        public float speedy;
        GamePlayAudio m_PlayAudio;
        GamePlayManager m_PlayMaster;
        private void OnEnable()
        {
            m_PlayMaster = GamePlayManager.instance;
            m_PlayAudio = GamePlayAudio.instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponentInParent<PlayerMaster>()) return;
            Debug.Log("Colidiu");
            m_PlayAudio.ChangeBGM(idBgMtoChange, speedy);
            m_PlayMaster.actualBGM = idBgMtoChange;
            GetComponent<Collider>().enabled = false;
        }
    }
}
