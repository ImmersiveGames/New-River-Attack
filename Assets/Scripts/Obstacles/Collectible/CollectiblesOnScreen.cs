using UnityEngine;

namespace  RiverAttack
{
    
    [RequireComponent(typeof(Renderer))]
    public class CollectiblesOnScreen : MonoBehaviour
    {
        private CollectiblesMaster collectiblesMaster;
        private void OnEnable()
        {
            collectiblesMaster = GetComponentInParent<CollectiblesMaster>();
        }

        private void OnBecameVisible()
        {
            collectiblesMaster.CallShowOnScreen();
        }
    }
}
