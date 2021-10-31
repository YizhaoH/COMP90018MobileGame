using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text Scores;

    public int score = 0 ;
    
    public void ScoreDisplay()
    {
        Scores.text = score.ToString();
    }
}
