using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> //Static class
{
    // Start is called before the first frame update
    public enum Scenes { MotorTask, CognitiveTask, MCTRelated, MCTUnrelated, Training, Calibration, Menu };
    public static GameManager instance; 
    void Start()
    {
        DontDestroyOnLoad(this);
        instance = this; 
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
        }

    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
        //dbManager.SaveJSONToFile();
        //dbManager.SaveXMLToFile();
        //dbManager.closeConnection();
        //Debug.Log("Connection Closed");
    }
}
