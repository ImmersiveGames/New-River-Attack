using UnityEngine;
using UnityEngine.EventSystems;

namespace ImmersiveGames.Utils
{
    public class SystemEventFirstSelect : MonoBehaviour
    {
        private EventSystem _eventSystem;

        private void Awake()
        {
            _eventSystem = EventSystem.current;
        }
        public void Init()
        {
            _eventSystem.SetSelectedGameObject(gameObject);
        }
    }
}