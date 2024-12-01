using TMPro;
using UnityEngine;

namespace NewRiverAttack.HUBManagers.UI
{
    public class UiHubTextMission : MonoBehaviour
    {
        private HubGameManager _hubGameManager;
        private TMP_Text _text; // Componente de texto TMP para exibir o nome da missão

        private void Awake()
        {
            SetInitialReferences();
        }

        private void OnEnable()
        {
            _hubGameManager.EventBuildHub += InitializeName;
            _hubGameManager.EventUpdateHub += UpdateName;
        }


        private void OnDisable()
        {
            _hubGameManager.EventBuildHub -= InitializeName;
        }

        private void SetInitialReferences()
        {
            _hubGameManager = HubGameManager.Instance;
            _text = GetComponent<TMP_Text>();
        }

        private void InitializeName()
        {
            var actualLevelData = _hubGameManager.GetActualDataSave();
            if (actualLevelData == null) return;
            _text.text = actualLevelData.GetName();
        }

        private void UpdateName(int indexHub)
        {
            var actualLevelData = _hubGameManager.GetLevelDataByIndex(indexHub);
            if (actualLevelData == null) return;
            _text.text = actualLevelData.GetName();
        }
    }
}