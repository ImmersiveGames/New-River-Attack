using UnityEngine;
using UnityEngine.UI;
namespace RiverAttack
{
    public class UiButtonMissionOnly: MonoBehaviour
    {
        void OnEnable()
        {
            ActiveButton();
        }

        void ActiveButton()
        {
            var gameMode = GameManager.instance.gameModes;
            var button = GetComponent<Button>();
            button.enabled = gameMode != GameManager.GameModes.Mission;
            
        }
    }
}
