using UnityEngine;
using UnityEngine.InputSystem;

namespace ImmersiveGames.InputManager.InputActionAssets
{
    [CreateAssetMenu(fileName = "InputActionAssetReference", menuName = "ImmersiveGames/InputActions/Input Action Reference", order = 1)]
    public class InputActionAssetReference : ScriptableObject
    {
        public InputActionAsset inputActionAsset;
    }
}