using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> //Static class
{
    // Start is called before the first frame update
    public enum Scenes { MotorTask, CognitiveTask, MCTRelated, MCTUnrelated, Training, Calibration, Menu };
    public static GameManager _instance;
    private string participantsFolderPath = "C:/Users/mamen/git/CMC_Protocol";
    private string participantsFileName = "CMC_subjects.csv"; 


    //Participant's info
    public string name = "";
    public string surname = "";
    public int subn = -1; //subject number
    public int session = -1; 
    public string sex = "";
    public string filePath = "";
    void Start()
    {
        DontDestroyOnLoad(this);
        _instance = this;

        //initialize variables
        name = ""; 
        surname = "";
        subn = -1;
        session = -1;
        sex = ""; 
        filePath = "";
    }

    public void ChangeScene(Scenes scene_name)
    {
        if (scene_name == Scenes.Menu)
        {
            SceneManager.LoadScene(0);
        }
        else if (scene_name == Scenes.Training)
        {
            SceneManager.LoadScene(1);
        }
        else if (scene_name == Scenes.MotorTask)
        {
            SceneManager.LoadScene(2);
        }
        else if (scene_name == Scenes.CognitiveTask)
        {
            SceneManager.LoadScene(3);
        }
        else if (scene_name == Scenes.MCTRelated)
        {
            SceneManager.LoadScene(4);

        }else if(scene_name == Scenes.MCTUnrelated)
        {
            SceneManager.LoadScene(5); 
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
    public void ChangeToTrainingTask()
    {
        ChangeScene(Scenes.Training); 
    }
    public void ChangeToMCTRelated()
    {
        ChangeScene(Scenes.MCTRelated);
    }
    public void ChangeToMCTUnrelated()
    {
        ChangeScene(Scenes.MCTUnrelated); 
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// BIDS format for events 
    ///  sub-<label>[_ses-<label>]_task-<label>[_acq-<label>][_run-<index>]_events.csv
    /// </summary>
    /// <param name="events"></param>
    /// <param name="task"></param>
    public void SaveEventsToCSV(List<EventTime> events, string task)
    {
        string data = "onset; duration; condition\n";
        foreach (EventTime ev in events)
        {
            data += ev.ToCSV(";");
        }
        FileManager.WriteToFile($"sub-{subn}_ses-{session}_task-{task}_events.csv", data);
    }

    public void saveSubject2File()
    {
        //"Subject Nº,Session,Name,Surname,Sex,Events File Path ";
        string sub = $"{subn};{session};{name};{surname};{sex};{filePath}";
        Debug.Log(sub);
        bool saved = FileManager.SaveSubjectToFile(participantsFolderPath, participantsFileName, sub); 
    }
}
