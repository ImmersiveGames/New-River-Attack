using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace RiverAttack
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "RiverAttack/Levels", order = 102)]
    [System.Serializable]
    public class Levels : ScriptableObject
    {
        [SerializeField]
        public string levelName;
        /*[SerializeField]
        public LocalizationString translateName;*/
        [SerializeField]
        [Multiline]
        public string levelDescription;
        /*[SerializeField]
        public LocalizationString translateDedescription;*/
        [SerializeField]
        public Sprite levelIcon;
        [SerializeField]
        public Vector3 levelIconPos;
        [SerializeField]
        public GamePlayAudio.LevelType startLevelBGM;
        [SerializeField]
        public bool beatGame;
        [SerializeField]
        private Vector3 levelOffset;
        [SerializeField]
        private GameObject pathStart;
        [SerializeField]
        private GameObject pathEnd;
        [SerializeField]
        private List<LevelsSetup> levelSet;
        [SerializeField]
        private List<Levels> previousLevel;
        [SerializeField]
        private int maxLevels = 8;

        public List<Levels> previousLevelList { get { return previousLevel; } }

        private List<GameObject> PoolLevels;
        private List<GameObject> PoolEnemyLevels;

        public List<Vector3> levelMilstones { get; private set; }
        public void CreateLevel(Transform myroot = null)
        {
            if (levelSet.Count > 0)
            {
                Vector3 nextbound = new Vector3(levelOffset.x, levelOffset.y, levelOffset.z);
                int patchs = levelSet.Count;
                PoolLevels = new List<GameObject>();
                PoolEnemyLevels = new List<GameObject>();
                levelMilstones = new List<Vector3>();

                FixedPath(ref nextbound, pathStart, myroot);
                for (int i = 0; i < patchs; i++)
                {
                    SetEnemys(nextbound, i, myroot);
                    PoolLevels.Add(BuildPath(ref nextbound, levelSet[i].levelPaths, myroot));
                    if (maxLevels > i)
                        PoolLevels[i].SetActive(true);
                }
                FixedPath(ref nextbound, pathEnd, myroot);
            }
        }

        private void FixedPath(ref Vector3 nextbound, GameObject nextPath, Transform myroot)
        {
            if (nextPath != null)
            {
                GameObject path = BuildPath(ref nextbound, nextPath, myroot);
                path.SetActive(true);
            }
        }

        private GameObject BuildPath(ref Vector3 nextbound, GameObject nextPath, Transform myroot)
        {
            GameObject patch = Instantiate(nextPath, myroot);
            patch.SetActive(false);
            Bounds bound = MashTriangulation.GetChildRenderBounds(patch);
            patch.transform.position = nextbound;
            nextbound += new Vector3(levelOffset.x, levelOffset.y, bound.size.z);
            levelMilstones.Add(nextbound);
            return patch;
        }

        public void CallUpdatePoolLevel(int actualHandle)
        {
            UpdatePoolLevel(PoolLevels, actualHandle);
            UpdatePoolLevel(PoolEnemyLevels, actualHandle);
        }

        private void UpdatePoolLevel(List<GameObject> pool, int actualHandle)
        {
            int activeindex = actualHandle + (maxLevels - 1);
            int deactiveindex = actualHandle - (maxLevels - 1);
            int removeindex = actualHandle - (maxLevels - 2);

            if (activeindex < pool.Count && !pool[activeindex].activeInHierarchy)
                pool[activeindex].SetActive(true);

            if ((deactiveindex >= 0 && deactiveindex < pool.Count - maxLevels) && pool[deactiveindex].activeInHierarchy)
                pool[deactiveindex].SetActive(false);

            if ((removeindex >= 0 && removeindex < pool.Count - maxLevels) && !pool[removeindex].activeInHierarchy)
                Destroy(pool[removeindex]);
        }

        private void SetEnemys(Vector3 nextbound, int i, Transform myroot)
        {
            if (levelSet[i].enemysSets != null)
            {
                levelSet[i].enemysSets.SetActive(false);
                GameObject enemys = Instantiate(levelSet[i].enemysSets, myroot);
                enemys.transform.position = nextbound;
                if (maxLevels > i)
                    enemys.SetActive(true);
                PoolEnemyLevels.Add(enemys);
            }
        }


        public bool CheckIfCompleate(List<Levels> finishList)
        {
            return (finishList.Contains(this)) ? true : false;
        }

        public bool CheckIfLastFinish(List<Levels> finishList)
        {
            return (finishList[finishList.Count - 1] == this) ? true : false;
        }

        public bool CheckIfUnloked(Levels previous)
        {
            return (previousLevel.Contains(previous)) ? true : false;
        }

        public bool CheckIfLocked(List<Levels> finishList)
        {
            if (previousLevel.Count <= 0) return false;
            for (int i = 0; i < previousLevel.Count; i++)
            {
                if (finishList.Count >= 1 && finishList.Contains(previousLevel[i])) return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public struct LevelsSetup
    {
        public GameObject levelPaths;
        public GameObject enemysSets;
    }
}
