using System.Collections;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class UiHubBridges : MonoBehaviour
    {
        [SerializeField] internal Levels level;
        [SerializeField] private GameObject explosion;
        [SerializeField] private AudioEventSample enemyExplodeAudio;

        private const float TIMER = 1f;
        private const float TIMER_EXPLOSION = 1.8f;

        private IEnumerator Start()
        {
            if (!GameHubManager.instance.gamePlayingLog.GetLevelsResult().Contains(level))
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
            yield return new WaitForSeconds(TIMER);
            Tools.ToggleChildren(transform, false);
            //Debug.Log($"Level: {level.name}");
            GameAudioManager.instance.PlaySfx(enemyExplodeAudio);
            var explosionGameObject = Instantiate(explosion, transform);
            Destroy(explosionGameObject, TIMER_EXPLOSION);
            level.levelsStates = LevelsStates.Open;
            yield return new WaitForSeconds(TIMER);
            GameHubManager.instance.CheckNextLevel();
            yield return null;
        }
    }
}
