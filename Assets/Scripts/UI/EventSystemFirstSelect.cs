using UnityEngine;
using UnityEngine.EventSystems;
namespace RiverAttack
{
    public class EventSystemFirstSelect : MonoBehaviour
    {
        EventSystem m_EventSystem;
        void Awake()
        {
            m_EventSystem = EventSystem.current;
        }
        public void Init()
        {
            m_EventSystem.SetSelectedGameObject(gameObject);
        }
    }
}