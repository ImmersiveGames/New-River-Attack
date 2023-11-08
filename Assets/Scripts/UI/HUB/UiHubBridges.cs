using System;
using UnityEngine;
namespace RiverAttack
{
    public class UiHubBridges : MonoBehaviour
    {
        [SerializeField] internal Levels level;
        [SerializeField] internal int myIndex;
        [SerializeField] GameObject explosion;
        
        //Spawn um tiro na posição do Player, move até este objeto
        void Start()
        {
            if (GamePlayingLog.instance.lastMissionFinishIndex < myIndex)
                return;
            if (level.levelsStates == LevelsStates.Complete)
            {
                GameHubManager.instance.readyHub = false;
                ExplodeBridge();
            }
                
            gameObject.SetActive(false);
            level.levelsStates = LevelsStates.Open;
        }

        void ExplodeBridge()
        {
            //Spawn Tiro na posição do player
            //move até este objeto
            //Desativa este objeto
            //Ativa a explosão
            level.levelsStates = LevelsStates.Open;
            GameHubManager.instance.readyHub = true;

        }
    }
}
