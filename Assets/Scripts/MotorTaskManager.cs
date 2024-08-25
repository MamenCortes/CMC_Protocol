using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net;

public class MotorTaskManager : MonoBehaviour

{
    // Game elements
    public GameObject traffic_light;
    private Image red_light;
    private Image green_light;
    private Image yellow_light;
    public Button startButton;
    public Button next;
    public Button back; 

    //private Color red = new Color(228f, 69f, 69f, 255f);
    private Color red = Color.red; 
    private Color green = Color.green;
    private Color yellow = Color.yellow;
    //private Color yellow = new Color(217f, 200f, 66f, 255f);
    //private Color green = new Color(82f, 220f, 71f, 255f);
    
    //timer 
    //private float timeElapsed = 0;
    private List<float> events;
    private List<EventTime> eventTimes; 
    private int numTrials; 

    public bool dualTask; 

    void Start()
    {
        events = new List<float>();
        eventTimes = new List<EventTime>();
        red_light = traffic_light.transform.GetChild(0).GetComponent<Image>();
        red_light.color = red;
        yellow_light = traffic_light.transform.GetChild(1).GetComponent<Image>();
        yellow_light.color = Color.black;
        green_light = traffic_light.transform.GetChild(2).GetComponent<Image>();
        green_light.color = Color.black;

        //Button and gameObjects
        startButton.gameObject.SetActive(true);
        traffic_light.gameObject.SetActive(false);
        startButton.onClick.AddListener(startTimer);
        back.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        back.onClick.AddListener(BackToMenu);
        next.onClick.AddListener(NextExperiment);

        //set parameters
        numTrials = 0;

    }

    public void startTimer()
    {
        startButton.gameObject.SetActive(false);
        traffic_light.gameObject.SetActive(true);
        StartCoroutine(timer());
    }

    //Corrutina para controlar el tiempo de juego 
    IEnumerator timer()
    {
        int seconds = 0;
        float startTime = Time.realtimeSinceStartup;
        float timeElapsed = 0; 
        yield return new WaitForSeconds(5f);
        //We need 300 seconds to complete the whole protocol
        while (timeElapsed<=300)
        {
            yield return new WaitForSecondsRealtime(1f);
            timeElapsed = Time.realtimeSinceStartup - startTime;
            seconds++; 

            if(seconds > 0 && seconds <= 1)
            {
                //display yellow
                red_light.color = Color.black;
                yellow_light.color = yellow;

            }else if(seconds > 1 && seconds <= 4)
            {
                //display green
                yellow_light.color = Color.black; 
                green_light.color = green;
                if (seconds <= 2)
                {
                    events.Add(timeElapsed);
                    eventTimes.Add(new EventTime(timeElapsed, 3f, "")); 
                    //Debug.Log($"Event at time: {timeElapsed}");
                }

            }else if(seconds > 4 && seconds <= 5)
            {
                //display yellow
                green_light.color = Color.black;
                yellow_light.color = yellow;
            }else if(seconds > 5 && seconds < 10)
            {
                //display red
                yellow_light.color = Color.black;
                red_light.color = red;
            }
            else
            {
                //keep displaying red
                numTrials++;
                Debug.Log($"Num Trials: {numTrials}");
                seconds = 0; 
            }
        }
        traffic_light.gameObject.SetActive( false );
        endTask();
    }

    private void endTask()
    {
        StopAllCoroutines();

        if (dualTask) 
        {
            next.gameObject.SetActive(true);
            GameManager._instance.SaveEventsToCSV(eventTimes, "MCTUnrelated");
        }
        else
        {
            next.gameObject.SetActive(true);
            back.gameObject.SetActive(true);
            GameManager._instance.SaveEventsToCSV(eventTimes, "motor");
        } 
        

    }

    private void OnApplicationQuit()
    {
         endTask();
    }

    public void BackToMenu()
    {
        GameManager._instance.ChangeScene(GameManager.Scenes.Menu);
    }

    public void NextExperiment()
    {
        if (dualTask)
        {
            //Finish protocol
            GameManager._instance.ChangeScene(GameManager.Scenes.Menu);
        }
        else
        {
            //Continue to Cognitive task 
            GameManager._instance.ChangeScene(GameManager.Scenes.CognitiveTask);
        }
        
    }
}
