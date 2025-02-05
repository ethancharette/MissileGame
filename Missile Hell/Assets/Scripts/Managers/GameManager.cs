using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
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
        currentScore = 0;
        ResetPlayer();
        OpenMenu();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !MainMenu.isOpen)
        {
            OpenPauseMenu();
        }

        if (!isPaused)
        {
            // Handle score
            currentScore += Time.deltaTime;
            HUD.UpdateScore(currentScore);
        }
        
    }
    #region Player
    private void ResetPlayer()
    {
        // Reset controller
        missileController.ResetPlayer();

        // Reset Health and Score
        currentHealth = maxHealth;
        currentScore = 0f;

        // Reset HUD
        HUD.UpdateHealth((int)currentHealth);
    }

    public void updatePlayerHealth(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0) StartCoroutine(PlayDeath());
        HUD.UpdateHealth((int)currentHealth);
    }

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

    public void OpenPauseMenu()
    {
        PauseGame(true);
        PauseScreen.SetActive(true);
    }

    public void OpenDeathScreen()
    {
        PauseGame(true);
        DeathScreen.SetStatsText(currentScore);
        DeathScreen.ToggleOn(true);
    }

    #region Buttons

    public void NewGame()
    {
        // Close All Screens
        if (MainMenu.isOpen) MainMenu.ToggleMenu(false);
        if (DeathScreen.isOpen) DeathScreen.ToggleOn(false);
        // Load Game
        PauseGame(false);
        LoadGame();
    }

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

    public void ResumeGame()
    {
        PauseGame(false);

        PauseScreen.SetActive(false);
        HUD.ToggleOn(true);
    }

    #endregion
    public void LoadGame()
    {
        // Reset Game Systems
        ResetPlayer();
        Spawner.Reset();

        // Toggle HUD
        HUD.ToggleOn(true);
    }
    #endregion
}
