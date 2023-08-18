using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class GameMissionBuilder : MonoBehaviour
    {
        [Header("Level Settings"), SerializeField]
        Levels actualLevel;
        int m_ActualPathIndex;
        [SerializeField]
        int maxLevels = 8;
        
        [Header("Level HUB Settings")]
        public List<Levels> levelsFinishList = new List<Levels>();
        [SerializeField]
        List<Levels> previousLevelList = new List<Levels>();
        
        [Header("INTERNAL SETTINGS")]
        [SerializeField]
        List<float> pathMilestones = new List<float>();
        [SerializeField]
        List<GameObject> poolPathLevels = new List<GameObject>();
        [SerializeField]
        List<GameObject> poolEnemyLevels = new List<GameObject>();

        GamePlayManager m_GamePlayManager;



#region UNITYMETHODS
        void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
        }
        void Start()
        {
            poolPathLevels = new List<GameObject>();
            poolEnemyLevels = new List<GameObject>();
            pathMilestones = new List<float>();
            if (GameManager.instance.debugMode) return;
            StartBuildMission(actualLevel);
        }
  #endregion
        
        void StartBuildMission(Levels level)
        {
            var levelRoot = new GameObject();
            m_ActualPathIndex = 0;
            levelRoot.name = level.levelName;
            CreateLevel(level, levelRoot.transform);
        }
        
        void CreateLevel(Levels level, Transform myRoot = null)
        {
            if (level.setLevelList.Count <= 0) return;
            var nextBound = new Vector3(level.levelOffset.x, level.levelOffset.y, level.levelOffset.z);
            int numPatches = level.setLevelList.Count;
            
            FixedPath(ref nextBound, level.pathStart, myRoot);
            //Debug.Log($"Path Start: {nextBound}");
           
            for (int i = 0; i < numPatches; i++)
            {
                //Debug.Log($"Path Next Start: {nextBound}");
                nextBound.x = level.levelOffset.x;
                BuildEnemies(nextBound,level.setLevelList[i], i, myRoot);
                nextBound.x = level.levelOffset.x;
                poolPathLevels.Add(BuildPath(ref nextBound, level.setLevelList[i].levelPaths, myRoot));
                if (maxLevels > i)
                    poolPathLevels[i].SetActive(true);
            }
            if (level.pathEnd == null) return;
            //Todo Corrigir no Update das fazes quando for as ultimas.
            FixedPath(ref nextBound, level.pathEnd, myRoot);
        }
        void FixedPath(ref Vector3 nextBound, GameObject nextPath, Transform myRoot)
        {
            if (nextPath == null)
                return;
            var path = BuildPath(ref nextBound, nextPath, myRoot);
            path.SetActive(true);
        }
        GameObject BuildPath(ref Vector3 nextBound, GameObject nextPath, Transform myRoot)
        {
            var patch = Instantiate(nextPath, myRoot);
            patch.SetActive(false);
            var bound = MashTriangulation.GetChildRenderBounds(patch);
            //Debug.Log($"Tamanho do Trecho: {bound.size}");
            patch.transform.position = nextBound;
            nextBound += new Vector3(actualLevel.levelOffset.x, actualLevel.levelOffset.y, bound.size.z);
            pathMilestones.Add(nextBound.z);
            return patch;
        }
        void BuildEnemies(Vector3 nextBound, LevelsSetup levelsSetup, int i, Transform myRoot)
        {
            if (levelsSetup.enemiesSets == null) return;
            levelsSetup.enemiesSets.SetActive(false);
            var enemies = Instantiate(levelsSetup.enemiesSets, myRoot);
            enemies.transform.position = nextBound;
            if (maxLevels > i)
                enemies.SetActive(true);
            poolEnemyLevels.Add(enemies);
        }
        // Quando ele pode ser chamado? Player morre? Player passa uma Ponte parece ser o ideal
        void CheckPoolLevel(float posZ)
        {
            if (m_GamePlayManager.completePath || !(pathMilestones[m_ActualPathIndex] - posZ <= 0))
                return;
            UpdatePoolLevel(poolPathLevels, m_ActualPathIndex);
            UpdatePoolLevel(poolEnemyLevels, m_ActualPathIndex);
            m_ActualPathIndex++;
        }

        void UpdatePoolLevel(IReadOnlyList<GameObject> pool, int actualHandle)
        {
            int activeIndex = actualHandle + (maxLevels - 1);
            int deactivateIndex = actualHandle - (maxLevels - 1);
            int removeIndex = actualHandle - (maxLevels - 2);

            if (activeIndex < pool.Count && !pool[activeIndex].activeInHierarchy)
                pool[activeIndex].SetActive(true);

            if (deactivateIndex >= 0 && deactivateIndex < pool.Count - maxLevels && pool[deactivateIndex].activeInHierarchy)
                pool[deactivateIndex].SetActive(false);

            if (removeIndex >= 0 && removeIndex < pool.Count - maxLevels && !pool[removeIndex].activeInHierarchy)
                Destroy(pool[removeIndex]);
        }
    
    }
}


