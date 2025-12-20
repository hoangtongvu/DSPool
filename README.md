# DSPool

![Unity](https://img.shields.io/badge/Unity-2021?logo=unity)
![License](https://img.shields.io/github/license/hoangtongvu/com.darksun.enum-length-generator)

## Overview

**DSPool** is an object pooling solution for Unity. It provides various classes for object pooling, such as generic [ObjectPool](Assets/DSPool/Runtime/ObjectPool.cs) for general use case or [ComponentPool](Assets/DSPool/Runtime/ComponentPool.cs) for `Component` pooling with a `GameObject` as the prefab. Additionally, you can create various kinds of pools with ready-to-use attributes.

## Installation

To install, paste the following URL into Unity's **Package Manager**:

1. Open **Package Manager**.
2. Click the **+** button.
3. Select **"Add package from git URL..."**.
4. Enter:

```bash
https://github.com/hoangtongvu/DSPool.git?path=/Assets/DSPool#1.1.1
```

## Basic usage

`ComponentPool` Example:

```cs
public class Example : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private ComponentPool<MeshRenderer> meshRendererPool;

    private void Awake()
    {
        this.meshRendererPool = new() { Prefab = this.prefab };
    }

    void Start()
    {
        this.meshRendererPool.Prewarm(10); // Pre-warm objects in advance
        var instance = this.meshRendererPool.Rent(); // Rent an object from the pool
        this.meshRendererPool.Return(instance); // Return the object back to the pool after use
    }
}
```

Regular class pool Example:

```cs
public class Foo { }

public class FooPool : ObjectPool<Foo>
{
    protected override Foo InstantiateElement() => new Foo();

    protected override void OnRent(Foo element) { }

    protected override void OnReturn(Foo element) { }
}

var pool = new FooPool();
pool.Prewarm(10);

var instance = pool.Rent();
pool.Return(instance);

int count = pool.Count; // Get the number of unused objects in the pool
pool.Clear(); // Clear all objects in the pool
pool.Dispose(); // Dispose the pool
```

## Create custom Pools

You can create your custom pool by inheriting from [`ObjectPool<T>`](Assets/DSPool/Runtime/ObjectPool.cs).

```cs
public class Foo { }

public class FooPool : ObjectPool<Foo>
{
    protected override Foo InstantiateElement() => new Foo();

    protected override void OnRent(Foo element)
    {
        // Add operations before the element being rented
    }

    protected override void OnReturn(Foo element)
    {
        // Add operations before the element returned to the pool
    }
}
```

Additionally, you can use ready-to-use attributes to custom your pool even further:
- `[DSPoolUsePrefab(typeof(T))]`: Generate a Prefab property of type `T` for the pool
- `[DSPoolSingleton]`: Generate a lazy-initialized singleton for the pool, access via `{PoolName}.Instance`
- `[DSPoolManualSingleton]`: Generate a manual-initialized singleton for the pool, suitable for pools that cannot be initialized via `new()` *(require prefab or any additional fields)*
- `[DSPoolSharedInstance]`: Generate a singleton wrapper for the marked pool, can access directly via `Shared{PoolName}`

**Note**:
- `[DSPoolSingleton]` and `[DSPoolManualSingleton]` can't be put together on the same pool as they serve the same singleton purpose

```cs
// Auto-genertated from [DSPoolSingleton]
public partial class FooPool
{
    private static FooPool instance;
    public static FooPool Instance => instance ??= new();

    public static void DestroyInstance() => instance = null;

#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void ClearOnLoad() => DestroyInstance();
#endif
}
```

```cs
// Auto-genertated from [DSPoolManualSingleton]
public partial class FooPool
{
    private static bool isInstanceInitialized;
    private static FooPool instance;
    public static FooPool Instance => instance ?? throw new NullReferenceException("FooPool_Singleton hasn't been initialized");

    public static bool IsInstanceInitialized => isInstanceInitialized;

    public static void InitializeInstance(FooPool newInstance)
    {
        if (newInstance == null) throw new NullReferenceException("Can not initialize null instance for FooPool_Singleton");
        isInstanceInitialized = true;
        instance = newInstance;
    }

    public static void DestroyInstance()
    {
        isInstanceInitialized = false;
        instance = null;
    }

#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void ClearOnLoad() => DestroyInstance();
#endif
}
```

## Pool map

**PoolMap<TPoolKey, TPool, TPoolElement>** is a dictionary with the key is `TPoolKey` and value is `TPool`

```cs
public enum UIType { HealthBar = 0, ToolBar = 1 }

public class UICtrl : MonoBehaviour { }

var poolMap = new PoolMap<UIType, ComponentPool<UICtrl>, UICtrl>();

// Add pools to the poolMap
poolMap.Pools.Add(UIType.HealthBar, new()
{
    Prefab = someHealthBarPrefab,
});

// Can rent and return objects to the poolMap just like a normal pool
var instance = poolMap.Rent(UIType.HealthBar);
poolMap.Return(instance);
```

Additionally, you can create your own `PoolMap` and add `[DSPoolSharedInstance]` on it:

```cs
[DSPoolSharedInstance]
public class UICtrlPoolMap : PoolMap<UIType, ComponentPool<UICtrl>, UICtrl> { }
```

## License

[MIT License](LICENSE)