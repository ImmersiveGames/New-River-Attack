using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class WallsMaster : MonoBehaviour
    {
        void OnEnable()
        {
            Tools.SetLayersRecursively(GameManager.instance.layerWall, transform);
        }

    }
}
