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

        private GameObject _setsContainer;
        private List<LevelData> _listLevelDatas;
        private HubGameManager _hubGameManager;

        #region Unity Methods

        private void Awake()
        {
            SetInitialReferences();
        }

        private void OnEnable()
        {
            CreateHubContainer();
        }

        private void Start()
        {
            BuildHub(_listLevelDatas);
        }

        private void OnDisable()
        {
            Cleanup();
        }

        #endregion

        private void SetInitialReferences()
        {
            _hubGameManager = HubGameManager.Instance;
            _listLevelDatas = GameManager.instance.missionModeLevels.value;
        }

        private void CreateHubContainer()
        {
            if (_setsContainer == null)
            {
                _setsContainer = new GameObject("HUB");
            }
        }

        private void Cleanup()
        {
            _listLevelDatas = null;
            if (_setsContainer == null) return;
            Destroy(_setsContainer);
            _setsContainer = null;
        }

        private void BuildHub(IReadOnlyList<LevelData> listLevelDatas)
        {
            if (listLevelDatas == null || listLevelDatas.Count == 0) return;
            
            CreateHubStart(listLevelDatas);
            _hubGameManager?.OnEventBuildHub();
        }

        private void CreateHubStart(IReadOnlyList<LevelData> listLevelDatas)
        {
            float nextPosition = 0f, previousSize = 0f;

            foreach (var data in listLevelDatas)
            {
                var hudData = data.hudPath;
                if (hudData.segmentObject == null) continue;

                var newSegment = Instantiate(
                    hudData.segmentObject,
                    Vector3.zero,
                    Quaternion.identity,
                    _setsContainer.transform
                );
                var actualSize = CalculateRealLength.GetBounds(newSegment).size.z;
                nextPosition += previousSize;

                var segmentPosition = new Vector3(0, 0, nextPosition) + offset;
                newSegment.transform.localPosition = segmentPosition;

                _hubGameManager.CachedHubOrderData.Add(new HubOrderData(
                    newSegment.GetComponentInChildren<UiHubIcons>(),
                    segmentPosition.z,
                    data,
                    newSegment.GetComponentInChildren<UiHubBridges>()
                ));
                ConfigureSegment(_hubGameManager.CachedHubOrderData, _hubGameManager.CachedHubOrderData.Count -1);

                previousSize = actualSize;
            }
        }

        private void ConfigureSegment(List<HubOrderData> data, int index)
        {
            var hubIcon = data[index].icon;
            var hubBridge = data[index].bridge;

            if (hubIcon == null || hubBridge == null) return;

            hubIcon.SetIcon(data[index].levelData, index);
            hubBridge.SetBridge(index);
        }
    }
}
