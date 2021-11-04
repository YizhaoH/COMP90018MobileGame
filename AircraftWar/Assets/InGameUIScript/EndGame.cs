using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGame : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject EndGamepage;
    public TMP_Text scores;
    public string _menu;
    
    public void Setup(int score)
    {
        Time.timeScale = 0f;
        EndGamepage.SetActive(true);
        scores.text = score.ToString();
    }
    public void retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quit()
    {
        //FirebaseManager.auth.SignOut();
        SceneManager.LoadScene(_menu);
        
        Debug.Log(FirebaseManager.User.DisplayName);
    }
}
