using UnityEngine;

namespace RiverAttack
{
    public class PanelSplashScreen : MonoBehaviour
    {
        [Header("Menu Fades")]
        public Transform panelFade;
        public Animator fadeAnimator;
        private float m_FadeInTime;
        private float m_FadeOutTime;
        private static readonly int FadeIn = Animator.StringToHash("FadeIn");

        private static readonly int FadeOut = Animator.StringToHash("FadeOut");
        // Start is called before the first frame update
        private void Start()
        {
            PerformFadeOut();
        }

        private void PerformFadeOut()
        {
            fadeAnimator.SetTrigger(FadeOut);
        }
        public void PerformFadeIn()
        {
            fadeAnimator.SetTrigger(FadeIn);
        }
    }
}


