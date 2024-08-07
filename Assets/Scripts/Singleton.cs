using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    //Creo una referencia a mi GameManager (y a todos sus m?todos) que
    //como es est?tico no necesito un GameObject para acceder a ?l pero s? 
    //una referencia 
    protected static T instance;

    //Para acceder a todos los m?todos de GameManager, como es privado, genero la variable 
    //p?blica con lo que define el acceso con los getters y setters
    public static T Instance
    {
        get
        {
            if (instance == null) //Busco si existe alguno en la escena
            {
                instance = FindObjectOfType<T>(); //si lo hay, coge ese
                if (instance == null) //si no lo hay
                {
                    //lo creo
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }
            return instance;
        }
    }



}
