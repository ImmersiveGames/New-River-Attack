using UnityEngine;
using UnityEngine.UI;

public class ScrollViewButtonSystem : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] float scrollSpeed = 0.1f;

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
