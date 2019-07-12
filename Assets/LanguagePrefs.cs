using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using System;
using System.Threading.Tasks;

public class LanguagePrefs: MonoBehaviour {

  

    void Start()
    {


        PlayerPrefs.SetInt("isFirstTime", 1);
        PlayerPrefs.Save();



    }


 

  



}
   

