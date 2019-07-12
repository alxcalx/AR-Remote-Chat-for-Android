using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Invites;


public class Invites : MonoBehaviour {

    

    // Use this for initialization
    void Start () {

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp, i.e.
                //   app = Firebase.FirebaseApp.DefaultInstance;
                // where app is a Firebase.FirebaseApp property of your application class.

                // Set a flag here indicating that Firebase is ready to use by your
                // application.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SendInvite()
    {
        Debug.Log("Sending an invitation...");
        var invite = new Firebase.Invites.Invite()
        {
            TitleText = "Vizzo",
            MessageText = "Try this app! It's awesome.",
            CallToActionText = "Download it for FREE",
         //   DeepLinkUrl = "http://google.com/abc"
        };

     FirebaseInvites
    .SendInviteAsync(invite).ContinueWith(HandleSentInvite);
    }

    void HandleSentInvite(Task<Firebase.Invites.SendInviteResult> sendTask)
    {
        if (sendTask.IsCanceled)
        {
            Debug.Log("Invitation canceled.");
        }
        else if (sendTask.IsFaulted)
        {
            Debug.Log("Invitation encountered an error:");
            Debug.Log(sendTask.Exception.ToString());
        }
        else if (sendTask.IsCompleted)
        {
          //  int inviteCount = (new List(sendTask.Result.InvitationIds)).Count;
       //     Debug.Log("SendInvite: " + inviteCount + " invites sent successfully.");
            foreach (string id in sendTask.Result.InvitationIds)
            {
                Debug.Log("SendInvite: Invite code: " + id);
            }
        }
    }
}
