using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class UiHubBridges : MonoBehaviour
    {
        [SerializeField] internal Levels level;
        [SerializeField] GameObject explosion;
        [SerializeField]  AudioEventSample enemyExplodeAudio;

        IEnumerator Start()
        {
            if (!GameHubManager.instance.gamePlayingLog.finishLevels.Contains(level))
                yield break;
            if (level.levelsStates != LevelsStates.Complete)
            {
                gameObject.SetActive(false);
                yield break;
            }
            while (GameHubManager.instance.readyHub == false)
            {
                yield return null;
            }
            GameHubManager.instance.readyHub = false;
            yield return new WaitForSeconds(1f);
            Tools.ToggleChildren(transform, false);
            GameAudioManager.instance.PlaySfx(enemyExplodeAudio);
            var explosionGameObject = Instantiate(explosion, transform);
            Destroy(explosionGameObject, 1.8f);
            level.levelsStates = LevelsStates.Open;
            yield return new WaitForSeconds(1f);
            GameHubManager.instance.CheckNextLevel();
            yield return null;
        }
    }
}
