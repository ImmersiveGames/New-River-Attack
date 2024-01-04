using UnityEngine;
using UnityEngine.UI;
namespace RiverAttack
{
    public class UiButtonShoot : MonoBehaviour
    {
        const string BUTTON_TRIGGER = "IsPressed";
        GamePlayManager m_GamePlayManager;
        Animator m_Animator;
        static readonly int IsPressed = Animator.StringToHash(BUTTON_TRIGGER);
        [SerializeField] Image rapidFireImage;
        void OnEnable()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_Animator = GetComponent<Animator>();
            
            m_GamePlayManager.EventPlayerPushButtonShoot += ActionShoot;
            
        }
        void Start()
        {
            rapidFireImage.enabled = false;
            m_GamePlayManager.EventStartRapidFire += StartRapidFire;
            m_GamePlayManager.EventEndRapidFire += EndRapidFire;
        }

        void ActionShoot()
        {
            if (m_Animator != null)
            {
                m_Animator.SetTrigger(IsPressed);
            }
        }

        void StartRapidFire()
        {
            rapidFireImage.enabled = true;
        }
        void EndRapidFire()
        {
            rapidFireImage.enabled = false;
        }

        void OnDisable()
        {
            m_GamePlayManager.EventPlayerPushButtonShoot -= ActionShoot;
            m_GamePlayManager.EventStartRapidFire -= StartRapidFire;
            m_GamePlayManager.EventEndRapidFire -= EndRapidFire;
        }
    }
}
