using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb, rocket, car;
    public GameObject rocketGameObject, Effect;
    public GM gameMaster;
    public float forwardforce;
    float upwardforce;
    public float swipingspeed;
    private int count;
    public float speed = 1.0f, journey, duration = 3.0f,  jumpDuration = 4.0f, journeyTime = 1.0f, speedJump, playerposition;
    public TextMeshProUGUI name, coin, TotalCoin, distance, highest;
    public Button button, selection;
    
    string selected;
    int speedLimit = 35;
    float startTime, Distance;
    Vector3 TargetDestination, StartPos, EndPos, CenterPoint, StartRelCenter, EndRelCenter;
    bool buttonStatus = false;
    int platformIndex = 1, TouchCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        ;
        selected = PlayerPrefs.GetString("selected", "a");
        playerposition = rb.transform.position.z;
        if(selected == "b")
        {
            rocket.gameObject.SetActive(false);
            car.gameObject.SetActive(false);
            rocketGameObject.gameObject.SetActive(false);
        }
        else if(selected == "r")
        {
            rb.gameObject.SetActive(false);
            car.gameObject.SetActive(false);
            rb = rocket;
        }
        else if(selected == "c")
        {
            rocket.gameObject.SetActive(false);
            rb.gameObject.SetActive(false);
            rocketGameObject.gameObject.SetActive(false);
            rb = car;
        }

        count = 0;
        upwardforce = 27000;
        //rb.AddForce(0, 0, forwardforce * Time.deltaTime * 40f);
        startTime = Time.time;
        DisplayTotalCoin();
        DisplayHighest();
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplay();
        DisplayDistance();

        if(rb.transform.position.z > playerposition + 250f)
        {
            speedLimit += 5;
            forwardforce += 5;
            playerposition = rb.transform.position.z;
        }
        if (rb.velocity.magnitude < speedLimit && buttonStatus == true)
        {
            //rb.AddForce(0, 0, forwardforce * Time.deltaTime);

            if (selected == "b" || selected == "c")
            {
                //Debug.Log(forwardforce);
                transform.Translate((Vector3.forward) * forwardforce * Time.deltaTime);
            }
            else if (selected == "r")
            {
                rocketGameObject.transform.Translate((Vector3.forward) * forwardforce * Time.deltaTime);
            }
        }
        

        if (buttonStatus && (Input.anyKeyDown || Input.touchCount > TouchCount))
        {
            if (rb.transform.position.x >= 0.05f)
            {
                TargetDestination = new Vector3(-1f, rb.transform.position.y, rb.transform.position.z + 8f);
                //Debug.Log(TargetDestination);

            }
            if (rb.transform.position.x <= -0.05f)
            {
                TargetDestination = new Vector3(1f, rb.transform.position.y, rb.transform.position.z + 8f);
                //Debug.Log(TargetDestination);

            }

            //Debug.Log(TargetDestination);
            //rb.position = Vector3.MoveTowards(rb.position, TargetDestination, swipingspeed * Time.deltaTime);
            // rb.MovePosition(TargetDestination);

            TouchCount++;
            MoveToPos(rb.transform, rb.transform.position, TargetDestination, duration);
        }

        if (transform.position.z < -5)
        {
            SceneManager.LoadScene(1);
        }
    }
            
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hurdle"))
        {
            saving();
            gameMaster.startEffect();
            /*SceneManager.LoadScene(1);*///Debug.Log("End");
        }
        else if (other.gameObject.CompareTag("platform end"))
        {
            //Debug.Log("updating " + upwardforce);
            if (platformIndex <= 1)
            {
                rb.AddForce(0f, upwardforce * Time.deltaTime, forwardforce * Time.deltaTime);
                platformIndex++;
            }
            else
            {
                jump(rb.transform, StartPos, EndPos, jumpDuration);
            }
        }
        else if (other.gameObject.CompareTag("pickup"))
        {
            count++;
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("end"))
        {
            
        }
    }  

    void saving()
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + count);
        PlayerPrefs.SetInt("count", count);
        if (rb.position.z > PlayerPrefs.GetFloat("highest", 0))
        {
            PlayerPrefs.SetFloat("highest", rb.transform.position.z);
        }
        PlayerPrefs.SetFloat("covered", rb.transform.position.z);
    }

    public void setupwardforce(float force)
    {
        upwardforce = force;
    }

    public void scoreDisplay()
    {
        coin.text = "COIN:" + count;
    }

    public IEnumerator Move(Transform target, Vector3 starting, Vector3 ending, float completeTime)
    {
        //float interpolation = 0;
        //while (interpolation <= 1)
        //{
        //    interpolation = interpolation + (Time.deltaTime / completeTime);
        //    target.position = Vector3.Lerp(starting, ending, interpolation);

        //    yield return new WaitForEndOfFrame();
        //}

        float i = 0.0f;
        float rate = (1.0f / duration) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            //Debug.Log(i);
            target.position = Vector3.Lerp(starting, ending, i);
            yield return null;
        }

    }

    public void MoveToPos(Transform target, Vector3 starting, Vector3 ending, float completeTime)
    {
        StartCoroutine(Move(target, starting, ending, completeTime));
    }

    public void Play()
    {
        buttonStatus = true;
        name.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
        TotalCoin.gameObject.SetActive(false);
        selection.gameObject.SetActive(false);
        highest.gameObject.SetActive(false);
        distance.gameObject.SetActive(true);
    }

    public void setVectors(Vector3 Start, Vector3 End)
    {
        StartPos = Start;
        EndPos = End;
    }

    public void jump(Transform target, Vector3 starting, Vector3 ending, float completeTime)
    {
        StartCoroutine(slerp(target, starting, ending, completeTime));
        //slerp();
    }

    public void GetCenter(Vector3 direction)
    {
        CenterPoint = (StartPos + EndPos) * 0.5f;
        CenterPoint -= direction;
        StartRelCenter = StartPos - CenterPoint;
        EndRelCenter = EndPos - CenterPoint;
    }

    public IEnumerator slerp(Transform target, Vector3 starting, Vector3 ending, float completeTime)
    {
        //Debug.Log("Slerp");
        GetCenter(Vector3.up);
        //float fracComplete = (Time.time - startTime) / journeyTime * speedJump;
        //transform.position = Vector3.Slerp(StartRelCenter, EndRelCenter, fracComplete * speedJump);
        //transform.position += CenterPoint;
        //yield return null;


        float interpolation = 0;
        while (interpolation <= 1)
        {
            interpolation = interpolation + (Time.deltaTime / completeTime);
            target.position = Vector3.Slerp(StartRelCenter, EndRelCenter, interpolation);
            target.position += CenterPoint;
            yield return new WaitForEndOfFrame();
        }
    }

    public void DisplayTotalCoin()
    {
        TotalCoin.text = "Total Coins: " + PlayerPrefs.GetInt("coins", 0);
    }

    void DisplayDistance()
    {
        //float dist = rb.velocity.sqrMagnitude * Time.time;
        //Debug.Log(dist);
        distance.text = "Distance: " + rb.transform.position.z;
    }

    void DisplayHighest()
    {
        highest.text = "Highest Distance: " + PlayerPrefs.GetFloat("highest", 0);

    }
}
