using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissileBody : MonoBehaviour
{
    #region Variables
    public float moveSpeed = 5f;
    public float rotationSpeed = 1.5f;
    public float tiltAngle = 30f;
    public float spinSpeed = 100f;
    private Vector3 spinspeed;
    // Object references
    [SerializeField] GameObject missileModel;
    GameManager gm;
    #endregion
    private void Start()
    {
        // Initialize internal spin vector
        spinspeed = new Vector3(0,spinSpeed,0);

        gm = GameManager.Instance;
    }
    /// <summary>
    /// Adjusts the body's transform to Translate in the provided direction input
    /// </summary>
    /// <param name="direction"> direction to move </param>
    public void UpdatePosition(Vector2 direction)
    {
        // Set move direction
        Vector3 move = new Vector3(direction.x, 0, direction.y).normalized;

        // Move missile
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

        // Rotate missile
        Quaternion targetRotation = Quaternion.identity;
        // if missile is moving
        if (move.magnitude > 0)
        {
            // Rotate the missle
            Quaternion rot = Quaternion.Euler(move);
            // had to get freaky here cause quaternions are weird...
            targetRotation = new Quaternion(-rot.z * tiltAngle, rot.y, rot.x * tiltAngle, rot.w);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Spin the missle model
        missileModel.transform.Rotate(spinspeed * Time.deltaTime);
    }
    /// <summary>
    /// Sets the visibility of the body based on passed boolean parameter
    /// </summary>
    /// <param name="visible"></param>
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    private void OnTriggerEnter(Collider other)
    {
        // When colliding with an asteroid
        if (other.CompareTag("Asteroid"))
        {
            // Damage the player based on asteroid damage and explode the asteroid
            MoveAsteroid m = other.GetComponent<MoveAsteroid>();
            gm.updatePlayerHealth(m.damage);
            m.ExplodeOnCollision();
        }
    }
}
