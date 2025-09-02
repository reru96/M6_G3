using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ReturnCubeToPool : MonoBehaviour
{
    public IObjectPool<GameObject> pool;
    public float lifeTime = 3f;

    void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void ReturnToPool()
    {
        pool.Release(gameObject);
    }
}
