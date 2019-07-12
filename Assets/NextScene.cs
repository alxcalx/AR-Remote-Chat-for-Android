using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour {

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "ForgotPassword")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Aunthentication");

            }

        }

        if (SceneManager.GetActiveScene().name == "SignUp")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Aunthentication");

            }

        }


    }

    public void AuthenticationScene()
    {
        SceneManager.LoadScene("Aunthentication");

    }


    public void Settings()
    {
        SceneManager.LoadScene("Settings");

    }


    public void MainScene()
    {
        SceneManager.LoadScene("Main Scene");

    }

    public void ChangePassword()
    {

        SceneManager.LoadScene("ForgotPassword");
    }

    public void Register()
    {

        SceneManager.LoadScene("SignUp", LoadSceneMode.Single);
        

    }

    public void WelcomeScene()
    {
        SceneManager.LoadScene("WelcomeScene");

    }

    public void LogoScene()
    {

        SceneManager.LoadScene("LogoScene");
    }

    public void PreMaintoMain()
    {


        if (PlayerPrefs.HasKey("Tutorial") || PlayerPrefs.GetInt("Tutorial") == 1)
        {

            SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);

        }
        else
        {

            SceneManager.LoadScene("Main Scene_Help", LoadSceneMode.Single);

        }

    }

    public void WaitingScene()
    {

        SceneManager.LoadScene("WaitingScene", LoadSceneMode.Single);

    }
    public void PreMain()
    {

        SceneManager.LoadScene("Pre-Main", LoadSceneMode.Single);

    }


}
