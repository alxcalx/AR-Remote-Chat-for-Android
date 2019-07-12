using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

    private static SceneController _instance;
    public static SceneController instance;
    public static string sessionID;
    public static string senderID;
    public static string senderName;
    public static string receiverId;
    public static string time;
    
  
  

   

    bool isOnAndroid = false;

    MessageReceived messageReceived = new MessageReceived();

	// Use this for initialization
	void Start () {

        

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "WaitingScene")
        {

            sessionID = messageReceived.GetData_sessionId();

            senderName = messageReceived.GetData_sendername();

            receiverId = messageReceived.GetData_receiverId();

        }
    }

    private void Awake()
    {
     
       

        if (_instance !=null && _instance !=this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {

            _instance = this;
        }

        instance = _instance;
        DontDestroyOnLoad(this);

#if UNITY_ANDROID
        isOnAndroid = true;
        Screen.fullScreen = false;
#endif
    }

    private void OnApplicationPause(bool appPaused)
    {
        if(!isOnAndroid || Application.isEditor)
        {
            return;
        }

        if (!appPaused)
        {
            //Returning to Application
            Debug.Log("Application Resumed");
            StartCoroutine(LoadSceneFromFCM());
        }
        else
        {

            Debug.Log("Application Paused");
        }
    }


    IEnumerator LoadSceneFromFCM()
    {
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject curActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject curIntent = curActivity.Call<AndroidJavaObject>("getIntent");

        string sceneToLoad = curIntent.Call<string>("getStringExtra", "sceneToOpen");
        sessionID = curIntent.Call<string>("getStringExtra", "session_Id");
        senderID = curIntent.Call<string>("getStringExtra", "sender_Id");
        senderName = curIntent.Call<string>("getStringExtra", "sender_Name");
        receiverId = curIntent.Call<string>("getStringExtra", "receiver_Id");


        Scene curScene = SceneManager.GetActiveScene();
        

        if(!string.IsNullOrEmpty(sceneToLoad) && sceneToLoad != curScene.name)
        {
            Debug.Log("Loading Scene:" + sceneToLoad);
            Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Large);
            Handheld.StartActivityIndicator();
            yield return new WaitForSeconds(0f);


            SceneManager.LoadScene(sceneToLoad);


        }


    }

 

    


    public string GetSessionID()
    {
        

        return sessionID; 

    }

    public string GetSenderID()
    {
        return senderID;
       

    }

    public string GetSenderName()
    {
        return senderName;


    }

    public string GetReceiverId()
    {
        return receiverId;

    }



}
