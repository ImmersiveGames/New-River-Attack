using UnityEngine;
using UnityEngine.EventSystems;
namespace RiverAttack
{
    public class EventSystemFirstSelect : MonoBehaviour
    {
        private EventSystem m_EventSystem;

        private void Awake()
        {
            m_EventSystem = EventSystem.current;
        }
        public void Init()
        {
            m_EventSystem.SetSelectedGameObject(gameObject);
        }
    }
}