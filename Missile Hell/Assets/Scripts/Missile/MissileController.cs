using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public MissileBody body;
    [HideInInspector]public Animator animator;

    private bool canMove = true;

    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            direction.x = Input.GetAxis("Horizontal");
            direction.y = Input.GetAxis("Vertical");

            body.UpdatePosition(direction);
        }
    }

    public void PlayerDeath()
    {
        canMove = false;
    }

    public void ResetPlayer()
    {
        canMove = true;
    }
}
