using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ImmersiveGames.MenuManagers.PanelGameManagers
{
    public class PanelGamePause : MonoBehaviour
    {
        [SerializeField] private RectTransform hubButton;
        [SerializeField] private Button firstButton;
        private void OnEnable()
        {
            hubButton.gameObject.SetActive(false);
            if(GameManager.instance.gamePlayMode == GamePlayModes.MissionMode)
                hubButton.gameObject.SetActive(true);
            FocusButton();
        }
        private void FocusButton()
        {
            firstButton.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
        }
    }
}