using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public bool isOn = true;
    public float alertTime = 2f;
    [Header("Text")]
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI alertText;
    [Header("Other Elements")]
    [SerializeField] Image alertBackground;

    private void Start()
    {
        gameObject.SetActive(isOn);
        alertText.text = "";
        alertBackground.enabled = false;
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

    public void SetAlert(string alert)
    {
        alertBackground.enabled = true;
        alertText.text = $"ALERT!: {alert} DETECTED";
        StartCoroutine(ClearAfterSeconds(alertTime));
    }

    private IEnumerator ClearAfterSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        alertBackground.enabled = false;
        alertText.text = "";
    }

    public void ToggleOn(bool on)
    {
        isOn = on;
        gameObject.SetActive(isOn);

    }
}
