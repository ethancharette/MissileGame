using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerPosition : MonoBehaviour
{
    MissileBody player;

    private void Start()
    {
        player = GameManager.Instance.missileController.body;
    }

    private void Update()
    {
        // Follow Player
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
    }
}
