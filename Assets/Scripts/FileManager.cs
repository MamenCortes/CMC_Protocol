using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
public static class FileManger
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
            Debug.Log("Fichero guardado correctamente en: " + fullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("Error al guardar el fichero en: " + fullPath + " con el error " + e);
            return false;
        }
    }
}
