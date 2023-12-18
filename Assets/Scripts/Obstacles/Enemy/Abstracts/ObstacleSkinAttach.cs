using UnityEngine;
namespace Obstacles.Enemy.Abstracts
{
    public class ObstacleSkinAttach : MonoBehaviour
    {
        public void ToggleSkin(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
