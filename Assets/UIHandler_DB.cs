// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// Handler for UI buttons on the scene.  Also performs some
// necessary setup (initializing the firebase app, etc) on
// startup.
public class UIHandler_DB : MonoBehaviour
{

    ArrayList Users = new ArrayList();
  

   

    private const int MaxScores = 5;
    private string logText = "";
    private string email = "";
    private string username = "";
    private string fullname = "";
    private Vector2 scrollViewVector = Vector2.zero;
    protected bool UIEnabled = true;

    const int kMaxLogSize = 16382;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    protected virtual void Start()
    {
        Users.Clear();
        Users.Add("Firebase Top " + MaxScores.ToString() + " Scores");

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    // Initialize the Firebase database:
    protected virtual void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        // NOTE: You'll need to replace this url with your Firebase App's database
        // path in order for the database connection to work correctly in editor.
        app.SetEditorDatabaseUrl("https://vizzo-app.firebaseio.com/");
        if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
        StartListener();
    }

   

    protected void StartListener()
    {
        FirebaseDatabase.DefaultInstance
          .GetReference("Users").OrderByChild("score")
          .ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
          {
              if (e2.DatabaseError != null)
              {
                  Debug.LogError(e2.DatabaseError.Message);
                  return;
              }
              Debug.Log("Received values for Leaders.");
              string title = Users[0].ToString();
              Users.Clear();
              Users.Add(title);
              if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
              {
                  foreach (var childSnapshot in e2.Snapshot.Children)
                  {
                      if (childSnapshot.Child("score") == null
                    || childSnapshot.Child("score").Value == null)
                      {
                          Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                          break;
                      }
                      else
                      {
                          Debug.Log("Leaders entry : " +
                        childSnapshot.Child("email").Value.ToString() + " - " +
                        childSnapshot.Child("score").Value.ToString());
                          Users.Insert(1, childSnapshot.Child("score").Value.ToString()
                        + "  " + childSnapshot.Child("email").Value.ToString());
                      }
                  }
              }
          };
    }

    // Exit if escape (or back, on mobile) is pressed.
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Output text to the debug log text field, as well as the console.
    public void DebugLog(string s)
    {
        Debug.Log(s);
        logText += s + "\n";

        while (logText.Length > kMaxLogSize)
        {
            int index = logText.IndexOf("\n");
            logText = logText.Substring(index + 1);
        }

        scrollViewVector.y = int.MaxValue;
    }

    // A realtime database transaction receives MutableData which can be modified
    // and returns a TransactionResult which is either TransactionResult.Success(data) with
    // modified data or TransactionResult.Abort() which stops the transaction with no changes.
    TransactionResult AddScoreTransaction(MutableData mutableData)
    {
        List<object> users = mutableData.Value as List<object>;

        if (users == null)
        {
            users = new List<object>();
        }
     
        // Now we add the new score as a new entry that contains the email address and score.
        Dictionary<string, object> userinfo = new Dictionary<string, object>();

        userinfo["username"] = username;
        userinfo["email"] = email;
        userinfo["fullname"] = fullname;

         Users.Add(userinfo);

        // You must set the Value to indicate data at that location has changed.
        mutableData.Value = users;
        return TransactionResult.Success(mutableData);
    }

    public void AddScore()
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
        {
            DebugLog("Please an enter an email and/or username.");
            return;
        }
        DebugLog(String.Format("Attempting to add info {0} {1}",
          email, username.ToString()));

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Users");

        DebugLog("Running Transaction...");
        // Use a transaction to ensure that we do not encounter issues with
        // simultaneous updates that otherwise might create more than MaxScores top scores.
        reference.RunTransaction(AddScoreTransaction)
          .ContinueWith(task =>
          {
              if (task.Exception != null)
              {
                  DebugLog(task.Exception.ToString());
              }
              else if (task.IsCompleted)
              {
                  DebugLog("Transaction complete.");
              }
          });
    }

   
}

