using UnityEngine;

namespace DSPool;

[DSPoolUsePrefab(typeof(GameObject))]
public partial class GameObjectPool : ObjectPool<GameObject>
{
    protected override GameObject InstantiateElement()
    {
        return Object.Instantiate(this.Prefab);
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
