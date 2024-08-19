using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class StroopTestManager : MonoBehaviour
{
    // Game elements
    public TMP_Text colorText;
    public Button startButton; 


    //timer 
    private List<float> events;
    private List<EventTime> eventTimes; 
    private int numTrials;
    private int maxTrials = 30; 
    private int numeBlocks;
    private int maxBloks = 3; 
    private float timeElapsed;

    //Stroop test variables
    public Boolean dualTask; 
    private List<Color> colors;
    private List<String> colorNames;
    private List<String> neutralNames;
    public float yellowProbability = 0.67f; 

    private enum Conditions { Congruent, Incongruent, Neutral };
    // Access an enum by index: Conditions c = (Conditions)index;

    void Start()
    {
        events = new List<float>();
        eventTimes = new List<EventTime>();

        //Button and gameObjects
        startButton.gameObject.SetActive(true);
        colorText.gameObject.SetActive(false);
        startButton.onClick.AddListener(startTimer);

        //set parameters
        numTrials = 0;
        timeElapsed = 0;
 
        //The yellow color must go at the end of the list
        //The lists should be of the same length
        colors = new List<Color> { Color.red, Color.green, Color.blue, Color.yellow};
        neutralNames = new List<String> { "@@@@@@", "######", "%%%%%%", "$$$$$$$" };

        if (dualTask)
        {
            colorNames = new List<String> { "GRIP", "FLEX", "EXTEND", "GRASP" };
        }
        else
        {
            colorNames = new List<String> { "RED", "GREEN", "BLUE", "YELLOW" };
        }


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
        while (numeBlocks <= maxBloks)
        {
            while (numTrials < maxTrials)
            {
                //Decide whether the trial will be congruent, incongruent or neutral
                int c = UnityEngine.Random.Range(0, 3); 
                Conditions condition = (Conditions)c;

                // Decide whether to show yellow or not
                int colorIndex;
                bool showYellow = UnityEngine.Random.value <= yellowProbability;
                if (showYellow)
                {
                    colorIndex = colors.Count - 1; // Índice de amarillo
                }
                else
                {
                    // Choose a color different from yellow
                    colorIndex = UnityEngine.Random.Range(0, colors.Count - 1);
                }

                //Select color name and font depending on the condition. 
                if (condition == Conditions.Congruent)
                {
                    //Congruent condition
                    colorText.text = colorNames[colorIndex];
                    colorText.color = colors[colorIndex];
                    //Debug.Log($"Trial Condition: {condition}. Word: {colorNames[colorIndex]} in color {colors[colorIndex]}");
                }
                else if(condition == Conditions.Incongruent)
                {
                    int nameIndex = -1; 
                    //Incongruent condition
                    
                    // Filter the colorNames list to exclude the color name of the selected color font
                    //Exclude the name at index colorIndex
                    List<string> filteredNames = colorNames.Where((name, idx) => (idx != colorIndex)).ToList();
                    nameIndex = UnityEngine.Random.Range(0, filteredNames.Count);
                    //Debug.Log($"Index: {nameIndex}");
                    //Debug.Log($"Trial Condition: {condition}. Word: {filteredNames[nameIndex]} in color {colors[colorIndex]}");

                    colorText.color = colors[colorIndex];
                    colorText.text = filteredNames[nameIndex];
                }
                else if(condition == Conditions.Neutral)
                {
                    colorText.color = colors[colorIndex];
                    colorText.text = neutralNames[colorIndex];
                    //Debug.Log($"Trial Condition: {condition}. Word: {neutralNames[colorIndex]} in color {colors[colorIndex]}");
                }

                yield return new WaitForSecondsRealtime(1f);

                //After 1 second of color display, if the color presented is yellow, save the time event
                float contractionStart = 0; 
                if (showYellow)
                {
                    contractionStart = timeElapsed;
                }
                yield return new WaitForSecondsRealtime(4f);

                //TODO: save event times
                if (showYellow) {
                    saveTimeEvent(contractionStart, timeElapsed - contractionStart, condition);
                }
                
                //5 seconds of resting time
                colorText.text = "X";
                colorText.color = Color.white;
                yield return new WaitForSecondsRealtime(5f);

                numTrials++;
                Debug.Log($"Num Trials: {numTrials}");
            }
            numTrials = 0;
            numeBlocks++;
            yield return new WaitForSecondsRealtime(60f);
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

    private void saveTimeEvent(float onset, float duration, Conditions condition)
    {
        EventTime ev = new EventTime(onset, duration, condition.ToString());
        eventTimes.Add(ev);
        events.Add(timeElapsed);
        Debug.Log($"Event at time: {timeElapsed}");
        Debug.Log($"EventTime: {ev}"); 
    }
    private void endTask()
    {
        StopAllCoroutines();
        GameManager._instance.ChangeScene(GameManager.Scenes.Menu);
        GameManager._instance.SaveEventsToCSV(events, 3f);
    }

    private void OnApplicationQuit()
    {
        Debug.Log($"Total number of trials{numTrials}");
        //GameManager._instance.SaveEventsToCSV(events, 3f);
        GameManager._instance.SaveEventsToCSV(eventTimes);
        StopAllCoroutines();
    }

}
