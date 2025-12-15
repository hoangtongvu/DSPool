using UnityEngine;

namespace DSPool;

[DSPoolSharedInstance]
public class GameObjectPoolMap : PoolMap<GameObject, GameObjectPool, GameObject>
{
    protected override GameObjectPool GetPoolByKey(GameObject poolKey)
    {
        if (!this.Pools.TryGetValue(poolKey, out var pool))
        {
            this.CreateNewPool(poolKey, out pool);
            this.Pools.Add(poolKey, pool);
        }

        return pool;
    }

    private void CreateNewPool(GameObject prefab, out GameObjectPool newPool)
    {
        newPool = new() { Prefab = prefab };
    }
}