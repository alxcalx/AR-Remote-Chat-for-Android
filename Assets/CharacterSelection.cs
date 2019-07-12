using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

    private GameObject[] characterList;

    public static List<string> animations = new List <string>();

    public GameObject buttons;
    public GameObject changecharacter;

    public static string selectedCharcater;

    public GameObject switchcharacter_help;
    public GameObject selectCharcacter_help;
    public GameObject findyourfriend_help;


    SceneController SceneController;




    private int index ;
  

    private void Start()
    {
       

        

        characterList = new GameObject[transform.childCount];
       
        
        //Fill the array with our models
        for (int i= 0; i < transform.childCount; i++)
        {
            

            characterList[i] = transform.GetChild(i).gameObject;
           
        }

        

        //We toggle off their render
        foreach(GameObject go in characterList)
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

        switchcharacter_help.SetActive(false);
        selectCharcacter_help.SetActive(true);
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

        if (SceneManager.GetActiveScene().name == "Main Scene_Help")
        {

            switchcharacter_help.SetActive(false);
            selectCharcacter_help.SetActive(true);
        }
    }

    public void SelectButton()
    {
        PlayerPrefs.SetInt("CharacterSelected", index);
        buttons.SetActive(false);

        changecharacter.SetActive(true);

        selectedCharcater = characterList[index].name.ToString();
        Debug.Log(selectedCharcater);

       PrintAnimations();

        if (SceneManager.GetActiveScene().name == "Main Scene_Help")
        {

            findyourfriend_help.SetActive(true);
            selectCharcacter_help.SetActive(false);

        }


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
        if(selectedCharcater != null)
        {
            SceneManager.LoadScene("ARScene_invitee");
        }
      
       
        

    }

}
