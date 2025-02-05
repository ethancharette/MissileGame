using System.Collections;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float damage = 5f;
    [SerializeField] Vector3 direction = new Vector3(0, 1, 0);
    public bool explodesOnCollision;
    public float lifetimeAfterCollision = 0.5f;
    public float collisionSpeedMultiplier = 0.75f;
    [SerializeField] ParticleSystem explosionEffect;

    private float speed;

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
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void ExplodeOnCollision()
    {
        speed = moveSpeed * collisionSpeedMultiplier;
        if (explosionEffect != null) explosionEffect.Play();

        if (GetComponent<DestroyAtHeight>() == null)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            DestroyAtHeight d = GetComponent<DestroyAtHeight>();
            d.HideRocks();
        }

        StartCoroutine(ReturnToPoolAfterSeconds());
    }

    private IEnumerator ReturnToPoolAfterSeconds()
    {
        yield return new WaitForSeconds(lifetimeAfterCollision);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        PooledObject pooledObject = GetComponent<PooledObject>();
        if (pooledObject != null)
        {
            // Reset Object
            if (GetComponent<DestroyAtHeight>() == null)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;

            }
            else
            {
                DestroyAtHeight d = GetComponent<DestroyAtHeight>();
                d.ShowRocks();
            }
            speed = moveSpeed;
            pooledObject.ReturnToPool();
        }
    }
}