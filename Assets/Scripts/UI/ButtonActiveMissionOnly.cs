using UnityEngine;
using UnityEngine.UI;
namespace RiverAttack
{
    public class ButtonActiveMissionOnly: MonoBehaviour
    {
        private void Start()
        {
            if (GameManager.instance.gameModes != GameManager.GameModes.Mission)
            {
                gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }
}
