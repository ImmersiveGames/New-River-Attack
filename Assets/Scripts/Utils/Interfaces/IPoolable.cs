namespace Utils
{
    public interface IPoolable
    {
    }
    public interface IHasPool
    {
        void StartMyPool(bool isPersistent = false);
    }
}
