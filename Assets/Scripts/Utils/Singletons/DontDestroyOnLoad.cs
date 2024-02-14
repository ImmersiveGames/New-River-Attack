using UnityEngine;
namespace Utils
{
    namespace RiverAttack
    {
        public class DontDestroyOnLoad : MonoBehaviour
        {
            private void Awake()
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
    
}
