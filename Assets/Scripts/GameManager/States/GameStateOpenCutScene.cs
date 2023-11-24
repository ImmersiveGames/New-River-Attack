using System.Collections;
using UnityEngine;
namespace RiverAttack
{
    public class GameStateOpenCutScene : GameState
    {
        const float TIME_TO_FADE_BGM = 0.1f;
        public override IEnumerator OnLoadState()
        {
            var startLevel = GameManager.instance.GetLevel();
            //Debug.Log($"Start: {startLevel}");
            var startBgm = startLevel.bgmStartLevel;
            GameAudioManager.instance.ChangeBGM(startBgm, TIME_TO_FADE_BGM);
            yield return null;
        }
        public override void EnterState()
        {
            Debug.Log($"Entra no Estado: CutScene");
            GameMissionBuilder.instance.StartBuildMission(GamePlayManager.instance.actualLevels);
            PlayerManager.instance.InstantiatePlayers();
            GameTimelineManager.instance.openCutDirector.Play();
        }

        public override void UpdateState()
        {
            //if (GameManager.instance.onLoadScene) return;
            //Debug.Log($"CutScene!");
        }
        public override void ExitState()
        {
            //Debug.Log($"Sai do Estado: CutScene");
        }

    }
}
