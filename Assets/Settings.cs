using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    public InputField username;

    UIHandler uIHandler;

    string currentuserId;

	// Use this for initialization
	void Start () {

        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        currentuserId = auth.CurrentUser.UserId;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeUserName()
    {

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vizzo-app.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("users");

        reference.Child(currentuserId).Child("username").SetValueAsync(username.text);


        uIHandler.UpdateUserProfileAsync(username.text);



    }
}
