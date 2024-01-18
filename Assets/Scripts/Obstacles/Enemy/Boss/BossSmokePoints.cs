using System.Linq;
using UnityEngine;
namespace RiverAttack
{
    public class BossSmokePoints: MonoBehaviour
    {
        internal Transform FindChildWithoutChild()
        {

            return transform.Cast<Transform>().FirstOrDefault(child => child.childCount == 0);
        }
    }
}
