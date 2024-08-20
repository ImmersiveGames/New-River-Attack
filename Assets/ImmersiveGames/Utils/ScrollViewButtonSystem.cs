using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveGames.Utils
{
    public class ScrollViewButtonSystem : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private float scrollSpeed = 0.1f;

        public void VerticalScrollUp()
        {
            if (scrollRect == null)
                return;
            if (scrollRect.verticalNormalizedPosition <= 1f) 
            {
                scrollRect.verticalNormalizedPosition += scrollSpeed;
            }
        }

        public void VerticalScrollDown()
        {
            if (scrollRect == null)
                return;
            if (scrollRect.verticalNormalizedPosition >= 0f)
            {
                scrollRect.verticalNormalizedPosition -= scrollSpeed;
            }
        }
    }
}
