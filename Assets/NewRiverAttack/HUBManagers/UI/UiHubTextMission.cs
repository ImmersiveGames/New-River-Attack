using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NewRiverAttack.HUBManagers.UI
{
    public class UiHubTextMission : MonoBehaviour
    {
        private HubGameManager _hubGameManager;
        private TMP_Text _text;

        private void OnEnable()
        {
            SetInitialReferences();
            _hubGameManager.EventInitializeHub += InitializeName;
            _hubGameManager.EventCursorUpdateHub += InitializeName;
        }

        private void OnDisable()
        {
            _hubGameManager.EventInitializeHub -= InitializeName;
            _hubGameManager.EventCursorUpdateHub -= InitializeName;
        }

        private void SetInitialReferences()
        {
            _hubGameManager = HubGameManager.instance;
            _text = GetComponent<TMP_Text>();
        }
        
        private void InitializeName(List<HubOrderData> hubOrderData, int startIndex)
        {
            _text.text = hubOrderData[startIndex].levelData.GetName();
        }
    }
}