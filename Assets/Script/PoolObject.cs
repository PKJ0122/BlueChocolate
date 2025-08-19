using UnityEngine;
using UnityEngine.Pool;

public class PoolObject : MonoBehaviour
{
    IObjectPool<PoolObject> _pool;

    public void SetPool(IObjectPool<PoolObject> pool)
    {
        _pool = pool;
    }

    public void Release()
    {
        _pool.Release(this);
    }
}
