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
    [SerializeField]
    private GameObject folderSelection;
    [SerializeField]
    private Button settingsButton;
    private Button saveFolder;
    private TMP_InputField folderPath;
    private Button openFolder;
    private TMP_Text error2; 

    void Start()
    {
        save.onClick.AddListener(saveSubjectInfo);
        openDirectory.onClick.AddListener(() => OpenDirectory(filePath)); ; 

        saveFolder = getChildGameObject(folderSelection, "save").GetComponent<Button>(); 
        folderPath = folderSelection.GetComponentInChildren<TMP_InputField>();
        openFolder = getChildGameObject(folderSelection, "openFolder").GetComponent<Button>();
        error2 = getChildGameObject(folderSelection, "error").GetComponent<TMP_Text>();
        error2.gameObject.SetActive(false);
        saveFolder.onClick.AddListener(SaveFolderInfo);
        openFolder.onClick.AddListener(() => OpenDirectory(folderPath));
        settingsButton.onClick.AddListener(OpenFolderInfo); 
        folderSelection.gameObject.SetActive(false);

        UpdateView(); 
    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {
        //Author: Isaac Dart, June-13.
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
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

    public void OpenDirectory(TMP_InputField inputfield)
    {
        string directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        //show directory chosen in this input field
        inputfield.text = directory;

    }

    private void UpdateView()
    {
        error.gameObject.SetActive (false);
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
        if (GameManager._instance.participantsFolderPath == "")
        {
            error.gameObject.SetActive(true);
            error.text = "Select a folder's path to save the participant's info";
        }
        else
        {
            if (GameManager._instance.filePath == "" || GameManager._instance.subn == -1 || GameManager._instance.session == -1)
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

    public void SaveFolderInfo()
    {
        error2.gameObject.SetActive(false);
        string folderPathInput = folderPath.text;
        GameManager._instance.participantsFolderPath = folderPath.text;
        if (folderPathInput == "")
        {
            error2.gameObject.SetActive(true);
            error2.text = "Select a path to save the participant's info";
        }
        else if (!Directory.Exists(folderPathInput))
        {
            error2.text = "Path doesn't exist";
            //if it doesn't, create it Directory.CreateDirectory(directoryPath); }
        }
        else
        {
            //Save subject info
            GameManager._instance.SetFolderPath(folderPathInput); 
            //GameManager._instance.participantsFolderPath = folderPathInput;
            folderSelection.gameObject.SetActive(false);
        }
    }

    private void OpenFolderInfo()
    {
        if(folderSelection.gameObject.activeSelf== false)
        {
            folderSelection.gameObject.SetActive(true);
            if (GameManager._instance.participantsFolderPath != null)
            {
                folderPath.text = GameManager._instance.participantsFolderPath;
            }
        }
        else
        {
            folderSelection.gameObject.SetActive(false);
        }
        

    }

    private void checkFolderPathNotEmpty()
    {
        if (GameManager._instance.CheckFolderPathNotEmpty())
        {
            error.gameObject.SetActive(false);
            error.text = "Select a path to save the participant's info!!";
        }
    }
}
