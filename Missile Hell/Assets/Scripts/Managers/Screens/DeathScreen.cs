using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DeathScreen : MonoBehaviour
{
    public bool isOpen = false;

    [SerializeField] TextMeshProUGUI statsText;

    /// <summary>
    /// Toggles the Death Screen on/off based on passed boolean.
    /// </summary>
    /// <param name="on"></param>
    public void ToggleOn(bool on)
    {
        isOpen = on;
        gameObject.SetActive(isOpen);
    }
    /// <summary>
    /// Sets the final player score text on the death screen in HH:MM:SS.
    /// </summary>
    /// <param name="score"> player score </param>
    public void SetStatsText(float score)
    {

        int hours = (int)(score / 3600);
        int minutes = (int)((score % 3600) / 60);
        float seconds = score % 60;

        statsText.text = $"STATS\nflight time = {hours:00}:{minutes:00}:{seconds:00.00}";
    }
}
