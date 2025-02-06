using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    /// <summary>
    /// Simple singleton
    /// </summary>
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    #region Variables
    [Header("Screens")]
    [SerializeField] public HUDManager HUD;
    [SerializeField] MainMenu MainMenu;
    [SerializeField] DeathScreen DeathScreen;
    [SerializeField] GameObject PauseScreen;

    [Header("Player")]
    public MissileController missileController;
    public float maxHealth = 100;
    private float currentHealth;

    [Header("Score")]
    public float currentScore;

    [Header("Obstacles")]
    [SerializeField] SpawnHandler Spawner;
    public float asteroidDamage = 5f;

    #endregion
    #region Time
    /// <summary>
    /// This section handles pausing and unpausing the game.
    /// </summary>
    public bool isPaused = true;
    public void PauseGame(bool pause)
    {
        isPaused = pause;
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    #endregion
    private void Start()
    {
        // Instantiate all data / set them to their base values. Reset objects.
        currentScore = 0;
        ResetPlayer();
        OpenMenu();
    }

    private void Update()
    {
        // Attempt to pause game if possible
        if (Input.GetKeyDown(KeyCode.Escape) && !MainMenu.isOpen)
        {
            OpenPauseMenu();
        }

        // Otherwise, if the game is unpaused then continue incrementing score and updating the HUD
        if (!isPaused)
        {
            // Handle score
            currentScore += Time.deltaTime;
            HUD.UpdateScore(currentScore);
        }
        
    }
    #region Player
    /// <summary>
    /// Resets the Missile Player Controller and Body to their beginning state
    /// </summary>
    private void ResetPlayer()
    {
        // Reset controller
        missileController.ResetPlayer();

        // Reset Health and Score (incase it hasn't already been reset)
        currentHealth = maxHealth;
        currentScore = 0f;

        // Reset HUD
        HUD.UpdateHealth((int)currentHealth);
    }
    /// <summary>
    /// Updates the player health by subtracting the passed parameter, handling death logic as well.
    /// </summary>
    /// <param name="damage"> Value to subtract </param>
    public void updatePlayerHealth(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0) StartCoroutine(PlayDeath());
        HUD.UpdateHealth((int)currentHealth);
    }
    /*
     * Death Coroutine, allows buffer time between when the death screen is pulled up and when the player actually dies.
     */
    private IEnumerator PlayDeath()
    {
        missileController.animator.SetBool("isDying", true);
        missileController.PlayerDeath();
        yield return new WaitForSeconds(2);
        missileController.animator.SetBool("isDying", false);
        // Open Death Screen
        HUD.ToggleOn(false);
        OpenDeathScreen();
    }

    #endregion
    #region Menus & Level
    /// <summary>
    /// Opens the pause menu
    /// </summary>
    public void OpenPauseMenu()
    {
        PauseGame(true);
        PauseScreen.SetActive(true);
    }
    /// <summary>
    /// Opens the death screen
    /// </summary>
    public void OpenDeathScreen()
    {
        PauseGame(true);
        DeathScreen.SetStatsText(currentScore);
        DeathScreen.ToggleOn(true);
    }

    #region Buttons
    /// <summary>
    /// Starts a new game
    /// </summary>
    public void NewGame()
    {
        // Close All Screens
        if (MainMenu.isOpen) MainMenu.ToggleMenu(false);
        if (DeathScreen.isOpen) DeathScreen.ToggleOn(false);
        // Load Game
        PauseGame(false);
        LoadGame();
    }
    /// <summary>
    /// Resets objects and systems to start a new game.
    /// </summary>
    public void LoadGame()
    {
        // Reset Game Systems
        ResetPlayer();
        Spawner.Reset();

        // Toggle HUD
        HUD.ToggleOn(true);
    }
    /// <summary>
    /// Opens the Main Menu, pausing the game and closing other screens.
    /// </summary>
    public void OpenMenu()
    {
        PauseGame(true);

        // turn off all other screens
        HUD.ToggleOn(false);
        DeathScreen.ToggleOn(false);
        PauseScreen.SetActive(false);

        // turn menu on
        MainMenu.ToggleMenu(true);
    }
    /// <summary>
    /// Unpauses and resumes current game.
    /// </summary>
    public void ResumeGame()
    {
        PauseGame(false);

        PauseScreen.SetActive(false);
        HUD.ToggleOn(true);
    }

    #endregion
    #endregion
}
