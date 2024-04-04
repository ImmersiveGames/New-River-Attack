using UnityEngine;

namespace ImmersiveGames.MenuManagers.NotificationManager
{
    [System.Serializable]
    public class NotificationData
    {
        public GameObject panelPrefab;
        public string message;
        public System.Action confirmAction;
    }
}