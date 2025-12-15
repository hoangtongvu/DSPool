using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DSPool;

[DSPoolUsePrefab(typeof(GameObject))]
public partial class AsyncComponentPool<TPoolElement> : AsyncObjectPool<TPoolElement>
    where TPoolElement : Component
{
    protected override UniTask<TPoolElement> InstantiateElementAsync()
    {
        return UniTask.FromResult(Object.Instantiate(this.Prefab).GetComponent<TPoolElement>());
    }

    protected override void OnPrewarm(TPoolElement element)
    {
        base.OnPrewarm(element);
        element.gameObject.SetActive(false);
    }

    protected override void OnReturn(TPoolElement element)
    {
        base.OnReturn(element);
        element.gameObject.SetActive(false);
    }
}