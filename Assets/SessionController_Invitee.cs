using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionController_Invitee : MonoBehaviour {

    string invitee_character;
    
    public Text SenderName;

    // Use this for initialization
    void Start()
    {
        //  StatusCheck();
        //   StartCoroutine(Load());

        SceneController sceneController = new SceneController();

        SenderName.text = "Session in Progress";


        FirebaseDatabase.DefaultInstance
          .GetReference("sessions").Child(sceneController.GetReceiverId()).Child(sceneController.GetSessionID()).Child("status")
          .ValueChanged += HandleValueChanged;


    }

    // Update is called once per frame
    void Update()
    {

    }



    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        SceneController sceneController = new SceneController();

        Debug.Log(args.Snapshot);

     
        if (args.Snapshot.Value.ToString() == "Terminated")
        {

            SenderName.text = sceneController.GetSenderName() + " " + "canceled the session";

        }




    }

   void OnDestroy()
    {
        EndSession_Invitee();
    }

    private void EndSession_Invitee()
    {


        SceneController sceneController = new SceneController();

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vizzo-app.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("sessions");


        reference.Child(sceneController.GetReceiverId()).Child(sceneController.GetSessionID()).Child("status").SetValueAsync("Terminated");






    }


}


