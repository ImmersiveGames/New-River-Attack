using UnityEngine;
namespace RiverAttack
{
    public class ObjectInScreenAchievement: MonoBehaviour
    {
        [SerializeField]
        string idAchievement;
        void OnBecameVisible()
        {
            GameSteamManager.UnlockAchievement(idAchievement);
        }
    }
}
