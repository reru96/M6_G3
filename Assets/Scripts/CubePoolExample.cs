using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubePoolExample : MonoBehaviour
{
    public Material cubeMaterial;
    public float cubeLifeTime = 3f;

    public enum PoolTypeSq
    {
        StackSq,
        LinkedListSq
    }

    public PoolTypeSq poolType;

    public bool collectionChecks = true;
    public int maxPoolSize = 20;

    IObjectPool<GameObject> sq_Pool;

    public IObjectPool<GameObject> Pool
    {
        get
        {
            if (sq_Pool == null)
            {
                if (poolType == PoolTypeSq.StackSq)
                {
                    sq_Pool = new ObjectPool<GameObject>(
                        CreatePooledItem,
                        OnTakeFromPool,
                        OnReturnedToPool,
                        OnDestroyPoolObject,
                        collectionChecks,
                        10,
                        maxPoolSize
                    );
                }
                else
                {
                    sq_Pool = new LinkedPool<GameObject>(
                        CreatePooledItem,
                        OnTakeFromPool,
                        OnReturnedToPool,
                        OnDestroyPoolObject,
                        collectionChecks,
                        maxPoolSize
                    );
                }
            }
            return sq_Pool;
        }
    }


    GameObject CreatePooledItem()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.GetComponent<Renderer>().material = cubeMaterial;

        var returnToPool = cube.AddComponent<ReturnCubeToPool>();
        returnToPool.pool = Pool;
        returnToPool.lifeTime = cubeLifeTime;

        cube.SetActive(false);
        return cube;
    }

    
    void OnTakeFromPool(GameObject cube)
    {
        cube.SetActive(true);
    }

   
    void OnReturnedToPool(GameObject cube)
    {
        cube.SetActive(false);
    }

    void OnDestroyPoolObject(GameObject cube)
    {
        Destroy(cube);
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 100, 200, 200));
        GUILayout.Label("Pool size: " + Pool.CountInactive);

        if (GUILayout.Button("Create Cubes"))
        {
            var amount = Random.Range(1, 10);
            for (int i = 0; i < amount; i++)
            {
                var cube = Pool.Get();
                cube.transform.position = Random.insideUnitSphere * 10f;
            }
        }
        GUILayout.EndArea();
    }
}

