using System.Collections;
using System.Collections.Generic;
using ImmersiveGames.Utils;
using NewRiverAttack.GameManagers;
using NewRiverAttack.HUBManagers.UI;
using NewRiverAttack.LevelBuilder;
using UnityEngine;

namespace NewRiverAttack.HUBManagers
{
    public class HubBuilder : MonoBehaviour
    {
        public Vector3 offset;
        private List<LevelData> _listLevelDatas;
        private LevelListData _levelList;
        private HubGameManager _hubGameManager;
        private GameObject _hubRoot;
        private GameObject _setsContainer;
        private bool _finishBuild;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _setsContainer = new GameObject("HUB");
            BuildHub(_listLevelDatas);
        }

        private void Start()
        {
            StartCoroutine(WaitForInitialization());
        }

        private void OnDestroy()
        {
            _listLevelDatas = null;
            Destroy(_setsContainer);
        }

        #endregion

        private IEnumerator WaitForInitialization()
        {
            while (!_finishBuild)
            {
                yield return null;
            }
            _finishBuild = false;
            _hubGameManager.OnEventInitializeHub();
        }
        private void SetInitialReferences()
        {
            _finishBuild = false;
            _hubGameManager = HubGameManager.instance;
            _levelList = GameManager.instance.missionModeLevels;
            _listLevelDatas = _levelList.value;
        }

        private void BuildHub(IReadOnlyList<LevelData> listLevelDatas)
        {
            float nextPosition = 0;
            float previousSize = 0;
            foreach (var data in listLevelDatas)
            {
                var hudData = data.hudPath;
                var newSegment = Instantiate(hudData.segmentObject, Vector3.zero, Quaternion.identity, _setsContainer.transform);

                var actualSize = CalculateRealLength.GetBounds(newSegment).size.z;

                nextPosition += previousSize;
                var position1 = newSegment.transform.position;
                position1 = new Vector3(position1.x, position1.y, nextPosition);
                newSegment.transform.position = position1 + offset;
                previousSize = actualSize;

                SetSegment(newSegment, data);
            }
            _finishBuild = true;
        }
        private void SetSegment(GameObject segment, LevelData levelData)
        {
            var hubIcon = segment.GetComponentInChildren<UiHubIcons>();
            var hubBridge = segment.GetComponentInChildren<UiHubBridges>();
            if (hubIcon == null) return;
            _hubGameManager.LevelOrder.Add(new HubOrderData(hubIcon, hubIcon.transform.position.z, levelData, hubBridge));
            var actualIndex = _hubGameManager.LevelOrder.Count - 1;
            hubIcon.SetIcon(levelData, actualIndex);
            hubBridge.SetBridge(actualIndex);
        }
        
    }
}