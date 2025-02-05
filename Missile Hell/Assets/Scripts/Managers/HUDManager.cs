using System;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public bool isOn = true;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI scoreText;

    private void Start()
    {
        gameObject.SetActive(isOn);
    }
    public void UpdateHealth(int health)
    {
        healthText.text = $"missile integrity = {health}%";

        // Change Color
        float normalized = Mathf.Clamp01(health / GameManager.Instance.maxHealth);
        healthText.color = new Color(1f, normalized, normalized);
        
    }

    public void UpdateScore(float score)
    {

        int hours = (int)(score / 3600);
        int minutes = (int)((score % 3600) / 60);
        float seconds = score % 60;

        scoreText.text = $"flight time = {hours:00}:{minutes:00}:{seconds:00.00}";
    }

    public void ToggleOn(bool on)
    {
        isOn = on;
        gameObject.SetActive(isOn);

    }
}
