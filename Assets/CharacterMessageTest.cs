using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMessageTest : MonoBehaviour {


    public GameObject prefab;
    // Use this for initialization
    void Start()
    {
        string messagetext = "hello";

        prefab.transform.Find("BigVegas").Find("Message").GetComponent<TextMesh>().text = messagetext;


        
    }
	// Update is called once per frame


	


}
