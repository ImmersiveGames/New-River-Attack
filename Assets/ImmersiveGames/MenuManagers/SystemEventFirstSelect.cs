using UnityEngine;
using ImmersiveGames.DebugManagers;
using UnityEngine.EventSystems;

namespace ImmersiveGames.MenuManagers
{
    public class SystemEventFirstSelect : MonoBehaviour
    {
        private EventSystem _eventSystem;

        public void Init()
        {
            _eventSystem = EventSystem.current;
            DebugManager.Log($"Iniciou o botão e o EventSystem {_eventSystem}");
            _eventSystem.SetSelectedGameObject(gameObject);
        }
    }
}
