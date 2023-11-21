using System;
using System.Collections;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class UiHubBridges : MonoBehaviour
    {
        [SerializeField] internal Levels level;
        [SerializeField] internal int myIndex;
        [SerializeField] GameObject explosion;
        [SerializeField]  AudioEventSample enemyExplodeAudio;

        //Spawn um tiro na posição do Player, move até este objeto

        void OnEnable()
        {
            GameHubManager.instance.CompleteLevel += CheckLevelComplete;
        }
        void Start()
        {
            if (GamePlayingLog.instance.lastMissionFinishIndex < myIndex)
                return;
            if (level.levelsStates == LevelsStates.Complete)
            {
                GameHubManager.instance.readyHub = false;
                return;
            }
            if (myIndex >= GamePlayingLog.instance.lastMissionFinishIndex)
                return;
            gameObject.SetActive(false);
            level.levelsStates = LevelsStates.Open;

        }
        void OnDisable()
        {
            GameHubManager.instance.CompleteLevel -= CheckLevelComplete;
        }
        void CheckLevelComplete()
        {
            if (level.levelsStates != LevelsStates.Complete) return;
            Invoke(nameof(ExplodeBridge), 1.5f);
        }

        void ExplodeBridge()
        {
            Debug.Log("Explode a ponte ai carinha");
            //gameObject.SetActive(false);
            Tools.ToggleChildren(transform, false);
            GameAudioManager.instance.PlaySfx(enemyExplodeAudio);
            var explosionGameObject = Instantiate(explosion, transform);
            Destroy(explosionGameObject, 1.8f);
            level.levelsStates = LevelsStates.Open;
            GameHubManager.instance.MissionNextLevel();
            GameHubManager.instance.readyHub = true;
        }
        
    }
}
