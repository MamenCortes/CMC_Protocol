using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> //Static class
{
    // Start is called before the first frame update
    public enum Scenes { MotorTask, CognitiveTask, MCTRelated, MCTUnrelated, Training, Calibration, Menu };
    public static GameManager _instance;
    private string TrialNumberKey = "trialNumber";
    void Start()
    {
        DontDestroyOnLoad(this);
        _instance = this;
        //PlayerPrefs.SetInt(TrialNumberKey, 0);
        Debug.Log($"Trial number: {PlayerPrefs.GetInt(TrialNumberKey)}");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene(Scenes scene_name)
    {
        if (scene_name == Scenes.Menu)
        {
            SceneManager.LoadScene(0);
        }else if (scene_name == Scenes.MotorTask)
        {
            SceneManager.LoadScene(1);
        }else if (scene_name == Scenes.CognitiveTask)
        {
            SceneManager.LoadScene(2);
        }

    }
    public void ChangeToMotorTask()
    {

        ChangeScene(Scenes.MotorTask);
    }
    public void ChangeToCognitiveTask()
    {
        ChangeScene(Scenes.CognitiveTask);
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
        //dbManager.SaveJSONToFile();
        //dbManager.SaveXMLToFile();
        //dbManager.closeConnection();
        //Debug.Log("Connection Closed");
    }


    public void SaveEventsToCSV(List<float> events, float duration)
    {
        int trial = PlayerPrefs.GetInt(TrialNumberKey);
        string data = "onset; duration\n";
        foreach (float onset in events)
        {
            //To avoid float numbers be saved with ',' instead of '.' because of the Visual Studio Culture settings
            string formattedOnset = onset.ToString(CultureInfo.InvariantCulture);
            data += formattedOnset+";"+duration+"\n";
        }
        FileManger.WriteToFile($"sub-01_motor-task_events_{trial}.csv", data);
        UpdateTrialNumber();
    }
    public void SaveEventsToCSV(List<EventTime> events)
    {
        int trial = PlayerPrefs.GetInt(TrialNumberKey);
        string data = "onset; duration; condition\n";
        foreach (EventTime ev in events)
        {
            //To avoid float numbers be saved with ',' instead of '.' because of the Visual Studio Culture settings
            data += ev.ToCSV(";");
        }
        FileManger.WriteToFile($"sub-01_motor-task_events_{trial}.csv", data);
        UpdateTrialNumber();
    }

    public void UpdateTrialNumber()
    {
        int trialNumber  = PlayerPrefs.GetInt(TrialNumberKey);
        trialNumber++;
        PlayerPrefs.SetInt(TrialNumberKey, trialNumber);
        PlayerPrefs.Save(); // Make sure to save the changes
    }

}
