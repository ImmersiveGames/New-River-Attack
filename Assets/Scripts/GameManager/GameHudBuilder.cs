using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace RiverAttack
{
    public class GameHudBuilder : MonoBehaviour
    {
        [SerializeField]
        ListLevels listLevels;
        GameObject m_LevelRoot;
        [SerializeField]
        List<GameObject> poolPathLevels = new List<GameObject>();

        void Start()
        {
            StartBuildHUD(listLevels);
        }
        void StartBuildHUD(ListLevels level)
        {
            m_LevelRoot = new GameObject
            {
                name = "HUD"
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
                poolPathLevels.Add(BuildPath(ref nextBound, level.hudPath, myRoot));
                poolPathLevels[j].SetActive(true);
            }
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
            nextBound += new Vector3(0, 0, bound.size.z);
            return patch;
        }
    }
}
