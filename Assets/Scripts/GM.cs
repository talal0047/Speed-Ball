using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GM : MonoBehaviour
{

    public Transform platform, trigger, coin, hurdle;
    public GameObject Effect;
    Transform player;
    private List<Transform> platform_temp = new List<Transform>();
    private List<Transform> trigger_temp = new List<Transform>();
    Vector3 startpos, endpos;

    private PlayerController controller, temp;
    private float zScenepos = 204, zScenePosition = 204,  distance_platform = 110;
    private float zobj = 15;
    private int randNumber = 0;
    int number_platform = 0, limiter = 0, LastPlatform = -1, objCreationLimiter = 0, totalPlatform = 1;
    float size, zPlatformsize, PlatformEnd, playerX, playerY;
    int sideChecker = 0;
    string selected;
    void Start()
    {
        selected = PlayerPrefs.GetString("selected", "r");
        if (selected == "b")
        {
            controller = GameObject.Find("Player").GetComponent<PlayerController>();
            //temp = GameObject.Find("Rocket").GetComponent<PlayerController>();
            //temp.gameObject.SetActive(false);
        }
        else if (selected == "r")
        {
            controller = GameObject.Find("Rocket").GetComponent<PlayerController>();
            //temp = GameObject.Find("Player").GetComponent<PlayerController>();
            //temp.gameObject.SetActive(false);
        }
        else if(selected == "c")
        {
            controller = GameObject.Find("Car").GetComponent<PlayerController>();
        }
        //controller = GameObject.Find("Player").GetComponent<PlayerController>();
        //controller = GameObject.Find("Rocket").GetComponent<PlayerController>();
        player = controller.gameObject.transform;
        for (int i = 0; i < 5; i++)
        {
            zPlatformsize = Random.Range(80, 140);
            platform_temp.Add(Instantiate(platform, new Vector3(platform.position.x, platform.position.y, zScenepos), platform.rotation) as Transform);
            platform_temp[i].localScale = new Vector3(platform.localScale.x, platform.localScale.y, zPlatformsize);
            trigger_temp.Add(Instantiate(trigger, new Vector3(trigger.position.x, trigger.position.y, (platform_temp[i].position.z + (platform_temp[i].localScale.z/2))), trigger.rotation) as Transform);
            zScenepos += (platform_temp[i].localScale.z / 2) + distance_platform;
        }
        zScenepos = 204 - (platform_temp[0].localScale.z / 2);
        PlatformEnd = platform_temp[0].localScale.z + zScenepos;
        playerX = player.position.x;
        playerY = player.position.y;
    }


    void Update()
    {

        creatingObjects();

        PlatfromJump();
        
        PlatformProgression();

        if (number_platform < 5)
        {
            if (player.position.z > zScenePosition + platform_temp[number_platform].localScale.z)
            {
                zPlatformsize = Random.Range(80, 140);
                if (number_platform == 0)
                {
                    platform_temp[0].localScale = new Vector3(platform_temp[0].localScale.x, platform_temp[0].localScale.y, zPlatformsize);
                    platform_temp[0].position = new Vector3(platform.position.x, platform.position.y, distance_platform + (platform_temp[4].localScale.z / 2) + platform_temp[4].position.z);
                    trigger_temp[0].position = new Vector3(trigger.position.x, trigger.position.y, platform_temp[0].position.z + (platform_temp[0].localScale.z / 2));
                }
                else
                {
                    platform_temp[number_platform].localScale = new Vector3(platform_temp[number_platform].localScale.x, platform_temp[number_platform].localScale.y, zPlatformsize);
                    platform_temp[number_platform].position = new Vector3(platform.position.x, platform.position.y, distance_platform + (platform_temp[number_platform - 1].localScale.z / 2) + platform_temp[number_platform - 1].position.z);
                    trigger_temp[number_platform].position = new Vector3(trigger.position.x, trigger.position.y, platform_temp[number_platform].position.z + (platform_temp[number_platform].localScale.z / 2));
                }

                if (number_platform >= 4)
                {
                    zScenePosition = platform_temp[0].position.z;   
                }
                else
                {
                    zScenePosition = platform_temp[number_platform + 1].position.z;
                }

                number_platform++;
            }
        }
        else
        {
            number_platform = 0;
        }
    }

    void creatingObjects()
    {
        //zobj = 5;
        bool side = false;
        if (objCreationLimiter < totalPlatform)
        { 
            while (zobj + zScenepos < PlatformEnd)
            {
                if (side == true && sideChecker == 0)
                {
                    randNumber = Random.Range(0, 10);
                }
                else if(sideChecker == 0)
                {
                    randNumber = 2;
                }

                if (randNumber == 3 || randNumber == 1 || randNumber == 5 || randNumber == 7 || randNumber == 9 || sideChecker == 1)
                {
                    Instantiate(coin, new Vector3(0.95f, 1.19f, (zScenepos + zobj - 2.5f)), coin.rotation);
                    Instantiate(coin, new Vector3(0.95f, 1.19f, (zScenepos + zobj)), coin.rotation);
                    Instantiate(coin, new Vector3(0.95f, 1.19f, (zScenepos + zobj + 2.5f)), coin.rotation);
                    Instantiate(hurdle, new Vector3(-0.91f, 1.25f, (zScenepos + zobj)), hurdle.rotation);
                    sideChecker = 0;
                    side = true;
                }
                else if (randNumber == 2 || randNumber == 4 || randNumber == 6 || randNumber == 8 || randNumber == 10 || sideChecker == 2)
                {
                    Instantiate(coin, new Vector3(-0.95f, 1.19f, (zScenepos + zobj - 2.5f)), coin.rotation);
                    Instantiate(coin, new Vector3(-0.95f, 1.19f, (zScenepos + zobj)), coin.rotation);
                    Instantiate(coin, new Vector3(-0.95f, 1.19f, (zScenepos + zobj + 2.5f)), coin.rotation);
                    Instantiate(hurdle, new Vector3(0.88f, 1.25f, (zScenepos + zobj)), hurdle.rotation);
                    sideChecker = 0;
                    side = true;
                }
                zobj += 20;
            }

            if((randNumber == 3 || randNumber == 1 || randNumber == 5 || randNumber == 7 || randNumber == 9))
            {
                sideChecker = 1;
                randNumber = 0;
            }
            else if((randNumber == 2 || randNumber == 4 || randNumber == 6 || randNumber == 8 || randNumber == 10))
            {
                sideChecker = 2;
                randNumber = 0;
            }

            if (zobj + zScenepos > PlatformEnd)
            {
                zobj = 15;
                objCreationLimiter++;
            }
        }
    }

    void PlatfromJump()
    {
        if (platform_temp[0].localScale.z < 88)
        {
            controller.setupwardforce(29000f);
        }

        if (number_platform < 5)
        {
            if (player.position.z >= (platform_temp[number_platform].position.z) + (platform_temp[number_platform].localScale.z / 2) - 5f)
            {
                float force = 25000, distance;

                if (number_platform == 4)
                {
                    //distance = platform_temp[4].localScale.z + platform_temp[0].localScale.z;
                    startpos = new Vector3(player.position.x, playerY, platform_temp[4].position.z + (platform_temp[4].localScale.z / 2));
                    endpos = new Vector3(player.position.x, playerY, platform_temp[0].position.z - (platform_temp[0].localScale.z / 2) + 3f);
                }
                else
                {
                    //distance = platform_temp[number_platform].localScale.z + platform_temp[number_platform + 1].localScale.z;
                    startpos = new Vector3(player.position.x, playerY, platform_temp[number_platform].position.z + (platform_temp[number_platform].localScale.z / 2));
                    endpos = new Vector3(player.position.x, playerY, platform_temp[number_platform + 1].position.z - (platform_temp[number_platform + 1].localScale.z / 2) + 3f);
                }

                //if (distance >= 160 && distance <= 180)
                //    force = 29000;
                //else if (distance > 180 && distance <= 2000)
                //    force = 27000;
                //else if (distance > 200 && distance <= 260)
                //    force = 26000;
                //else if (distance > 240)
                //    force = 25000;

                //controller.setupwardforce(force);

                //Debug.Log(startpos);
                //Debug.Log(endpos);
                controller.setVectors(startpos, endpos);
            }
        }
    }

    void PlatformProgression()
    {
        if (number_platform < 5)
        {
            if (player.position.z > platform_temp[number_platform].position.z - platform_temp[number_platform].localScale.z / 2 && player.position.z < platform_temp[number_platform].position.z && LastPlatform != number_platform)
            {
                if (number_platform >= 4)
                {
                    zScenepos = platform_temp[0].position.z - (platform_temp[0].localScale.z / 2);
                    PlatformEnd = platform_temp[0].localScale.z + zScenepos;
                    totalPlatform++;
                    LastPlatform = number_platform;

                }
                else if (number_platform < 4)
                {
                    zScenepos = platform_temp[number_platform + 1].position.z - (platform_temp[number_platform + 1].localScale.z / 2);
                    PlatformEnd = platform_temp[number_platform + 1].localScale.z + zScenepos;
                    totalPlatform++;
                    LastPlatform = number_platform;
                }
            }
        }
    }

    public void startEffect()
    {
        StartCoroutine(effect());
    }
    IEnumerator effect()
    {
        controller.gameObject.SetActive(false);
        Instantiate(Effect, controller.transform.position - new Vector3(.5f, 0, .5f), controller.transform.rotation);
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene(1);
    }

}