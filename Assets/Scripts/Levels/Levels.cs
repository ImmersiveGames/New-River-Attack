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
        public bool dontCountMilestone;
        public bool bossFight;

        [Header("HUD Settings")]
        [SerializeField] internal GameObject hudPath;
        public LevelsStates levelsStates = LevelsStates.Locked;

        [Header("Build Settings")]
        public LevelTypes bgmStartLevel;
        [SerializeField] internal GameObject pathStart;
        [SerializeField] internal GameObject pathEnd;
        [SerializeField] internal Vector3 levelOffset;
        [SerializeField] internal List<LevelsSetup> setLevelList;
        
    }
    
    public enum LevelsStates
    {
        Locked,   // Não é possivel acessar - Vermelho
        Actual,   // Level selecionado - Amarelo
        Complete, // nivel foi jogado e deve ser concluido assim que o jogador voltar pra HUB destruir pónte etc. - temp Verde
        Open      // é possivel retornar a estes niveis já jogados - Branco
            
    }

    [System.Serializable]
    public struct LevelsSetup
    {
        public GameObject levelPaths;
        public GameObject enemiesSets;
        public LevelTypes bgmLevel;
    }
}
