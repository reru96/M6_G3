using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolExample : MonoBehaviour
{
    public Material defaultParticleMaterial;

    public enum PoolType
    {
        Stack,
        LinkedList
    }

    public PoolType poolType;

    public bool collectionChecks = true;
    public int maxPoolSize = 10;

    IObjectPool<ParticleSystem> m_Pool;

    public IObjectPool<ParticleSystem> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                if (poolType == PoolType.Stack)
                {
                    m_Pool = new ObjectPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
                }
                else
                    m_Pool = new LinkedPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
            }
            return m_Pool;
        }
    }

    ParticleSystem CreatePooledItem()
    {
        var go = new GameObject("Pooled Particle System");
        var ps = go.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.duration = 1f;
        main.startLifetime = 1f;
        main.loop = false;

        ps.GetComponent<ParticleSystemRenderer>().material = defaultParticleMaterial;
        var returnToPool = go.AddComponent<ReturnToPool>();
        returnToPool.pool = Pool;

        return ps;
    }
    void OnTakeFromPool(ParticleSystem system)
    {
        system.gameObject.SetActive(true);
    }   
    void OnReturnedToPool(ParticleSystem system)
    {
        system.gameObject.SetActive(false);
    }

    void OnDestroyPoolObject(ParticleSystem system) 
    {
        Destroy(system.gameObject);
    }

    void OnGUI()
    {
        GUILayout.Label("Pool size: " + Pool.CountInactive);
        if(GUILayout.Button("Create Particles"))
        {
            var amount = Random.Range(1, 10);
            for(int i = 0; i < amount; i++)
            {
                var ps = Pool.Get();
                ps.transform.position = Random.insideUnitSphere * 10;
                ps.Play();
            }
        }
    }
   
}
