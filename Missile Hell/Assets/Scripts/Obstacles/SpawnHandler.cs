using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    public bool on = true;
    [Header("Object Pools")]
    [SerializeField] ObjectPool asteroidPool;
    [SerializeField] ObjectPool asteroidExplosivePool;
    [SerializeField, Range(0,100)] float explosiveChance;

    [Header("Spawn Variables")]
    [SerializeField] float spawnRange = 25f;
    [SerializeField] float spawnRate = 10f;
    private float cooldown;

    private MissileBody player;


    private void Start()
    {
        player = GameManager.Instance.missileController.body;
    }

    private void Update()
    {
        if (on)
        {
            // Spawn cooldown
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                SpawnObject();
                cooldown = spawnRate;
            }

            // Follow Player
            transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        }
    }

    private void SpawnObject()
    {
        // Pull from chance pool
        ObjectPool pool = Random.Range(0f, 100f) < explosiveChance ? asteroidExplosivePool : asteroidPool;
        // Get Object

        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnRange, spawnRange),
            0f,
            Random.Range(-spawnRange, spawnRange)
            );

        Vector3 spawnPosition = transform.position + randomOffset;

        // Get and set spawn point
        GameObject asteroid = pool.GetFromPool(spawnPosition);
    }

    public void Reset()
    {
        cooldown = spawnRate;
        // Empty Object Pools
    }

}
