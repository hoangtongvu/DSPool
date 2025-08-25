using System;
using System.Collections.Generic;

namespace DSPool;

public abstract class ObjectPool<TPoolElement> : IDisposable
    where TPoolElement : class
{
    private bool isDisposed;
    protected readonly Stack<TPoolElement> poolElements = new();

    public int Count => this.poolElements.Count;
    public bool IsDisposed => this.isDisposed;

    public void Prewarm(int amount)
    {
        this.ThrowIfDisposed();

        for (int i = 0; i < amount; i++)
        {
            var element = this.InstantiateElement();
            this.OnPrewarm(element);
            this.poolElements.Push(element);
        }
    }

    public TPoolElement Rent()
    {
        this.ThrowIfDisposed();

        if (!this.poolElements.TryPop(out var element))
        {
            element = this.InstantiateElement();
        }

        this.OnRent(element);
        return element;
    }

    public void Return(TPoolElement element)
    {
        this.ThrowIfDisposed();
        this.OnReturn(element);
        this.poolElements.Push(element);
    }

    public void Clear()
    {
        this.ThrowIfDisposed();
        this.poolElements.Clear();
    }

    public void Dispose()
    {
        this.ThrowIfDisposed();
        this.isDisposed = true;
        this.Clear();
    }

    protected abstract TPoolElement InstantiateElement();

    protected virtual void OnPrewarm(TPoolElement element) { }

    protected virtual void OnRent(TPoolElement element) { }

    protected virtual void OnReturn(TPoolElement element) { }

    private void ThrowIfDisposed()
    {
        if (this.isDisposed) throw new ObjectDisposedException(this.GetType().Name);
    }

}
