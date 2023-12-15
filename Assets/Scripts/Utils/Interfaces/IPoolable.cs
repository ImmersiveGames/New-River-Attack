namespace Utils
{
    public interface IPoolable
    {
    }
    public interface IHasPool
    {
        void StartMyPool(int quantity, bool isPersistent = false);
    }
}
