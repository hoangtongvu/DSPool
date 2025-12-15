using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DSPool;

[DSPoolUsePrefab(typeof(GameObject))]
public partial class AsyncGameObjectPool : AsyncObjectPool<GameObject>
{
    protected override UniTask<GameObject> InstantiateElementAsync()
    {
        return UniTask.FromResult(Object.Instantiate(this.Prefab));
    }

    protected override void OnPrewarm(GameObject element)
    {
        base.OnPrewarm(element);
        element.SetActive(false);
    }

    protected override void OnReturn(GameObject element)
    {
        base.OnReturn(element);
        element.SetActive(false);
    }
}