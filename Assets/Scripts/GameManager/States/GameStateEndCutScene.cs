using System.Collections;
using UnityEngine.Playables;
namespace RiverAttack
{
    public class GameStateEndCutScene : GameState
    {
        /*const float TIME_TO_FADE_BGM = 0.1f;
        const float TOLERANCE = 1f;*/
        readonly PlayableDirector m_PlayableDirector;
        readonly GameManager m_GameManager;
        public override IEnumerator OnLoadState()
        {
            //throw new System.NotImplementedException();
            yield return null;
        }
        public override void EnterState()
        {
            //Debug.Log($"Enter State: CutScene END");
            PlayerManager.instance.DestroyPlayers();
            GameMissionBuilder.instance.ResetBuildMission();
            GameTimelineManager.instance.endCutDirector.gameObject.SetActive(false);
            
        }

        public override void UpdateState()
        {
            //Debug.Log($"Run State: CutScene END");
        }
        public override void ExitState()
        {
            //Debug.Log($"Saindo no Estado: CutScene END");
        }
    }
}
