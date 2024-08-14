using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StroopTestManager : MonoBehaviour
{
    // Game elements
    public TMP_Text colorText;
    public Button startButton; 


    //timer 
    private List<float> events;
    private int numTrials;
    private float timeElapsed;

    //Stroop test variables
    private List<Color> colors;
    private List<String> colorNames;
    public float yellowProbability = 0.33f; 

    void Start()
    {
        events = new List<float>();

        //Button and gameObjects
        startButton.gameObject.SetActive(true);
        colorText.gameObject.SetActive(false);
        startButton.onClick.AddListener(startTimer);

        //set parameters
        numTrials = 0;
        timeElapsed = 0;
 
        //The yellow color must go at the end of the list
        colors = new List<Color> { Color.red, Color.green, Color.blue, Color.magenta, Color.white, Color.yellow};
        colorNames = new List<String> { "RED", "GREEN", "BLUE", "PINK", "WHITE", "YELLOW" };

    }

    public void startTimer()
    {
        startButton.gameObject.SetActive(false);
        colorText.gameObject.SetActive(true);
        StartCoroutine(Timer());
        StartCoroutine(ShowColorRutine());
    }

    //Corrutina para controlar el tiempo de juego 
    IEnumerator ShowColorRutine()
    {
        while (numTrials <= 30)
        {

            // Decide whether to show yellow or not
            bool showYellow = UnityEngine.Random.value <= yellowProbability;
            int colorIndex;

            if (showYellow)
            {
                colorIndex = colors.Count-1; // Índice de amarillo
            }
            else
            {
                // Choose a color different from yellow
                colorIndex = UnityEngine.Random.Range(0, colors.Count-2);
            }

            //Set colorName and color font
            colorText.text = colorNames[colorIndex];
            colorText.color = colors[colorIndex];
            yield return new WaitForSecondsRealtime(1f);

            //After 1 second of color display, if the color presented is yellow, save the time event
            if (showYellow) {
                saveTimeEvent(); 
            }
            yield return new WaitForSecondsRealtime(4f);

            //5 seconds of resting time
            colorText.text = "X";
            colorText.color = Color.white;
            yield return new WaitForSecondsRealtime(5f);

            numTrials++;
            Debug.Log($"Num Trials: {numTrials}");
        }

        endTask();
    }

    IEnumerator Timer()
    {
        float startTime = Time.realtimeSinceStartup;
        while (numTrials <= 30) {
            yield return new WaitForSecondsRealtime(1f);
            timeElapsed = Time.realtimeSinceStartup - startTime;
        }
    }

    private void saveTimeEvent()
    {
        events.Add(timeElapsed);
        Debug.Log($"Event at time: {timeElapsed}");
    }
    private void endTask()
    {
        StopAllCoroutines();
        GameManager._instance.ChangeScene(GameManager.Scenes.Menu);
        GameManager._instance.SaveEventsToCSV(events, 3f);
    }

    private void OnApplicationQuit()
    {
        Debug.Log(numTrials);
        GameManager._instance.SaveEventsToCSV(events, 3f);
        StopAllCoroutines();
    }

}
