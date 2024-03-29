using UnityEngine;

namespace RiverAttack
{
    public class PanelSplashScreen : MonoBehaviour
    {
        [Header("Menu Fades")]
        public Transform panelFade;
        public Animator fadeAnimator;
        float m_FadeInTime;
        float m_FadeOutTime;
        static readonly int FadeIn = Animator.StringToHash("FadeIn");
        static readonly int FadeOut = Animator.StringToHash("FadeOut");
        // Start is called before the first frame update
        void Start()
        {
            PerformFadeOut();
        }
        void PerformFadeOut()
        {
            fadeAnimator.SetTrigger(FadeOut);
        }
        public void PerformFadeIn()
        {
            fadeAnimator.SetTrigger(FadeIn);
        }
    }
}


