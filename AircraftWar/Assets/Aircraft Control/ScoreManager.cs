using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text Scores;
    public TMP_Text FinalScores;

    public int score;
    void Start()
    {
        // set score value to be zero
        score = 0;
    }  
    void LateUpdate() 
    {
        Scores.text = score.ToString();
        StartCoroutine(UpdateScoreDatabase(Scores.text));
    }
    
    private IEnumerator UpdateScoreDatabase(string _score)
    {
        //Set the currently logged in user username in the database
        var DBTask = FirebaseManager.DBreference.Child("users").Child(FirebaseManager.User.UserId).Child("score").SetValueAsync(_score);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }

}
