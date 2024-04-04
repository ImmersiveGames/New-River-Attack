using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace ImmersiveGames.LevelBuilder
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "ImmersiveGames/RiverAttack/Levels", order = 201)]
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
        public GameObject hudPath;
        public LevelsStates levelsStates = LevelsStates.Locked;
        
        [Header("Build Settings")]
        public GameObject pathStart;
        public GameObject pathEnd;
        public List<LevelsSetup> setLevelList;
    }
    
    [System.Serializable]
    public struct LevelsSetup
    {
        public GameObject segmentObject;
        public GameObject enemySetObject; // GameObject do conjunto de inimigos
        public LevelTypes bgmLevel;
    }
    
    public enum LevelsStates
    {
        Locked,   // Não é possível acessar - Vermelho
        Actual,   // Level selecionado - Amarelo
        Complete, // nível foi jogado e deve ser concluído assim que o jogador voltar pra HUB destruir pónte etc. - temp Verde
        Open      // é possível retornar a estes nível já jogados - Branco
    }
    public enum LevelTypes {  Multi, Grass, Forest, Swamp, Antique, Desert, Ice, Boss }
}