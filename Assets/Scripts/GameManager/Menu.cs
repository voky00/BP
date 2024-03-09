using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject gameMenu;
    public RoundManager rndM;

    public void BackToGame()
    {
        gameMenu.SetActive(false);
        rndM.menuShown = false;
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
