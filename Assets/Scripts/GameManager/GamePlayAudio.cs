using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class GamePlayAudio : Singleton<GamePlayAudio>
    {
        public enum LevelType { Grass = 4, Forest = 2, Antique = 0, Desert = 1, Ice = 3, Swamp = 7, Hub = 5, MainTheme = 6 }
        public LevelType levelType;
        public void ChangeBGM(LevelType idBgMtoChange, float speedy)
        {
            throw new System.NotImplementedException();
        }
    }
}

