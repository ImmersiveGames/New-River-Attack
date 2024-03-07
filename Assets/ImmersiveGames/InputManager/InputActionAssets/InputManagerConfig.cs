using UnityEngine;

namespace ImmersiveGames.InputManager.InputActionAssets
{
    [CreateAssetMenu(fileName = "InputManagerConfig", menuName = "ImmersiveGames/InputManagerConfig", order = 1)]
    public class InputManagerConfig : ScriptableObject
    {
        public string selectedInputActionMapName = "UI_Controlls"; 
        public string escNotificationActionName = "EscNotification";
    }
}