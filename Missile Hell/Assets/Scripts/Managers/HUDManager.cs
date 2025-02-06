using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    #region Variables
    public bool isOn = true;
    public float alertTime = 2f;
    [Header("Text")]
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI alertText;
    [Header("Other Elements")]
    [SerializeField] Image alertBackground;
    #endregion
    private void Start()
    {
        // Starts off
        gameObject.SetActive(isOn);
        alertText.text = "";
        alertBackground.enabled = false;
    }
    #region Health and Score
    /// <summary>
    /// Updates the health text on the HUD by displaying the passed parameter.
    /// </summary>
    /// <param name="health"> health value to display </param>
    public void UpdateHealth(int health)
    {
        healthText.text = $"missile integrity = {health}%";

        // Fade red based on how low your health is
        float normalized = Mathf.Clamp01(health / GameManager.Instance.maxHealth);
        healthText.color = new Color(1f, normalized, normalized);
        
    }
    /// <summary>
    /// Displays the passed score parameter (elapsed time since start of run) as a HH:MM:SS time format.
    /// </summary>
    /// <param name="score"> Elapsed time since start of run </param>
    public void UpdateScore(float score)
    {
        // Convert to HH::MM::DD
        int hours = (int)(score / 3600);
        int minutes = (int)((score % 3600) / 60);
        float seconds = score % 60;

        scoreText.text = $"flight time = {hours:00}:{minutes:00}:{seconds:00.00}";
    }
    #endregion
    #region Alert
    /// <summary>
    /// Pings an alert to the hud. "ALERT param DETECTED!"
    /// </summary>
    /// <param name="alert"></param>
    public void SetAlert(string alert)
    {
        alertBackground.enabled = true;
        alertText.text = $"ALERT! {alert} DETECTED!";
        StartCoroutine(ClearAfterSeconds(alertTime));
    }
    /*
     * Clears the alert after a given time
     */
    private IEnumerator ClearAfterSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        alertBackground.enabled = false;
        alertText.text = "";
    }
    #endregion
    /// <summary>
    /// Toggles the hud active based on the passed boolean
    /// </summary>
    /// <param name="on"></param>
    public void ToggleOn(bool on)
    {
        isOn = on;
        gameObject.SetActive(isOn);

    }
}
