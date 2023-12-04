using System.Collections;
namespace RiverAttack
{
    public abstract class GameState
    {
        public abstract IEnumerator OnLoadState();
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
    }
    public enum LevelTypes { Menu = 0, Hub = 1, Grass = 2, Forest = 3, Swamp = 4, Antique = 5, Desert = 6, Ice = 7, GameOver = 8, Complete = 9, HUD = 10, Boss = 11 }
}
