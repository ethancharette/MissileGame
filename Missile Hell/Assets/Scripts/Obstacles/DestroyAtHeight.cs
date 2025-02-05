using Unity.VisualScripting;
using UnityEngine;

public class DestroyAtHeight : MonoBehaviour
{
    [Header("Asteroid Variables")]
    public float height = 10f;
    [Header("Explosion Variables")]
    public Transform[] rocks;
    private Vector3[] rockPos;
    public bool explodes = false;
    public float explosionHeight;
    public float explosionForce = 3f;
    [Header("Particle Effects")]
    [SerializeField] ParticleSystem explosionEffect;

    private bool hasExploded = false;

    private void Start()
    {
        rockPos = new Vector3[rocks.Length];
        for (int i = 0; i < rocks.Length; i++) rockPos[i] = rocks[i].localPosition;
    }
    void Update()
    {
        if (explodes && !hasExploded && transform.position.y >= explosionHeight)
        {
            Explode();
        }

        if (transform.position.y >= height)
        {
            ReturnToPool();
        }
    }

    private void Explode()
    {
        hasExploded = true;
        PlayLargeExplosion();

        foreach (Transform child in rocks)
        {
            Vector3 dir = (child.transform.position - transform.position).normalized;
            RotateObject r = child.gameObject.GetComponent<RotateObject>();
            MoveObject m = child.gameObject.GetComponent<MoveObject>();

            dir.y = 1;
            r.SetSpeed(r.rotationSpeed * explosionForce);
            m.SetDirection(dir);
            m.SetSpeed(m.moveSpeed * explosionForce);

            r.enabled = true;
            m.enabled = true;
        }

        RotateObject rotateObj = gameObject.GetComponent<RotateObject>();
        if (rotateObj != null) rotateObj.enabled = false;

        transform.rotation = Quaternion.identity;
    }

    public void PlayLargeExplosion()
    {
        explosionEffect.Play();
    }

    public void HideRocks()
    {
        foreach (Transform c in rocks)
        {
            c.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    public void ShowRocks()
    {
        foreach (Transform c in rocks)
        {
            c.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void ResetRocks()
    {
        for (int i = 0; i < rocks.Length; i++)
        {
            rocks[i].localPosition = rockPos[i];

            rocks[i].GetComponent<MeshRenderer>().enabled = true;
            RotateObject r = rocks[i].gameObject.GetComponent<RotateObject>();
            MoveObject m = rocks[i].gameObject.GetComponent<MoveObject>();
            r.enabled = false;
            m.enabled = false;
        }
    }

    private void ReturnToPool()
    {
        PooledObject pooledObject = GetComponent<PooledObject>();
        if (pooledObject != null)
        {
            // Reset Object
            if (explodes)
            {
                hasExploded = false;

                ResetRocks();
            }
            pooledObject.ReturnToPool();
        }
    }
}
