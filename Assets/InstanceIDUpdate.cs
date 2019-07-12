using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class InstanceIDUpdate : MonoBehaviour {

   
    public Text WelcomeUsername;
    // Get the root reference location of the database.

    public static string currentuserId;

    protected Firebase.Auth.FirebaseAuth auth;
    protected DatabaseReference databaseReference;


    // Use this for initialization
    void Awake () {

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;


       databaseReference = FirebaseDatabase.DefaultInstance.GetReference("users");

        databaseReference.Child(auth.CurrentUser.UserId).Child("username")
      .ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
      {
          if (e2.DatabaseError != null)
          {
              Debug.LogError(e2.DatabaseError.Message);
              return;
          }

          string username = e2.Snapshot.Value.ToString();

          WelcomeUsername.text = "Welcome" + " " + username + "!" + " " + " Would you like to start a new session or wait for an invitation?";






      };




       

        currentuserId = auth.CurrentUser.UserId.ToString();




          WriteInstanceID();


       




    }

     void OnDestroy()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {

    }


   


    private IEnumerator Enumerator()
    {
        Task<string> t = Firebase.InstanceId.FirebaseInstanceId.DefaultInstance.GetTokenAsync();
        while (!t.IsCompleted) yield return new WaitForEndOfFrame();
        Debug.Log(" FirebaseID is " + t.Result);
        string result = t.Result;
        UpdateInstanceID(currentuserId, result);
    }


    public void WriteInstanceID()
    {
        StartCoroutine(Enumerator());

        

    }







    private void UpdateInstanceID(string user, string instanceid)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vizzo-app.firebaseio.com/");

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("users");

        Debug.Log(currentuserId);

        reference.Child(user).Child("instanceid").SetValueAsync(instanceid);


    }



    public string GetUserId()
    {



        return currentuserId;
    }

    

}
