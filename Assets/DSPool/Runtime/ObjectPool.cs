using System.Collections.Generic;

namespace DSPool;

public abstract class ObjectPool<TPoolElement>
    where TPoolElement : class
{
    protected readonly Stack<TPoolElement> poolElements = new();

    public void Prewarm(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            this.poolElements.Push(this.InstantiateElement());
        }
    }

    public TPoolElement Rent()
    {
        if (!this.poolElements.TryPop(out var element))
        {
            element = this.InstantiateElement();
        }

        this.OnRent(element);
        return element;
    }

    public void Return(TPoolElement element)
    {
        this.OnReturn(element);
        this.poolElements.Push(element);
    }

    protected abstract TPoolElement InstantiateElement();

    protected virtual void OnRent(TPoolElement element) { }

    protected virtual void OnReturn(TPoolElement element) { }

}
