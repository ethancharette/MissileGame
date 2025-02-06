using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private ObjectPool pool;
    /// <summary>
    /// Sets the pool reference to passed ObjectPool parameter
    /// </summary>
    /// <param name="objectPool"> my pool </param>
    public void SetPool(ObjectPool objectPool)
    {
        pool = objectPool;
    }
    /// <summary>
    /// Returns an object to their pool. If no pool exists it will Destroy the Game Object.
    /// </summary>
    public void ReturnToPool()
    {
        if (pool != null)
        {
            pool.ReturnToPool(gameObject);
        }
        else
        {
            Destroy(gameObject); // Fallback in case of an error
        }
    }
}
