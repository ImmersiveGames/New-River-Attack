using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ImmersiveGames.MenuManagers.PanelGameManagers
{
    public class PanelGameOver : MonoBehaviour
    {
        [SerializeField] private Button firstMenu;
        [SerializeField] private RectTransform hubButton;
        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(null);
            // Coloque o botão como o objeto selecionado
            EventSystem.current.firstSelectedGameObject = firstMenu.gameObject;
            EventSystem.current.SetSelectedGameObject(firstMenu.gameObject);
            
            hubButton.gameObject.SetActive(false);
            if(GameManager.instance.gamePlayMode == GamePlayModes.MissionMode)
                hubButton.gameObject.SetActive(true);
        }
    }
}