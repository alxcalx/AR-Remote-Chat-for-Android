using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {



    Animation anim;
    Animator animator;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animation>();
       
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {

            anim.Play("Capoeira");
            

            

        }
		
	}
}
