using System.Collections.Generic;

namespace DSPool;

public abstract class PoolMap<TPoolKey, TPool, TPoolElement>
    where TPoolElement : class
    where TPool : ObjectPool<TPoolElement>
{
    public readonly Dictionary<TPoolKey, TPool> poolMap = new();
    private readonly Dictionary<TPoolElement, TPoolKey> rentedMap = new();

    public TPoolElement Rent(TPoolKey poolKey)
    {
        var pool = this.poolMap[poolKey];
        var rentedElement = pool.Rent();
        this.rentedMap.TryAdd(rentedElement, poolKey);

        this.OnRent(poolKey, rentedElement);
        return rentedElement;
    }

    public void Return(TPoolElement poolElement)
    {
        if (!this.rentedMap.TryGetValue(poolElement, out var poolKey))
            throw new System.Exception("Only can return rented instance from this pool");

        this.OnReturn(poolKey, poolElement);
        poolMap[poolKey].Return(poolElement);
    }

    protected virtual void OnRent(TPoolKey poolKey, TPoolElement poolElement) { }

    protected virtual void OnReturn(TPoolKey poolKey, TPoolElement poolElement) { }

}
