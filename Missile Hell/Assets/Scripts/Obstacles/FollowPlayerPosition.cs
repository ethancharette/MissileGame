using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerPosition : MonoBehaviour
{
    private MissileBody player;

    private void Start()
    {
        player = GameManager.Instance.missileController.body;
    }

    private void Update()
    {
        // Follow Player position without changing the height of the object
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    }
}
