using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour {

	// Use this for initialization
	void Awake () {




        

        if (!PlayerPrefs.HasKey("isFirstTime") || PlayerPrefs.GetInt("isFirstTime") != 1)
        {
            SceneManager.LoadScene("LanguageSelection", LoadSceneMode.Single);
         
        }
        else
        {
            SceneManager.LoadScene("Aunthentication", LoadSceneMode.Single);

        }
     
            


        
    }
	

}
