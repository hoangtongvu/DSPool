using UnityEngine;

namespace DSPool;

[DSPoolUsePrefab(typeof(GameObject))]
public partial class MonoPool<TPoolElement> : ObjectPool<TPoolElement>
    where TPoolElement : MonoBehaviour
{
    protected override TPoolElement InstantiateElement()
    {
        return Object.Instantiate(this.Prefab).GetComponent<TPoolElement>();
    }

    protected override void OnReturn(TPoolElement element)
    {
        base.OnReturn(element);
        element.gameObject.SetActive(false);
    }

}
