using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI coin, TotalCoin, distance;

    void Update()
    {
        //Debug.Log("here");
        coin.text = "Coins Collected: " + PlayerPrefs.GetInt("count", 0);
        //Debug.Log(coin);
        TotalCoin.text = "Total Coins: " + PlayerPrefs.GetInt("coins", 0);
        distance.text = "Distance Covered: " + PlayerPrefs.GetFloat("covered", 0);
        //Debug.Log(TotalCoin);
    }
}
