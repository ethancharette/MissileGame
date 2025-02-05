using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 40;
    private List<GameObject> pool = new List<GameObject>();

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            obj.AddComponent<PooledObject>().SetPool(this); // Store reference to the pool
            pool.Add(obj);
        }
    }

    public GameObject GetFromPool(Vector3 spawnPos)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = spawnPos;
                obj.SetActive(true);
                return obj;
            }
        }

        // Expand pool if needed
        GameObject newObj = Instantiate(prefab, spawnPos, Quaternion.identity);
        newObj.AddComponent<PooledObject>().SetPool(this);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
