using UnityEngine;
namespace RiverAttack
{
    public class ButtonActiveMissionOnly: MonoBehaviour
    {
        void Start()
        {
            if (GameManager.instance.gameModes != GameManager.GameModes.Mission)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
