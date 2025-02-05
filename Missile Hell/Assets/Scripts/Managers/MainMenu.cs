using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public bool isOpen = true;

    private void Start()
    {
        gameObject.SetActive(isOpen);
    }

    public void ToggleMenu(bool on)
    {
        isOpen = on;
        gameObject.SetActive(isOpen);
    }

    public void StartGame()
    {
        GameManager.Instance.NewGame();
    }
}
