using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DeathScreen : MonoBehaviour
{
    public bool isOpen = false;

    [SerializeField] TextMeshProUGUI statsText;

    public void ToggleOn(bool on)
    {
        isOpen = on;
        gameObject.SetActive(isOpen);
    }

    public void SetStatsText(float score)
    {

        int hours = (int)(score / 3600);
        int minutes = (int)((score % 3600) / 60);
        float seconds = score % 60;

        statsText.text = $"STATS\nflight time = {hours:00}:{minutes:00}:{seconds:00.00}";
    }
}
