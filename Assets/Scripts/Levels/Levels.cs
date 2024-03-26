using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace RiverAttack
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "RiverAttack/Levels", order = 102)]
    [System.Serializable]
    public class Levels : ScriptableObject
    {
        [Header("Level Settings")]
        public string levelName;
        public LocalizedString levelNameLocale;
#if UNITY_EDITOR
        [Multiline] public string developerDescription = "";
#endif
        public bool beatGame;
        public bool dontCountMilestone;
        public bool bossFight;

        [Header("HUD Settings")] [SerializeField]
        internal GameObject hudPath;

        public LevelsStates levelsStates = LevelsStates.Locked;

        [FormerlySerializedAs("bgmStartLevel")] [Header("Build Settings")]
        public LevelTypes pathType;

        [SerializeField] internal GameObject pathStart;
        [SerializeField] internal GameObject pathEnd;
        [SerializeField] internal Vector3 levelOffset;
        [SerializeField] internal List<LevelsSetup> setLevelList;
    }

    public enum LevelsStates
    {
        Locked, // Não é possível acessar - Vermelho
        Actual, // Level selecionado - Amarelo
        Complete, // nível foi jogado e deve ser concluído assim que o jogador voltar pra HUB destruir pónte etc. - temp Verde
        Open // é possível retornar a estes níveis já jogados - Branco
    }

    [System.Serializable]
    public struct LevelsSetup
    {
        [FormerlySerializedAs("levelPaths")] public GameObject segmentObject;
        [FormerlySerializedAs("enemiesSets")] public GameObject enemySetObject;
        public float absolutePosition;
        public BgmTypes bgmLevel;
    }

    public enum LevelTypes
    {
        Multi = 1 << 5,
        Grass = 1 << 6,
        Forest = 1 << 7,
        Swamp = 1 << 8,
        Antique = 1 << 9,
        Desert = 1 << 10,
        Ice = 1 << 11,
        Boss = 1 << 12
    }
}