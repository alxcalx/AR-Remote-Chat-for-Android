using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations_Inviter : MonoBehaviour {

 
    public static string animation_play;






    public void StoreAnimation(string animation)
    {
        AutoCompleteScrollRect autoComplete = new AutoCompleteScrollRect();

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vizzo-app.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("sessions");
        reference.Child(autoComplete.GetReceiverID()).Child(autoComplete.GetSessionID()).Child("inviter_animation").SetValueAsync(animation);



    }





    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        animation_play = args.Snapshot.Value.ToString();
    }


    public string GetAnimation_play()
    {

        return animation_play;
    }

}
