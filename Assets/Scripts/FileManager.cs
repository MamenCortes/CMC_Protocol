using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
public static class FileManager
{
    public static bool WriteToFile(string filename, string data)
    {
        string fullPath; 
        if (GameManager._instance.filePath == null || GameManager._instance.filePath == "")
        {
            fullPath = Path.Combine(Application.persistentDataPath, filename);
        }
        else
        {
            fullPath = Path.Combine(GameManager._instance.filePath, filename);
        }
    

        try
        {
            File.WriteAllText(fullPath, data);
            Debug.Log("File saved at: " + fullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("Error saving file at: " + fullPath + " with error" + e);
            return false;
        }
    }

    public static bool SaveSubjectToFile(string folderPath, string fileName, string newSubject)
    {
        string filePath = Path.Combine(folderPath, fileName);

        // Check if the folder already exists
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Check if the file already exists
        if (!File.Exists(filePath))
        {
            // If the file doesn't exist, create it and add the headings 
            string encabezados = "Subject num;Session;Name;Surname;Sex;Events File Path ";
            File.WriteAllText(filePath, encabezados + "\n");
        }

        try
        {
            // Añadir la nueva fila de datos
            File.AppendAllText(filePath, newSubject + "\n");
            Debug.Log("New subject added to CSV file at: " + filePath);
            return true; 
        }
        catch (Exception e) {
            Debug.Log("Error saving file at: " + filePath + " with error" + e);
            return false;
        }

    }
}
