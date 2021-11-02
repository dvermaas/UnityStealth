using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_Menu : MonoBehaviour
{
    public AudioSource _music;
    public Button _button;

    public GameObject settings_menu;

    void Start()
    {
        // Have start button preselected
        _button.Select();
    }

    // Starts the game by progressing to next scene
    public void StartGame()
    {
        SceneManager.LoadScene("Level_1");
        Debug.Log("Started the game");
    }

    // Quits application (not in editor lole)
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Closed the game");
    }

    // Set volume
    public void SetVolume(float Value)
    {
        Debug.Log("New volume:" + Value);
        _music.volume = Value;
    }
}
