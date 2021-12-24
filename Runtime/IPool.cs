namespace ObjectPool
{
    public interface IPool
    {
        void ReturnToPool(PoolObject poolObject);
    }
}