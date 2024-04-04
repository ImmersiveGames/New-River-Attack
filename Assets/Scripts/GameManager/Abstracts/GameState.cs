using System.Collections;
using System.Threading.Tasks;
namespace RiverAttack
{
    public abstract class GameState
    {
        public abstract IEnumerator OnLoadState();
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
    }
    public enum BgmTypes
    {
        Menu = 1 << 0, 
        HUD = 1 << 1, 
        GameOver = 1 << 2, 
        Complete = 1 << 3, 
        Tutorial = 1 << 4,
        Grass = LevelTypes.Grass, 
        Forest= LevelTypes.Forest, 
        Swamp= LevelTypes.Swamp, 
        Antique= LevelTypes.Antique, 
        Desert= LevelTypes.Desert, 
        Ice= LevelTypes.Ice, Boss = LevelTypes.Boss
    }
}
