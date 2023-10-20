using UnityEngine;
namespace RiverAttack
{
    public class GameStateHub: GameState
    {
        const float TIME_TO_FADE_BGM = 0.1f;
        public override void OnLoadState()
        {
            GameAudioManager.instance.ChangeBGM(LevelTypes.Hub, TIME_TO_FADE_BGM);
        }
        public override void EnterState()
        {
            Debug.Log($"Entra no Estado: HUB");
        }
        public override void UpdateState()
        {
            Debug.Log($"Game HUB!");
        }
        public override void ExitState()
        {
            Debug.Log($"Sai do Estado: HUB");
        }
    }
}
