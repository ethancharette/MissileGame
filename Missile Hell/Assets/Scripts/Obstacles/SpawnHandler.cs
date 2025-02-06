using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    #region Variables
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
    #endregion

    private void Start()
    {
        player = GameManager.Instance.missileController.body;
        // Calls "SpawnInteractable" after interactableSpawnRate seconds, and every InteractableSpawnRate seconds.
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
    /// <summary>
    /// Spawns a random hazardous object
    /// </summary>
    private void SpawnHazard()
    {
        // Pull from pool by asteroid chance
        ObjectPool pool = Random.Range(0f, 100f) < explosiveChance ? asteroidExplosivePool : asteroidPool;
        
        // Get random spawn position in range
        Vector3 randomOffset = RandomInRange();
        Vector3 spawnPosition = transform.position + randomOffset;

        // Get and set spawn point
        GameObject asteroid = pool.GetFromPool(spawnPosition);
    }
    /// <summary>
    /// Potentially spawns an interactable object on chance.
    /// </summary>
    private void SpawnInteractable()
    {
        // Try to spawn an interactable obj
        if (Random.Range(0f, 100f) <= wormholeChance)
        {
            // Alert HUD, Instantiate object at a random spawn location, and store the game object under a container parent
            GameManager.Instance.HUD.SetAlert("ANOMOLY");
            GameObject obj = Instantiate(wormHole, RandomInRange(), Quaternion.identity);
            obj.transform.SetParent(interactableContainer.transform);
        }

    }
    #endregion
    /// <summary>
    /// Resets the spawner to it's initial state
    /// </summary>
    public void Reset()
    {
        // Reset Cooldowns / Spawning
        cooldown = hazardSpawnRate;
        CancelInvoke(methodName: "SpawnInteractable");
        InvokeRepeating("SpawnInteractable", interactableSpawnRate, interactableSpawnRate);
        // Reset Pools
        asteroidPool.ResetPool();
        asteroidExplosivePool.ResetPool();
    }
    /// <summary>
    /// Returns a random Vector3 in the range of the game object
    /// </summary>
    /// <returns></returns>
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
