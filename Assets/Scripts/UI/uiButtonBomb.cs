using UnityEngine;
namespace RiverAttack
{
    public class UiButtonBomb : MonoBehaviour
    {
        private const string BUTTON_TRIGGER = "IsPressed";
        private GamePlayManager m_GamePlayManager;
        private Animator m_Animator;
        private static readonly int IsPressed = Animator.StringToHash(BUTTON_TRIGGER);

        private void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_Animator = GetComponent<Animator>();
            m_GamePlayManager.EventPlayerPushButtonBomb += ActionShoot;
        }

        private void ActionShoot()
        {
            if (m_Animator != null)
            {
                m_Animator.SetTrigger(IsPressed);
            }
        }

        private void OnDisable()
        {
            m_GamePlayManager.EventPlayerPushButtonBomb -= ActionShoot;
        }
    }
}
