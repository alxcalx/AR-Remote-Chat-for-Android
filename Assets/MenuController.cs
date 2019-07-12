using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour {




    public Canvas canvas;

	// Use this for initialization
	void Start () {
 
        canvas.GetComponent<CanvasGroup>().alpha = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Home_Inviter()

    {
          
       
        SceneManager.LoadScene("Pre-Main");
        EndSession_Inviter();
      

    }

    public void Home_Invitee()
    {
        SceneManager.LoadScene("Main Scene");
        EndSession_Invitee();

    }

    public void OpenChatView()
    {
        canvas.GetComponent<CanvasGroup>().alpha = 1;

    }

    public void CloseChatView()
    {
        canvas.GetComponent<CanvasGroup>().alpha = 0;
    }



    private void EndSession_Inviter()
    {

        AutoCompleteScrollRect autoCompleteScrollRect = new AutoCompleteScrollRect();
       

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vizzo-app.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("sessions");

        
        reference.Child(autoCompleteScrollRect.GetReceiverID()).Child(autoCompleteScrollRect.GetSessionID()).Child("status").SetValueAsync("Terminated");


       
        
        
            
        
  



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
