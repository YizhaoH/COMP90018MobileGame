using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Screen object variables
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject personalPage;
    public GameObject gameStart;
    public GameObject mainmenu;
    public GameObject startpage;

    [Header("Levels To Load")]
    public string _newGameLevel1;
    public string _newGameLevel2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    public void ClearScreen()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        personalPage.SetActive(false);
    }

    //Functions to change the login screen UI
    public void LoginScreen() //Back button
    {
        ClearScreen();
        loginUI.SetActive(true);
    }
    public void RegisterScreen() // Regester button
    {
        ClearScreen();
        registerUI.SetActive(true);
    }
    public void PersonalPage()
    {
        ClearScreen();
        personalPage.SetActive(true);
    }
    public void MainMenu()
    {
        ClearScreen();
        startpage.SetActive(false);
        mainmenu.SetActive(true);
    }
    public void GameStart()
    {
        Time.timeScale = 1f;
        if (Random.Range(1,100)<= 50)
        {
            SceneManager.LoadSceneAsync(_newGameLevel1);
        }
        else
        {
            SceneManager.LoadSceneAsync(_newGameLevel2);
        }
        
    }

    public void Quitgame()
    {
        Application.Quit();
    }
}
