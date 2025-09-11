using UnityEngine;

namespace DSPool;

[DSPoolSingleton]
public partial class SharedGameObjectPoolMap : PoolMap<GameObject, GameObjectPool, GameObject>
{
    protected override GameObjectPool GetPoolByKey(GameObject poolKey)
    {
        if (!this.Pools.TryGetValue(poolKey, out var pool))
        {
            this.CreateNewPool(poolKey, out pool);
        }

        return pool;
    }

    private void CreateNewPool(GameObject prefab, out GameObjectPool newPool)
    {
        newPool = new() { Prefab = prefab };
    }
}
