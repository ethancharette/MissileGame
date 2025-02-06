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
    /// <summary>
    /// Initializes the object pool
    /// </summary>
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            obj.AddComponent<PooledObject>().SetPool(this); // Store reference to the pool in the pooled object
            pool.Add(obj);
        }
    }
    /// <summary>
    /// Returns a Game Object from the object pool. If no object is available, it will expand the object pool and return a new object in the pool.
    /// </summary>
    /// <param name="spawnPos"></param>
    /// <returns></returns>
    public GameObject GetFromPool(Vector3 spawnPos)
    {
        foreach (GameObject obj in pool)
        {
            // Find active pooled object, set its position back to its initial pos and return the object
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
    /// <summary>
    /// Returns an object to the pool by setting its activity to false
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
    /// <summary>
    /// Returns every pooled object to the pool
    /// </summary>
    public void ResetPool()
    {
        foreach (GameObject obj in pool)
        {
            ReturnToPool(obj);
        }
    }
}
