using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AnimationList : MonoBehaviour {


    public Dropdown dropdown;
    public TMP_Text TMP_ChatOutput;
    public Scrollbar ChatScrollbar;
    public static string selectedAnimation_;
   
    

    List<string> animations = new List<string>();
    // Use this for initialization
    void Start() {


        if(SceneManager.GetActiveScene().name == "ARSCene")
        {
            PopulateList_Inviter();
            Debug.Log("Animations for Inviter");
        }
        else
        {
            PopulateList_Invitee();
            Debug.Log("Animations for Invitee");


        }

       

        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });




    }

    // Update is called once per frame
    void Update() {




        }

        public void PopulateList_Inviter()
        {
            CharacterSelection characterSelection = new CharacterSelection();
            animations = characterSelection.GetAnimation();
            dropdown.ClearOptions();
            dropdown.AddOptions(animations);

         


        }


       public void PopulateList_Invitee()
    {

        CharacterSelection_Invitee characterSelection_invitee = new CharacterSelection_Invitee();
        animations = characterSelection_invitee.GetAnimation();
        dropdown.ClearOptions();
        dropdown.AddOptions(animations);

    }


       
    public void DropdownValueChanged(Dropdown change)
    {
        
       CharacterPreview characterPreview = new CharacterPreview();
      
        
        selectedAnimation_ =  dropdown.options[dropdown.value].text.ToString();
        Debug.Log(dropdown.options[dropdown.value].text.ToString());

    


        characterPreview.PlayAnimation(selectedAnimation_);


        AddToScrollView("Your Selected Animation:" + " " + selectedAnimation_);

       
      
       
        
    }


    public void SendAnimation()
    {
        Animations_Inviter animations_Inviter = new Animations_Inviter();
        Animations_Invitee animations_Invitee = new Animations_Invitee();


        if (SceneManager.GetActiveScene().name == "ARSCene")
        {

            animations_Inviter.StoreAnimation(selectedAnimation_);

        }
        else
        {

            animations_Invitee.StoreAnimation(selectedAnimation_);
        }


    }


    public string GetAnimation()
        {

            return selectedAnimation_;
        }


    public void AddToScrollView(string newText)
    {

        var timeNow = System.DateTime.Now;

        TMP_ChatOutput.text += "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2") + "</color>] " + newText + "\n";

        // Set the scrollbar to the bottom when next text is submitted.
        ChatScrollbar.value = 0;

    }


}

