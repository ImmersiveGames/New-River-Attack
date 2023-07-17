using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    public class LevelFinish : MonoBehaviour
    {
        GameSettings gameSettings;
        GameManager gameManager;
        GamePlayManager gamePlay;
        private void OnEnable()
        {
            gamePlay = GamePlayManager.instance;
            gameManager = GameManager.instance;
            gameSettings = GameSettings.instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(gameSettings.playerTag))
            {
                if (!gameManager.levelsFinish.Contains(gameManager.actualLevel))
                    gameManager.levelsFinish.Add(gameManager.actualLevel);
                gamePlay.CallEventCompletePath();
            }
        }
    }
}

