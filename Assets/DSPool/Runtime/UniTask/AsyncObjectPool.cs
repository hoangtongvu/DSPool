using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace DSPool;

public abstract class AsyncObjectPool<TPoolElement>
    where TPoolElement : class
{
    private const byte prewarmBatchSize = 64;
    protected readonly Stack<TPoolElement> poolElements = new();

    public int Count => this.poolElements.Count;

    public async UniTask PrewarmAsync(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var element = await this.InstantiateElementAsync();
            this.OnPrewarm(element);
            this.poolElements.Push(element);

            if (i % prewarmBatchSize == 0)
                await UniTask.Yield();
        }
    }

    public UniTask<TPoolElement> RentAsync()
    {
        if (!this.poolElements.TryPop(out var element))
        {
            return this.InstantiateElementAsync();
        }

        this.OnRent(element);
        return UniTask.FromResult(element);
    }

    public void Return(TPoolElement element)
    {
        this.OnReturn(element);
        this.poolElements.Push(element);
    }

    public void Clear()
    {
        this.poolElements.Clear();
    }

    protected abstract UniTask<TPoolElement> InstantiateElementAsync();

    protected virtual void OnPrewarm(TPoolElement element) { }

    protected virtual void OnRent(TPoolElement element) { }

    protected virtual void OnReturn(TPoolElement element) { }
}