using UnityEngine;
namespace RiverAttack
{
    public class UiButtonBomb : MonoBehaviour
    {
        const string BUTTON_TRIGGER = "IsPressed";
        GamePlayManager m_GamePlayManager;
        Animator m_Animator;
        static readonly int IsPressed = Animator.StringToHash(BUTTON_TRIGGER);
        void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_Animator = GetComponent<Animator>();
            m_GamePlayManager.EventPlayerPushButtonBomb += ActionShoot;
        }

        void ActionShoot()
        {
            if (m_Animator != null)
            {
                m_Animator.SetTrigger(IsPressed);
            }
        }

        void OnDisable()
        {
            m_GamePlayManager.EventPlayerPushButtonBomb -= ActionShoot;
        }
    }
}
