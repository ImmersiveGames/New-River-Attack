using UnityEngine;
namespace Utils
{
    public interface IPoolablex
    {
    }
    public interface IHasPool
    {
        void StartMyPool(GameObject bullets, int quantity, bool isPersistent = false);
    }
}
