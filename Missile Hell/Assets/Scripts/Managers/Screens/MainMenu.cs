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
    /// <summary>
    /// Toggles the Menu Game Object on/off based on passed boolean
    /// </summary>
    /// <param name="on"></param>
    public void ToggleMenu(bool on)
    {
        isOpen = on;
        gameObject.SetActive(isOpen);
    }
    /// <summary>
    /// Calls "NewGame()" from the GameManager Instance
    /// </summary>
    public void StartGame()
    {
        GameManager.Instance.NewGame();
    }
    /// <summary>
    /// Quits the application
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
