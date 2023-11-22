using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class GameHudBuilder : MonoBehaviour
    {
        
        [SerializeField]
        List<GameObject> poolPathLevels = new List<GameObject>();

        GameObject m_LevelRoot;
        const float PLAYER_OFFSET = 5f;

        void OnEnable()
        {
            StartBuildHUD(GameHubManager.instance.missionListLevels);
        }

        void OnDisable()
        {
            DisableBuildMission();
        }
        void StartBuildHUD(ListLevels level)
        {
            m_LevelRoot = new GameObject
            {
                name = "HUB"
            };
            CreateLevel(level, m_LevelRoot.transform);
        }
        void CreateLevel(ListLevels listHudLevels, Transform myRoot = null)
        {
            var nextBound = new Vector3(listHudLevels.offset.x, listHudLevels.offset.y, listHudLevels.offset.z);
            for (int j = 0; j < listHudLevels.count; j++)
            {
                var level = listHudLevels.Index(j);
                if (level.hudPath == null) return;
                poolPathLevels.Add(BuildPath(ref nextBound, level, level.hudPath, myRoot));
                if (level)
                {
                    var icons = poolPathLevels[j].GetComponentInChildren<UiHubIcons>();
                    var bridges = poolPathLevels[j].GetComponentInChildren<UiHubBridges>();
                    if (icons)
                    {
                        icons.level = level;
                        icons.myIndex = j;
                    }
                    if (bridges)
                    {
                        bridges.level = level;
                        bridges.myIndex = j;
                    }
                }
                poolPathLevels[j].SetActive(true);
            }
        }
        /*void FixedPath(ref Vector3 nextBound, Levels level, GameObject nextPath, Transform myRoot)
        {
            if (nextPath == null)
                return;
            var path = BuildPath(ref nextBound, level, nextPath, myRoot);
            path.SetActive(true);
        }*/
        GameObject BuildPath(ref Vector3 nextBound, Levels level, GameObject nextPath, Transform myRoot)
        {
            var patch = Instantiate(nextPath, myRoot);
            patch.SetActive(false);
            var bound = MashTriangulation.GetChildRenderBounds(patch);
            //Debug.Log($"Tamanho do Trecho: {bound.size}");
            patch.transform.position = nextBound;
            float milestone = (level.dontCountMilestone) ? -1f : nextBound.z + PLAYER_OFFSET;
            GameHubManager.instance.hubMilestones.Add(milestone);
            nextBound += new Vector3(0, 0, bound.size.z);
            return patch;
        }
        void DisableBuildMission()
        {
            poolPathLevels = new List<GameObject>();
            Destroy(m_LevelRoot);
        }
    }
}
