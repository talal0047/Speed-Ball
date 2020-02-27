using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController player;
    private GameObject rocket;
    private Vector3 offset;
    string selected;
    // Start is called before the first frame update
    void Start()
    {
        selected = PlayerPrefs.GetString("selected", "r");
        if(selected == "b")
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            offset = transform.position - player.transform.position;
        }
        else if(selected == "r")
        {
            player = GameObject.Find("Rocket").GetComponent<PlayerController>();
            //rocket = player.transform.GetChild(0).gameObject;
            offset = transform.position - player.transform.position;
        }
        else if (selected == "c")
        {
            player = GameObject.Find("Car").GetComponent<PlayerController>();
            offset = transform.position - player.transform.position;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.gameObject.transform.position + offset;
    }
}
