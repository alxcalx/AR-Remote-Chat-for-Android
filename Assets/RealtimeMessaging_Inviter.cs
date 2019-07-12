using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using Firebase.Unity.Editor;
using TMPro;

public class RealtimeMessaging_Inviter : MonoBehaviour {

   
    public Canvas canvas_MessageScrollView;
    public InputField messagebox;
    public TMP_Text TMP_ChatOutput;
    public Scrollbar ChatScrollbar;
    string inviteemessage;
    string receiverId;
    string key;
    string error;
    bool Dialog;


    
    // Use this for initialization
    void Start () {

        AutoCompleteScrollRect autoComplete = new AutoCompleteScrollRect();
        DatabaseReference databaseReference = FirebaseDatabase.DefaultInstance.GetReference("sessions");
        canvas_MessageScrollView.GetComponent<CanvasGroup>().alpha = 0;
        receiverId = autoComplete.GetReceiverID();

       databaseReference.Child(receiverId).Child(autoComplete.GetSessionID()).Child("invitee_message")
      .ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
      {
          if (e2.DatabaseError != null)
          {
              Debug.LogError(e2.DatabaseError.Message);
              return;
          }
          Debug.Log("Receiving messages.");






          if (e2.Snapshot.Value.ToString() != null)
          {

              AddToChatOutput("<" + autoComplete.GetReceiverName() + ">" + " " + e2.Snapshot.Value.ToString());

          }

      };

       databaseReference.Child(receiverId).Child(autoComplete.GetSessionID()).Child("invitee_animation")
 .ValueChanged += (object sender2, ValueChangedEventArgs e3) =>
 {
     if (e3.DatabaseError != null)
     {
         Debug.LogError(e3.DatabaseError.Message);
         return;
     }
     Debug.Log("Receiving messages.");

     if (e3.Snapshot.Value.ToString() != null)
     {

         AddToChatOutput(autoComplete.GetReceiverName() + "'s" +  " " + "Selected Animation:" + " " + e3.Snapshot.Value.ToString());

     }

 };

     



    }



	
	// Update is called once per frame
	void Update () {

      
	}

    public void OnGUI()
    {
        DisplayButton();

    }

    public void DisplayButton()
    {



        if (Dialog)
        {

            GUI.BeginGroup(new Rect(Screen.width / 2 - 150, 50, 300, 250));

            GUI.Box(new Rect(0, 0, 300, 250), "");

            GUI.Label(new Rect(15, 10, 300, 68), error);

            if (GUI.Button(new Rect(55, 150, 180, 40), "Okay"))
            {
                Dialog = false;

            }

            GUI.EndGroup();


        }




    }








    private void StoreMessage(string message, string time)
    {

        AutoCompleteScrollRect autoComplete = new AutoCompleteScrollRect();

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vizzo-app.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("sessions");

        reference.Child(receiverId).Child(autoComplete.GetSessionID()).Child("inviter_message").SetValueAsync(message);


    }





    public void SendMessage()
    {


        if(String.IsNullOrEmpty(messagebox.text))
        {
            error = "Please Enter A Message";
            Dialog = true;
            


        }
        else
        {
            if(messagebox.text.Length > 100)
            {

                error = "Please limit it to 100 characters. The character count is " + messagebox.text.Length;
                Dialog = true;



            }
            else
            {
                string date = DateTime.Now.ToString("dd/MM/yyy hh:mm:ss tt", System.Globalization.DateTimeFormatInfo.InvariantInfo);

                StoreMessage(messagebox.text, date);

                AddToChatOutput(messagebox.text);

            }


        }


    

    }


    

    public void AddToChatOutput(string newText)
    {
        // Clear Input Field
        messagebox.text = string.Empty;

        var timeNow = System.DateTime.Now;

        TMP_ChatOutput.text += "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2") + "</color>] " + newText + "\n";

        messagebox.ActivateInputField();

        // Set the scrollbar to the bottom when next text is submitted.
        ChatScrollbar.value = 0;

    }




}


















