using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static bool isGamePaused = false;

    [SerializeField] GameObject PauseMenu;
    [Header("Levels To Load")]
    public string _menu;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void QuitGame()
    {
        //FirebaseManager.auth.SignOut();
        SceneManager.LoadScene(_menu);
    }
}
