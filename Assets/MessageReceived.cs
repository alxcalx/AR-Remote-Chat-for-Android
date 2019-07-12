using Firebase.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MessageReceived : MonoBehaviour {
    public static string sessionId;
    public static string sendername;
    public static string receiverId;
    public static string time;
    public Text text;
    public GameObject waiting_text;
    public GameObject buttons;




    
	// Use this for initialization
	void Start () {


        FirebaseMessaging.MessageReceived += OnMessageReceived;

    
        
      
    }


    void OnDestroy()
    {


      FirebaseMessaging.MessageReceived -= OnMessageReceived;

    }

    // Update is called once per frame
    void Update () {

        AcceptorDecline();


    }


    public void OnMessageReceived(object sender, MessageReceivedEventArgs e)

    {
        
        
        e.Message.Data.TryGetValue("session_Id", out sessionId);
        e.Message.Data.TryGetValue("sender_Name", out sendername);
        e.Message.Data.TryGetValue("receiver_Id", out receiverId);
        e.Message.Data.TryGetValue("date_time", out time);

        text.text = sendername + "  " + "@ " + "   " + time;




    }

    public string GetData_sessionId()
    {

        return sessionId;
        
    }

    public string GetData_sendername()
    {

        return sendername;

    }
    public string GetData_receiverId()
    {

        return receiverId;
    }


    private void AcceptorDecline()
    {


        if(sessionId!=null && sendername != null)
        {

            buttons.SetActive(true);
            waiting_text.SetActive(false);

        }
        else
        {

            buttons.SetActive(false);

        }

    }

    public void Accept()
    {

        SceneManager.LoadScene("SelectCharacter", LoadSceneMode.Single);

    }

    public void Decline()
    {

        sessionId = null;

        sendername = null;

        waiting_text.SetActive(true);


    }


}
