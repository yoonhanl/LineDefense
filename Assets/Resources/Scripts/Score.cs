/*
This script manages the Score
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

    #region Variables

    public string prefix;

    private int _score = 0;
    public int score
    {
        get { return _score;}
        set {
            _score = value;
            GetComponent<Text>().text = prefix + _score.ToString();
            }
    }

    #endregion


    #region Methods

    //adds score after a hit
    public void addScore()
    {
        score += 1;
    }

    //used to Reset the score after each game
    public void ResetScore()
    {
        score = 0;
    }

    #endregion
}
