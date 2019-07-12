using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionController : MonoBehaviour
{

    
    public Text ReceiverName;
    public GameObject ARController;
   
    // Use this for initialization
    void Start()
    {

        AutoCompleteScrollRect autoCompleteScroll = new AutoCompleteScrollRect();

        ARController.SetActive(false);

       

   

        ReceiverName.text = "Waiting for " + " " + autoCompleteScroll.GetReceiverName();
        Debug.Log(autoCompleteScroll.GetReceiverName());


        FirebaseDatabase.DefaultInstance
          .GetReference("sessions").Child(autoCompleteScroll.GetReceiverID()).Child(autoCompleteScroll.GetSessionID()).Child("status")
          .ValueChanged += HandleValueChanged;

        

    }




    

   
    

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        AutoCompleteScrollRect autoCompleteScroll = new AutoCompleteScrollRect();

        Debug.Log(args.Snapshot);

        if(args.Snapshot.Value.ToString() == "Accepted")
        {

            ReceiverName.text = "Session in Progress";
            ARController.SetActive(true);

        }
        else if (args.Snapshot.Value.ToString() == "Terminated"){

            ReceiverName.text = autoCompleteScroll.GetReceiverName() + " " + "canceled the session";

        }

      


    }

    void OnDestroy()
    {

        EndSession_Inviter();

    }


    private void EndSession_Inviter()
    {

        AutoCompleteScrollRect autoCompleteScrollRect = new AutoCompleteScrollRect();


        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vizzo-app.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("sessions");


        reference.Child(autoCompleteScrollRect.GetReceiverID()).Child(autoCompleteScrollRect.GetSessionID()).Child("status").SetValueAsync("Terminated");











    }


  

}
