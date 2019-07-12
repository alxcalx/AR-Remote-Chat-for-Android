using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPreview : MonoBehaviour {

    public static GameObject[] characterList;

    public static int index;


    // Use this for initialization
    void Start () {

        index = PlayerPrefs.GetInt("CharacterSelected");

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


        if (characterList[index])
        {

            characterList[index].SetActive(true);
        }


        


    }
	
	// Update is called once per frame
	void Update () {
		
        
	}


    public void PlayAnimation(string animation)
    {
       

      

        Animation anim = characterList[index].gameObject.GetComponent<Animation>();

        anim.Play(animation);

    }
}
