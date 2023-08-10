using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace RiverAttack
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "RiverAttack/Levels", order = 102)]
    [System.Serializable]
    public class Levels : ScriptableObject
    {
        public string levelName;
        /*[SerializeField]
        public LocalizationString translateName;*/
        [Multiline]
        public string levelDescription;
        /*[SerializeField]
        public LocalizationString translateDedescription;*/
        public Sprite levelIcon;
        public Vector3 levelIconPos;
        public GamePlayAudio.LevelType startLevelBGM;
        public bool beatGame;
        [SerializeField]
        Vector3 levelOffset;
        [SerializeField]
        GameObject pathStart;
        [SerializeField]
        GameObject pathEnd;
        [SerializeField]
        List<LevelsSetup> levelSet;
        [SerializeField]
        List<Levels> previousLevel;
        [SerializeField]
        int maxLevels = 8;

        public List<Levels> previousLevelList { get { return previousLevel; } }

        List<GameObject> m_PoolLevels;
        List<GameObject> m_PoolEnemyLevels;

        public List<Vector3> levelMilestones { get; private set; }
        public void CreateLevel(Transform myRoot = null)
        {
            if (levelSet.Count <= 0) return;
            var nextBound = new Vector3(levelOffset.x, levelOffset.y, levelOffset.z);
            int numPatches = levelSet.Count;
            m_PoolLevels = new List<GameObject>();
            m_PoolEnemyLevels = new List<GameObject>();
            levelMilestones = new List<Vector3>();

            FixedPath(ref nextBound, pathStart, myRoot);
            
            Debug.Log($"Path Start: {nextBound}");
            for (int i = 0; i < numPatches; i++)
            {
                Debug.Log($"Path Start: {nextBound}");
                SetEnemies(nextBound, i, myRoot);
                m_PoolLevels.Add(BuildPath(ref nextBound, levelSet[i].levelPaths, myRoot));
                if (maxLevels > i)
                    m_PoolLevels[i].SetActive(true);
            }
            FixedPath(ref nextBound, pathEnd, myRoot);
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
            Debug.Log($"Tamanho do Trecho: {bound.size}");
            patch.transform.position = nextBound;
            nextBound += new Vector3(levelOffset.x, levelOffset.y, bound.size.z);
            levelMilestones.Add(nextBound);
            return patch;
        }

        public void CallUpdatePoolLevel(int actualHandle)
        {
            UpdatePoolLevel(m_PoolLevels, actualHandle);
            UpdatePoolLevel(m_PoolEnemyLevels, actualHandle);
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

        void SetEnemies(Vector3 nextBound, int i, Transform myRoot)
        {
            if (levelSet[i].enemiesSets == null) return;
            levelSet[i].enemiesSets.SetActive(false);
            var enemies = Instantiate(levelSet[i].enemiesSets, myRoot);
            enemies.transform.position = nextBound;
            if (maxLevels > i)
                enemies.SetActive(true);
            m_PoolEnemyLevels.Add(enemies);
        }
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
        }
    }

    [System.Serializable]
    public struct LevelsSetup
    {
        public GameObject levelPaths;
        public GameObject enemiesSets;
    }
}
