using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreElement : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text RankText;
    public TMP_Text NumberOfCoinsText;
    public TMP_Text NumberOfDiamondsText;

    public void NewScoreElement (string _username, int _rank, int _coins, int _diamonds)
    {
        usernameText.text = _username;
        if(_rank<10)
            RankText.text = "#0"+_rank.ToString();
        else
            RankText.text = "#" + _rank.ToString();
        NumberOfCoinsText.text = _coins.ToString();
        NumberOfDiamondsText.text = _diamonds.ToString();
    }

}
