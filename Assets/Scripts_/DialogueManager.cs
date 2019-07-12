using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    
    bool showDialog;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnGUI()
    {
        DisplayButton();

    }


    public void DisplayButton()
    {

        if (showDialog) { 
       
            GUI.BeginGroup(new Rect(Screen.width / 2 - 150, 50, 300, 250));

            GUI.Box(new Rect(0, 0, 300, 250), "");

            GUI.Label(new Rect(15, 10, 300, 68), "Please fill in the empty fields");

            if (GUI.Button(new Rect(55,150,180,40), "Okay"))
            {
                showDialog = false;

            }

            GUI.EndGroup();


        }

    }

    

    }




