using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolObject<T> : MonoBehaviour, IIsPoolObject<T>
    where T : class
{
    protected IObjectPool<T> pool;

    public abstract void ResetToPool();

    public void SetPool(IObjectPool<T> pool)
    {
        this.pool = pool;
    }
}
