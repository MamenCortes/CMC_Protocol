using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MenuManager : MonoBehaviour
{

    //TODO: add updateView with subjects info method

    [SerializeField]
    private TMP_InputField name;
    [SerializeField]
    private TMP_InputField surname;
    [SerializeField]
    private TMP_InputField sub;
    [SerializeField]
    private TMP_InputField session;
    [SerializeField]
    private TMP_InputField filePath;
    [SerializeField]
    private TMP_Text error;
    [SerializeField]
    private Button save;
    [SerializeField]
    private Button openDirectory;
    [SerializeField]
    private TMP_Dropdown sex;

    void Start()
    {
        save.onClick.AddListener(saveSubjectInfo);
        openDirectory.onClick.AddListener(OpenDirectory); 
        error.gameObject.SetActive(false);

        UpdateView(); 
    }

    public void saveSubjectInfo()
    {
        error.gameObject.SetActive(false);
        filePath.textComponent.color = Color.white;
        string nameInput = name.text;
        string surnameInput = surname.text;
        string filePathInput = filePath.text;
        string sexInput = sex.options[sex.value].text;
        string subnInput = sub.text;
        string sessionInput = session.text;
        int subNum = -1;
        int sessionNum; 


        Debug.Log($"Name: {nameInput}, Surname: {surnameInput}, sex: {sexInput}, filepath: {filePathInput}"); 

        if (nameInput == "" || surnameInput=="" || subnInput=="") {
            error.gameObject.SetActive(true);
            error.text = "Fill in the subject's personal information"; 
        }
        else if (!int.TryParse(subnInput, out subNum))
        {
            error.gameObject.SetActive(true);
            error.text = $"The subject number is incorrect: {subNum}";
        }
        else
        {
            if(!int.TryParse(sessionInput, out sessionNum))
            {
                GameManager._instance.session = 1;
            }
            else
            {
                GameManager._instance.session = sessionNum;
            }
            //If everything is correct, save them in the GameManager variables for later use
            GameManager._instance.name = nameInput;
            GameManager._instance.surname = surnameInput;
            GameManager._instance.sex = sexInput;
            GameManager._instance.subn = subNum;
        }
        
        
        if (filePathInput == "")
        {
            error.gameObject.SetActive(true);
            error.text = "Select a path to save the event times";
        }
        else if (!Directory.Exists(filePathInput))
        {
            error.text = "Path doesn't exist";
            //if it doesn't, create it Directory.CreateDirectory(directoryPath); }
        }
        else
        {
            filePath.textComponent.color = Color.green; 

            //Save subject info
            GameManager._instance.filePath = filePathInput;
        }

    }

    public void OpenDirectory()
    {
        string directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        filePath.text = directory;

    }

    private void UpdateView()
    {
        if (GameManager._instance != null)
        {
            name.text = GameManager._instance.name;
            surname.text = GameManager._instance.surname;
            string sex = GameManager._instance.sex;
            filePath.text = GameManager._instance.filePath;
            sub.text = GameManager._instance.subn.ToString();
            session.text = GameManager._instance.session.ToString();
        }

    }

    public void startProtocol()
    {
        if(GameManager._instance.filePath == "" || GameManager._instance.subn == -1 || GameManager._instance.session == -1)
        {
            error.gameObject.SetActive(true);
            error.text = "The folder path, subject number and session must be provided before starting the protocol";
        }
        else
        {
            GameManager._instance.saveSubject2File(); 
            GameManager._instance.ChangeScene(GameManager.Scenes.Training);
        }
    }

}
