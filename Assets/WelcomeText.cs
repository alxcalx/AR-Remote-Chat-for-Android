using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeText : MonoBehaviour {


    public Text WelcomeUsername;

    
    // Use this for initialization

    
    void Awake () {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        FirebaseDatabase.DefaultInstance
          .GetReference("users").Child(auth.CurrentUser.UserId).Child("username")
      .ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
      {
          if (e2.DatabaseError != null)
          {
              Debug.LogError(e2.DatabaseError.Message);
              return;
          }

          string username = e2.Snapshot.Value.ToString();

          WelcomeUsername.text = "Welcome" +  " " + username + "!" + " " + " Would you like to start a new session or wait for an invitation?"; 






      };


    }


 

}
