using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {


    public GameObject canvas1;

    public GameObject canvas2;

   



    private int index;

    

	// Use this for initialization
	void Start () {

  

        


    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) { Back(); }
    }






    public void GetStarted()
    {
        canvas1.SetActive(true);

        canvas2.SetActive(false);



    }

    private void Back()
    {
        canvas1.SetActive(false);

        canvas2.SetActive(true);


    }

    public void GotItButton()
    {
        SceneManager.LoadScene("Aunthentication");

    }

}
