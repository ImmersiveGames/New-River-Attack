using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RiverAttack
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "RiverAttack/Levels", order = 102)]
    [System.Serializable]
    public class Levels : ScriptableObject
    {
        public string levelName;
#if UNITY_EDITOR
        [Multiline]
        public string developerDescription = "";
#endif
        public bool beatGame;

        [Header("HUD Settings")]
        public Sprite levelIcon;
        public Vector3 levelIconPos;
        
        [Header("Build Settings")]
        public LevelTypes bgmStartLevel;
        [SerializeField] internal GameObject pathStart;
        [SerializeField] internal GameObject pathEnd;
        [SerializeField] internal Vector3 levelOffset;
        [SerializeField] internal List<LevelsSetup> setLevelList;

        /*        
        public bool CheckIfComplete(List<Levels> finishList)
        {
            return finishList.Contains(this);
        }

        public bool CheckIfLastFinish(List<Levels> finishList)
        {
            return finishList[^1] == this;
        }

        public bool CheckIfUnlocked(Levels previous)
        {
            return previousLevel.Contains(previous);
        }

        public bool CheckIfLocked(List<Levels> finishList)
        {
            return previousLevel.Count > 0 && previousLevel.All(t => finishList.Count < 1 || !finishList.Contains(t));
        }*/
    }

    [System.Serializable]
    public struct LevelsSetup
    {
        public GameObject levelPaths;
        public GameObject enemiesSets;
        public LevelTypes bgmLevel;
    }
}
