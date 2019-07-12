using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionInformation : MonoBehaviour
{


   
    string sessionID;
    string senderName;
    string receiverId;
    public Text username;
    public static string character;
    public GameObject[] characterList;


    // Use this for initialization
    void Start()
    {
        SceneController sceneController = new SceneController();

        sessionID = sceneController.GetSessionID();
        senderName = sceneController.GetSenderName();
        receiverId = sceneController.GetReceiverId();

        FirebaseDatabase.DefaultInstance
           .GetReference("sessions").Child(receiverId).Child(sessionID).Child("inviter_character")
           .ValueChanged += HandleValueChanged;

        username.text = senderName;
    }






    // Update is called once per frame
    void Update()
    {



    }


    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        character = args.Snapshot.ToString();
        Debug.Log(character);



    }


    public int GetCharacter()
    {
        characterList = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {


            characterList[i] = transform.GetChild(i).gameObject;


        }

        int characterindex = Array.IndexOf(characterList, character);


        return characterindex;

    }

    public string GetCharacterName()
    {

        return character;
    }

}
