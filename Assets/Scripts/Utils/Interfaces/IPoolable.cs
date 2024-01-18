using UnityEngine;
namespace Utils
{
    public interface IPoolable
    {
    }
    public interface IHasPool
    {
        void StartMyPool(GameObject bullets, int quantity, bool isPersistent = false);
    }
}
