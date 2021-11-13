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

    public Text m_Text;

    void Start()
    {
        // Have start button preselected
        _button.Select();
    }

    // Starts the game by progressing to next scene
    public void StartGame()
    {
        //SceneManager.LoadScene("Level_1");
        //Application.backgroundLoadingPriority = ThreadPriority.Low;
        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Level_1");
        StartCoroutine(LoadScene("Level_1"));
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

    // We may need this someday
    // https://docs.unity3d.com/ScriptReference/AsyncOperation-allowSceneActivation.html
    IEnumerator LoadScene(string scene)
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            //m_Text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                //m_Text.text = "Press the space bar to continue";
                //Wait to you press the space key to activate the Scene
                Debug.Log("Job done");
                asyncOperation.allowSceneActivation = true;
                if (Input.GetKeyDown(KeyCode.Space))
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
