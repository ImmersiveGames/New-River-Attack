using ImmersiveGames.InputManager.InputActionAssets;
using UnityEngine;

namespace ImmersiveGames.InputManager
{
    public class TestResourceLoading : MonoBehaviour
    {
        private void Start()
        {
            LoadConfigFromResources();
        }

        private void LoadConfigFromResources()
        {
            var config = Resources.Load<InputManagerConfig>("InputManagerConfig");

            if (config != null)
            {
                Debug.Log("InputManagerConfig loaded successfully!");
                Debug.Log($"Selected InputActionMap: {config.selectedInputActionMapName}");
                Debug.Log($"EscNotification ActionName: {config.escNotificationActionName}");
            }
            else
            {
                Debug.LogError("InputManagerConfig not found. Make sure it's in the 'Resources' folder.");
            }
        }
    }
}