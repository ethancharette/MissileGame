using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    #region Variables
    public MissileBody body;
    [HideInInspector]public Animator animator;

    private bool canMove = true;
    private bool isVisible = true;

    private float height = 100f;
    private Vector2 direction;
    #endregion

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // If applicable, get movement inputs and pass them into the player body
        if (canMove && isVisible)
        {
            direction.x = Input.GetAxis("Horizontal");
            direction.y = Input.GetAxis("Vertical");

            body.UpdatePosition(direction);
        }
    }
    /// <summary>
    /// Hides the player Game Object for Player Death
    /// </summary>
    public void PlayerDeath()
    {
        canMove = false;
        SetVisible(false);
    }
    /// <summary>
    /// Resets the player to it's initial position and rotation. Enables movement and visibility
    /// </summary>
    public void ResetPlayer()
    {
        transform.position = new Vector3(0f, height, 0f);
        body.transform.localPosition = Vector3.zero;
        body.transform.localRotation = Quaternion.identity;
        canMove = true;
        SetVisible(true);
    }
    /// <summary>
    /// Sets player body visibility based on passed boolean parameter
    /// </summary>
    /// <param name="visible"></param>
    public void SetVisible(bool visible)
    {
        isVisible = visible;
        body.SetVisible(isVisible);
    }
}
