using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection_Invitee : MonoBehaviour
{

    private GameObject[] characterList;

    public static List<string> animations = new List<string>();

    public GameObject buttons;
    public GameObject changecharacter;

    public static string selectedCharcater;

    SceneController sceneController = new SceneController();

    

    private string receiverId;

    private int index;


    private void Start()
    {


       


        characterList = new GameObject[transform.childCount];


        //Fill the array with our models
        for (int i = 0; i < transform.childCount; i++)
        {


            characterList[i] = transform.GetChild(i).gameObject;

        }



        //We toggle off their render
        foreach (GameObject go in characterList)
        {
            go.SetActive(false);

        }


       


        //We toggle on the first index

        if (characterList[index])
        {

            characterList[index].SetActive(true);
        }
        changecharacter.SetActive(false);



       

    }

    public void ToggleLeft()


    {
        //Toggle off the current model 
        characterList[index].SetActive(false);


        index--; //index-=1; index = index -1;


        if (index < 0)
        {
            index = characterList.Length - 1;

        }

        //Toggle on the new model
        characterList[index].SetActive(true);

    }


    public void ToggleRight()


    {
        //Toggle off the current model 
        characterList[index].SetActive(false);


        index++; //index-=1; index = index -1;


        if (index == characterList.Length)
        {
            index = 0;

        }

        //Toggle on the new model
        characterList[index].SetActive(true);

    }

    public void SelectButton()
    {
        PlayerPrefs.SetInt("CharacterSelected", index);
        buttons.SetActive(false);

        changecharacter.SetActive(true);

        selectedCharcater = characterList[index].name.ToString();
        Debug.Log(selectedCharcater);

        PrintAnimations();




    }

    public void ReceiverCharacter()
    {
       
       


        Debug.Log(selectedCharcater);


       


        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vizzo-app.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("sessions");
      reference.Child(sceneController.GetReceiverId()).Child(sceneController.GetSessionID()).Child("invitee_character").SetValueAsync(selectedCharcater);
      reference.Child(sceneController.GetReceiverId()).Child(sceneController.GetSessionID()).Child("status").SetValueAsync("Accepted");
        NextScene();


    }



    public void ChangeCharacter()
    {
        buttons.SetActive(true);
        changecharacter.SetActive(false);
        selectedCharcater = null;

    }




    public string SelectedCharacter()
    {

        return selectedCharcater;
    }

    public GameObject CharactergameObject()
    {
        return characterList[index];

    }
    public int GetIndex()
    {

        return index;
    }

    public void PrintAnimations()
    {


        GameObject character = characterList[index].gameObject;

        Animation anim = character.GetComponent<Animation>();
        foreach (AnimationState state in anim)
        {

            animations.Add(state.name);
            Debug.Log(state.name);
        }


    }


    public List<string> GetAnimation()
    {


        return animations;
    }

    public void NextScene()
    {
        if (selectedCharcater != null)
        {
            SceneManager.LoadScene("ARScene_invitee");
        }




    }

}