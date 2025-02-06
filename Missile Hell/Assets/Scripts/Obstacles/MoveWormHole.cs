using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWormHole : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private float speed;
    [SerializeField] Vector3 direction = new Vector3(0, 1, 0);
    public float lifeTime = 20f;
    [Header("Teleport")]
    public float invisibleTime = 2f;
    [SerializeField] ParticleSystem teleportParticle;
    [SerializeField] MeshRenderer body;
    [SerializeField] GameObject destroy;

    private bool canCollide = true;

    private void Start()
    {
        speed = moveSpeed;
        StartCoroutine(DestroyAfterSeconds());
    }
    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
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

    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(destroy);
    }
}
