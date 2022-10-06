using System;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager<T> where T : class, IIsPoolObject<T>
{
    private IObjectPool<T> pool;
    private GameObject prefab;

    public ObjectPoolManager(int count, GameObject prefab, Action<T> setupObject)
    {
        this.prefab = prefab;
        pool = new ObjectPool<T>(InstantiateObject, setupObject, defaultCapacity: count);
    }

    public T Get() => pool.Get();

    private T InstantiateObject()
    {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.SetActive(false);
        T t = obj.GetComponent<T>();
        t.SetPool(pool);

        return t;
    }
}
