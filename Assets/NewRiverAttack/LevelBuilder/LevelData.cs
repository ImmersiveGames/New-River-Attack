using System.Collections.Generic;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.HUBManagers;
using UnityEngine;
using UnityEngine.Localization;

namespace NewRiverAttack.LevelBuilder
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "ImmersiveGames/RiverAttack/Levels", order = 301)]
    [System.Serializable]
    public class LevelData : ScriptableObject
    {
        [Header("Level Settings")]
        public ushort levelNumber;

        public LocalizedString levelNameLocale = new LocalizedString("StringTableCollection", "Levels/Name");
#if UNITY_EDITOR
        [Multiline]
        public string developerDescription = "";
#endif
        public LevelTypes levelType;
        
        [Header("HUD Settings")] [SerializeField]
        public HubData hudPath;
        
        
        [Header("Build Settings")]
        public GameObject pathStart;
        public GameObject pathEnd;
        public List<LevelsSetup> setLevelList;
        
        public string GetName()
        {
            return levelNameLocale.IsEmpty ? name : levelNameLocale.GetLocalizedString();
        }
    }
    
    [System.Serializable]
    public struct LevelsSetup
    {
        public GameObject segmentObject;
        public GameObject enemySetObject; // GameObject do conjunto de inimigos
        public LevelTypes levelType;
    }
    public enum LevelTypes {  Multi, Grass, Forest, Swamp, Antique, Desert, Ice, Boss }
}