using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        BackgroundMusic.current.playDefaultMusic();
    }
    public void PlayGame(bool isInfinite)
    {
        GamemodeType.isInfinite = isInfinite;
        if (isInfinite)
        {
            GamemodeType.isTutorial = false;
        }
        else
        {
            GamemodeType.isTutorial = true;
        }
        SceneManager.LoadScene("MainMap");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void goToCredits()
    {
        SceneManager.LoadScene("Credits Scene");
    }
}
