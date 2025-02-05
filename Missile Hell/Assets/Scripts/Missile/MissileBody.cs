using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissileBody : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 1.5f;
    public float tiltAngle = 30f;
    public float spinSpeed = 100f;
    private Vector3 spinspeed;

    [SerializeField] GameObject missileModel;
    GameManager gm;

    private void Start()
    {
        // Initialize internal spin vector
        spinspeed = new Vector3(0,spinSpeed,0);

        gm = GameManager.Instance;
    }

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
            // had to get fucky here cause quaternions are weird...
            targetRotation = new Quaternion(-rot.z * tiltAngle, rot.y, rot.x * tiltAngle, rot.w);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Spin the missle model
        missileModel.transform.Rotate(spinspeed * Time.deltaTime);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            gm.updatePlayerHealth(gm.asteroidDamage);
            other.GetComponent<MoveObject>().ExplodeOnCollision();
        }
    }
}
