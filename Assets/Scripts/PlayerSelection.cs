using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    public void selectBall()
    {
        PlayerPrefs.SetString("selected", "b");
        SceneManager.LoadScene(0);
    }

    public void selectRocket()
    {
        PlayerPrefs.SetString("selected", "r");
        SceneManager.LoadScene(0);
    }

    public void selectCar()
    {
        PlayerPrefs.SetString("selected", "c");
        SceneManager.LoadScene(0);
    }
}
