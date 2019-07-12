using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour {

	// Use this for initialization
	void Start () {

        PlayerPrefs.Save();
        PlayerPrefs.SetInt("Tutorial", 1);
    }
	
	
}
