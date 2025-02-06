using System.Collections;
using UnityEngine;

public class MoveAsteroid : MonoBehaviour
{
    #region Variables
    public float moveSpeed = 5f;
    public float damage = 5f;
    [SerializeField] Vector3 direction = new Vector3(0, 1, 0);
    public bool explodesOnCollision;
    public float lifetimeAfterCollision = 0.5f;
    public float collisionSpeedMultiplier = 0.75f;
    [SerializeField] ParticleSystem explosionEffect;

    private float speed;
    #endregion
    #region Setters
    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    #endregion

    private void Start()
    {
        speed = moveSpeed;
    }
    private void Update()
    {
        // move object
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
    /// <summary>
    /// Makes the object explode when collided with
    /// </summary>
    public void ExplodeOnCollision()
    {
        speed = moveSpeed * collisionSpeedMultiplier;
        if (explosionEffect != null) explosionEffect.Play();

        // Hide MeshRenderer if this is a rock (child of asteroid)
        if (GetComponent<DestroyAtHeight>() == null)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        // Otherwise hide all the rocks of this object
        else
        {
            DestroyAtHeight d = GetComponent<DestroyAtHeight>();
            d.HideRocks();
        }
        // Return object to object pool
        StartCoroutine(ReturnToPoolAfterSeconds());
    }
    /// <summary>
    /// Returns the Game Object to it's pool after a collision
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReturnToPoolAfterSeconds()
    {
        yield return new WaitForSeconds(lifetimeAfterCollision);
        ReturnToPool();
    }
    /// <summary>
    /// Returns the Game Object to it's pool
    /// </summary>
    private void ReturnToPool()
    {
        PooledObject pooledObject = GetComponent<PooledObject>();
        if (pooledObject != null)
        {
            // Reset Object
            // If object is a rock (child of asteroid), hide MeshRenderer
            if (GetComponent<DestroyAtHeight>() == null)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;

            }
            // Otherwise show all rocks
            else
            {
                DestroyAtHeight d = GetComponent<DestroyAtHeight>();
                d.ShowRocks();
            }
            // Reset move speed and return object to pool.
            speed = moveSpeed;
            pooledObject.ReturnToPool();
        }
    }
}