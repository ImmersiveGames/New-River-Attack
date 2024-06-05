using System.Collections.Generic;
using NewRiverAttack.HUBManagers.UI;
using UnityEngine;
using Utils;
using Utils.TriangulatorHelper;

namespace RiverAttack
{
    public class GameHudBuilder : MonoBehaviour
    {
        
        [SerializeField] private List<GameObject> poolPathLevels = new List<GameObject>();

        private GameObject m_LevelRoot;
        private const float PLAYER_OFFSET = 5f;

        private void OnEnable()
        {
            StartBuildHUD(GameManager.instance.missionLevels);
        }

        private void OnDisable()
        {
            DisableBuildMission();
        }

        private void StartBuildHUD(ListLevels level)
        {
            m_LevelRoot = new GameObject
            {
                name = "HUB"
            };
            CreateLevel(level, m_LevelRoot.transform);
        }

        private void CreateLevel(ListLevels listHudLevels, Transform myRoot = null)
        {
            var nextBound = new Vector3(listHudLevels.offset.x, listHudLevels.offset.y, listHudLevels.offset.z);
            for (int j = 0; j < listHudLevels.count; j++)
            {
                var level = listHudLevels.Index(j);
                if (level.hudPath == null) return;
                poolPathLevels.Add(BuildPath(ref nextBound, level, myRoot));
                poolPathLevels[j].SetActive(true);
            }
        }
        /*
        void FixedPath(ref Vector3 nextBound, Levels level, GameObject nextPath, Transform myRoot)
        {
            if (nextPath == null)
                return;
            var path = BuildPath(ref nextBound, level, nextPath, myRoot);
            path.SetActive(true);
        }
        */
        private static GameObject BuildPath(ref Vector3 nextBound, Levels level, Transform myRoot)
        {
            var patch = Instantiate(level.hudPath, myRoot);
            patch.SetActive(false);
            var bound = MashTriangulation.GetChildRenderBounds(patch);
            //Debug.Log($"Tamanho do Trecho: {bound.size}");
            patch.transform.position = nextBound;
            
            if (!level.dontCountMilestone)
            {
                // Inicialização congig dos trechos.
                
                GameHubManager.instance.missions.Add(new HubMissions
                {
                    position = nextBound.z + PLAYER_OFFSET,
                    levels = level
                });
                //patch.GetComponentInChildren<UiHubBridges>().level = level;
                //patch.GetComponentInChildren<UiHubIcons>().Initialization(level);
            }
            nextBound += new Vector3(0, 0, bound.size.z);
            return patch;
        }

        private void DisableBuildMission()
        {
            poolPathLevels = new List<GameObject>();
            Destroy(m_LevelRoot);
        }
    }
    
    
}
