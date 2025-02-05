using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public MissileBody body;
    [HideInInspector]public Animator animator;

    private bool canMove = true;
    private bool isVisible = true;

    private float height = 100f;
    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && isVisible)
        {
            direction.x = Input.GetAxis("Horizontal");
            direction.y = Input.GetAxis("Vertical");

            body.UpdatePosition(direction);
        }
    }

    public void PlayerDeath()
    {
        canMove = false;
        SetVisible(false);
    }

    public void ResetPlayer()
    {
        transform.position = new Vector3(0f, height, 0f);
        body.transform.localPosition = Vector3.zero;
        body.transform.localRotation = Quaternion.identity;
        canMove = true;
        SetVisible(true);
    }

    public void SetVisible(bool visible)
    {
        isVisible = visible;
        body.SetVisible(isVisible);
    }
}
