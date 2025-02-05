using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    public bool on = true;
    [Header("Hazards and Object Pools")]
    [SerializeField] ObjectPool asteroidPool;
    [SerializeField] ObjectPool asteroidExplosivePool;
    [SerializeField, Range(0,100)] float explosiveChance;

    [Header("Interactables")]
    [SerializeField] GameObject interactableContainer;
    [SerializeField] GameObject wormHole;
    [SerializeField, Range(0, 100)] float wormholeChance;

    [Header("Spawn Variables")]
    [SerializeField] float spawnRange = 25f;
    [SerializeField] float hazardSpawnRate = 10f;
    [SerializeField] float interactableSpawnRate = 25f;
    private float cooldown;

    private MissileBody player;


    private void Start()
    {
        player = GameManager.Instance.missileController.body;
        InvokeRepeating("SpawnInteractable", interactableSpawnRate, interactableSpawnRate);
    }

    private void Update()
    {
        if (on)
        {
            // Spawn cooldown
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                SpawnHazard();
                cooldown = hazardSpawnRate;
            }

            // Follow Player
            transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        }
    }

    #region Spawning
    private void SpawnHazard()
    {
        // Pull from chance pool
        ObjectPool pool = Random.Range(0f, 100f) < explosiveChance ? asteroidExplosivePool : asteroidPool;
        // Get Object

        Vector3 randomOffset = RandomInRange();

        Vector3 spawnPosition = transform.position + randomOffset;

        // Get and set spawn point
        GameObject asteroid = pool.GetFromPool(spawnPosition);
    }
    private void SpawnInteractable()
    {
        // Try to spawn an interactable obj
        if (Random.Range(0f, 100f) <= wormholeChance)
        {
            GameManager.Instance.HUD.SetAlert("ANOMOLY");
            GameObject obj = Instantiate(wormHole, RandomInRange(), Quaternion.identity);
            obj.transform.SetParent(interactableContainer.transform);
        }

    }
    #endregion
    public void Reset()
    {
        cooldown = hazardSpawnRate;
    }

    private Vector3 RandomInRange()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnRange, spawnRange),
            0f,
            Random.Range(-spawnRange, spawnRange)
            );
        return randomOffset;
    }

}
