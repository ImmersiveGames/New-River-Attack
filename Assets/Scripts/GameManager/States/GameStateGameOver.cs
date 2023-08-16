using UnityEngine;
namespace RiverAttack
{
    public class GameStateGameOver : GameState
    {
        const float TIME_TO_FADE_BGM = 0.1f;
        public override void EnterState()
        {
            Debug.Log($"Entra no Estado: GameOver");
            GamePlayAudio.instance.ChangeBGM(LevelTypes.GameOver, TIME_TO_FADE_BGM);
        }
        public override void UpdateState()
        {
            Debug.Log($"GameOver");
        }
        public override void ExitState()
        {
            Debug.Log($"Saida no Estado: GameOver");
        }
    }
}
