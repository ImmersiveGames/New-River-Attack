using UnityEngine;
namespace Utils
{
    namespace RiverAttack
    {
        public class DontDestroyOnLoad : MonoBehaviour
        {
            void Awake()
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
    
}
