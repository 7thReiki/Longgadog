using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void InstructionsGame()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void Menu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void CreditsMenu()
    {
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
