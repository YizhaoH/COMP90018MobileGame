using UnityEngine;
using System;
using UnityEngine.UI;
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
    }



}
