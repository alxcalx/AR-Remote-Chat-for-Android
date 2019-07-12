using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class Animations_Invitee : MonoBehaviour
{

  
    SceneController sceneController = new SceneController();
    AnimationList animationList = null;
    string animation_play;

    // Use this for initialization
    void Start()
    {
       



        FirebaseDatabase.DefaultInstance
          .GetReference("sessions").Child(sceneController.GetReceiverId()).Child(sceneController.GetSessionID()).Child("inviter_animation")
          .ValueChanged += HandleValueChanged;

    }





    public void StoreAnimation(string animation)
    {


        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vizzo-app.firebaseio.com/");

      // Get the root reference location of the database.
      DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("sessions");
      reference.Child(sceneController.GetReceiverId()).Child(sceneController.GetSessionID()).Child("invitee_animation").SetValueAsync(animation);



    }


    public void ReceiveAnimation()
    {


        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vizzo-app.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("sessions");
        reference.Child(sceneController.GetReceiverId()).Child(sceneController.GetSessionID()).Child("invitee_animation").SetValueAsync(animationList.GetAnimation());





    }


    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        animation_play = args.Snapshot.ToString();
    }


    public string GetAnimation_play()
    {

        return animation_play;
    }

}