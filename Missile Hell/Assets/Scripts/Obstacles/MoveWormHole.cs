using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWormHole : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    public float moveSpeed = 5f;
    private float speed;
    [SerializeField] Vector3 direction = new Vector3(0, 1, 0);
    public float lifeTime = 20f;
    [Header("Teleport")]
    public float invisibleTime = 2f;
    // Object & Script references
    [SerializeField] ParticleSystem teleportParticle;
    [SerializeField] MeshRenderer body;
    [SerializeField] GameObject destroy;

    private bool canCollide = true;
    #endregion
    private void Start()
    {
        speed = moveSpeed;
        // Start lifetime coroutine
        StartCoroutine(DestroyAfterSeconds());
    }
    private void Update()
    {
        // move game object
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // When colliding with the player, begin wormhole shenanegains
        if (canCollide && other.gameObject.CompareTag("Player"))
        {
            // can collide = false so player cant use this one again
            canCollide = false;
            // Freeze object
            speed = 0;
            // Hide gameObject components
            gameObject.GetComponent<PulseScale>().enabled = false;
            body.enabled = false;
            StartCoroutine(EffectAfterSeconds());
        }
    }
    /// <summary>
    /// Plays a particle effect twice, before and after the coroutine wait time, hiding the player during wait time as well.
    /// </summary>
    /// <returns></returns>
    private IEnumerator EffectAfterSeconds()
    {
        // Play particle
        if (teleportParticle != null)
        {
            teleportParticle.Play();
        }
        // Hide Player
        GameManager.Instance.missileController.SetVisible(false);
        yield return new WaitForSeconds(invisibleTime);
        // Show Player
        GameManager.Instance.missileController.SetVisible(true);
        // Play effect
        if (teleportParticle != null)
        {
            teleportParticle.Play();
            yield return new WaitForSeconds(teleportParticle.main.duration);
        }
        Destroy(destroy);
    }
    /// <summary>
    /// Destroys the destroy object after lifeTime
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(destroy);
    }
}
