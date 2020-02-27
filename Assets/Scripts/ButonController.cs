using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButonController : MonoBehaviour
{
    public void selectPlayer()
    {
        SceneManager.LoadScene(2);
    }
}
