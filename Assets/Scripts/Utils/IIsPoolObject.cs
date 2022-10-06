using UnityEngine.Pool;

public interface IIsPoolObject<T> where T : class
{
    void SetPool(IObjectPool<T> pool);

    void ResetToPool();
}
