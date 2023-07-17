using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    public class LevelChangeBGM : MonoBehaviour
    {
        public GamePlayAudio.LevelType idBGMtoChange;
        public float speedy;
        GamePlayAudio playAudio;
        GamePlayManager playMaster;
        private void OnEnable()
        {
            playMaster = GamePlayManager.instance;
            playAudio = GamePlayAudio.instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((other.transform.root.CompareTag(GameSettings.instance.playerTag)))
            {
                Debug.Log("Colidiu");
                playAudio.ChangeBGM(idBGMtoChange, speedy);
                playMaster.actualBGM = idBGMtoChange;
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
