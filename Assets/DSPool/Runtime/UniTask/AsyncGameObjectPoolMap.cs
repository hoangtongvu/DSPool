using UnityEngine;

namespace DSPool;

[DSPoolSharedInstance]
public class AsyncGameObjectPoolMap : AsyncPoolMap<GameObject, AsyncGameObjectPool, GameObject>
{
    protected override AsyncGameObjectPool GetPoolByKey(GameObject poolKey)
    {
        if (!this.Pools.TryGetValue(poolKey, out var pool))
        {
            this.CreateNewPool(poolKey, out pool);
            this.Pools.Add(poolKey, pool);
        }

        return pool;
    }

    private void CreateNewPool(GameObject prefab, out AsyncGameObjectPool newPool)
    {
        newPool = new() { Prefab = prefab };
    }
}