using UnityEngine;
using UnityEngine.UI;
namespace RiverAttack
{
    public class UiButtonShoot : MonoBehaviour
    {
        private const string BUTTON_TRIGGER = "IsPressed";
        private GamePlayManager m_GamePlayManager;
        private Animator m_Animator;
        private static readonly int IsPressed = Animator.StringToHash(BUTTON_TRIGGER);
        [SerializeField] private Image rapidFireImage;

        private void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_Animator = GetComponent<Animator>();
            
            m_GamePlayManager.EventPlayerPushButtonShoot += ActionShoot;
            
        }

        private void Start()
        {
            rapidFireImage.enabled = false;
            m_GamePlayManager.EventStartRapidFire += StartRapidFire;
            m_GamePlayManager.EventEndRapidFire += EndRapidFire;
        }

        private void ActionShoot()
        {
            if (m_Animator != null)
            {
                m_Animator.SetTrigger(IsPressed);
            }
        }

        private void StartRapidFire()
        {
            rapidFireImage.enabled = true;
        }

        private void EndRapidFire()
        {
            rapidFireImage.enabled = false;
        }

        private void OnDisable()
        {
            m_GamePlayManager.EventPlayerPushButtonShoot -= ActionShoot;
            m_GamePlayManager.EventStartRapidFire -= StartRapidFire;
            m_GamePlayManager.EventEndRapidFire -= EndRapidFire;
        }
    }
}
