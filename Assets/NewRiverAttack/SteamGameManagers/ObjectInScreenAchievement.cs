using UnityEngine;

namespace NewRiverAttack.SteamGameManagers
{
    public class ObjectInScreenAchievement: MonoBehaviour
    {
        [SerializeField] private string idAchievement;

        private void OnBecameVisible()
        {
            SteamGameManager.UnlockAchievement(idAchievement);
        }
    }
}
