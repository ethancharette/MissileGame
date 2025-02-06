using Unity.VisualScripting;
using UnityEngine;

public class DestroyAtHeight : MonoBehaviour
{
    #region Variables
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
    #endregion
    private void Start()
    {
        // store the initial position of all rocks
        rockPos = new Vector3[rocks.Length];
        for (int i = 0; i < rocks.Length; i++) rockPos[i] = rocks[i].localPosition;
    }
    void Update()
    {
        // Explode object when it is time
        if (explodes && !hasExploded && transform.position.y >= explosionHeight)
        {
            Explode();
        }
        // Return object to pool when it is time
        if (transform.position.y >= height)
        {
            ReturnToPool();
        }
    }
    /// <summary>
    /// Explodes all rocks out from the game object
    /// </summary>
    private void Explode()
    {
        // bool and Effect
        hasExploded = true;
        PlayLargeExplosion();

        foreach (Transform child in rocks)
        {
            // Get, set variables, and enable the RotateObject and MoveAsteroid components of the rock
            Vector3 dir = (child.transform.position - transform.position).normalized;
            RotateObject r = child.gameObject.GetComponent<RotateObject>();
            MoveAsteroid m = child.gameObject.GetComponent<MoveAsteroid>();

            dir.y = 1;
            r.SetSpeed(r.rotationSpeed * explosionForce);
            m.SetDirection(dir);
            m.SetSpeed(m.moveSpeed * explosionForce);

            r.enabled = true;
            m.enabled = true;
        }
        // Disable the rotation of the parent object/asteroid
        RotateObject rotateObj = gameObject.GetComponent<RotateObject>();
        if (rotateObj != null) rotateObj.enabled = false;

        transform.rotation = Quaternion.identity;
    }
    /// <summary>
    /// Plays the Large Explosion effect
    /// </summary>
    public void PlayLargeExplosion()
    {
        explosionEffect.Play();
    }
    /// <summary>
    /// Disables the MeshRenderer of all rocks in the rocks[] array
    /// </summary>
    public void HideRocks()
    {
        foreach (Transform c in rocks)
        {
            c.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    /// <summary>
    /// Enables the MeshRenderer of all rocks in the rocks[] array
    /// </summary>
    public void ShowRocks()
    {
        foreach (Transform c in rocks)
        {
            c.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    /// <summary>
    /// Resets the state of each rock object to it's initial state
    /// </summary>
    public void ResetRocks()
    {
        for (int i = 0; i < rocks.Length; i++)
        {
            rocks[i].localPosition = rockPos[i];

            rocks[i].GetComponent<MeshRenderer>().enabled = true;
            RotateObject r = rocks[i].gameObject.GetComponent<RotateObject>();
            MoveAsteroid m = rocks[i].gameObject.GetComponent<MoveAsteroid>();
            r.enabled = false;
            m.enabled = false;
        }
    }
    /// <summary>
    /// Returns the game object to its object pool if it exists
    /// </summary>
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
