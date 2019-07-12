using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTest : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start () {

        

       Debug.Log(DateTime.Now.ToString("dd/MM/yyy hh:mm:ss tt", System.Globalization.DateTimeFormatInfo.InvariantInfo));

        Debug.Log(DateTime.Now.ToString("dd.MM.yyy"));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
