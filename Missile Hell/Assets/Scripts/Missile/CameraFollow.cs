using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Variables
    [SerializeField] Transform target;
    public Vector3 offset = new Vector3(0, 10, 0);
    public float smoothTime = 0.2f;
    public Vector2 deadzone = new Vector2(2f, 2f);

    private Quaternion fixedRotation;
    private Vector3 velocity = Vector3.zero;
    #endregion
    void Start()
    {
        // Set initial position and camera point
        transform.position = target.position + offset;
        transform.LookAt(target.position);
        // Lock camera rotation so it isn't so disorienting to play
        fixedRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;
        Vector3 currentPosition = transform.position;
        Vector3 delta = targetPosition - currentPosition;

        // Deadzone logic
        if (Mathf.Abs(delta.x) > deadzone.x || Mathf.Abs(delta.z) > deadzone.y)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }

        // Maintain fixed rotation
        transform.rotation = fixedRotation;
    }
}