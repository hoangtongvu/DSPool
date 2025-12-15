using System.Collections.Generic;

namespace DSPool;

public class PoolMap<TPoolKey, TPool, TPoolElement>
    where TPoolElement : class
    where TPool : ObjectPool<TPoolElement>
{
    private readonly Dictionary<TPoolElement, TPoolKey> rentedToPoolKeyMap = new();
    public readonly Dictionary<TPoolKey, TPool> Pools = new();

    public TPoolElement Rent(TPoolKey poolKey)
    {
        var pool = this.GetPoolByKey(poolKey);
        var rentedElement = pool.Rent();
        this.rentedToPoolKeyMap.TryAdd(rentedElement, poolKey);

        this.OnRent(poolKey, pool, rentedElement);
        return rentedElement;
    }

    public void Return(TPoolElement poolElement)
    {
        if (!this.rentedToPoolKeyMap.TryGetValue(poolElement, out var poolKey))
            throw new System.Exception("Only can return rented instance from this pool");

        var pool = this.Pools[poolKey];
        this.OnReturn(poolKey, pool, poolElement);
        pool.Return(poolElement);
    }

    protected virtual TPool GetPoolByKey(TPoolKey poolKey) => this.Pools[poolKey];

    protected virtual void OnRent(TPoolKey poolKey, TPool retrievedPool, TPoolElement poolElement) { }

    protected virtual void OnReturn(TPoolKey poolKey, TPool retrievedPool, TPoolElement poolElement) { }

}
