using UnityEngine;
namespace RiverAttack
{
    public class ObjectInScreenAchievement: MonoBehaviour
    {
        [SerializeField] private string idAchievement;

        private void OnBecameVisible()
        {
            GameSteamManager.UnlockAchievement(idAchievement);
        }
    }
}
