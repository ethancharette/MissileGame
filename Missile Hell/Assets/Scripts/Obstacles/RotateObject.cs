using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    #region Variables
    [SerializeField] float rotationSpeedMin = 100f;
    [SerializeField] float rotationSpeedMax = 300f;
    [HideInInspector] public float rotationSpeed;
    private Vector3 rotation;
    #endregion
    #region Setters
    public void SetSpeedRange(float min, float max)
    {
        rotationSpeed = Random.Range(min, max);
    }

    public void SetSpeed(float speed)
    {
        rotationSpeed = speed;
    }
    #endregion
    private void Start()
    {
        // Initialize random rotation and speed
        rotation = Random.onUnitSphere;
        rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
    }
    void Update()
    {
        // Rotate the object
        transform.Rotate(rotation * rotationSpeed * Time.deltaTime);
    }

    
}
