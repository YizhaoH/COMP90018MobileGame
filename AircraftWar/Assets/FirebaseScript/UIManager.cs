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
    public string _newGameLevel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
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
        SceneManager.LoadSceneAsync(_newGameLevel);
        
    }
}
