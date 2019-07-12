using System.Collections.Generic;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System.Linq;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Timers;

public class AutoCompleteScrollRect : MonoBehaviour
{

   
    // References
    [SerializeField] InputField m_InputField;
    [SerializeField] ScrollRect m_ScrollRect;
    private RectTransform m_ScrollRectTransform;

    // Prefab for available options
    [SerializeField] OptionButton m_OptionPrefab;

    // The transform (parent) to spawn options into
    [SerializeField] Transform m_OptionsParent = null;

    public GameObject connectbutton;
    public GameObject CharacterList;
    public GameObject findYourfriend_help;
    public GameObject connectwithuser_help;
 

    Timer t;

    string result;
    public static string opt_;
    public static string opt_value;
    public static string key;

   
 
    

    [SerializeField] List<string> m_Options;
    

    bool Dialog;
    bool optDialog;
    bool updateOn;
    bool rejected;
    bool timeout;

    private Dictionary<string, GameObject> m_OptionObjectSpawnedDict;       // Store a list of options in a dictionary, essentially object pooling the buttons
    Dictionary<string, string> dict = new Dictionary<string, string>();
    private float m_OriginalOffsetMinY;

    [SerializeField] int m_ComponentsHeight = 50;                           // The size of the option buttons
    [SerializeField] int m_OptionsOnDisplay = 3;                            // How many options the scrollview can display at one time

   
    List<string> animations;

  

   
   

    private void Start()
    {
        DatabaseReference databaseReference = FirebaseDatabase.DefaultInstance.GetReference("users");

        databaseReference
       .ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
       {
           if (e2.DatabaseError != null)
           {
               Debug.LogError(e2.DatabaseError.Message);
               return;
           }
           // Debug.Log("Received user list.");



           if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
           {
               foreach (var childSnapshot in e2.Snapshot.Children)
               {


                   dict.Add(childSnapshot.Child("username").Value.ToString(), childSnapshot.Key);


                   m_Options = dict.Keys.ToList<string>();




               }


           }
       };









        m_OriginalOffsetMinY = m_ScrollRect.gameObject.GetComponent<RectTransform>().offsetMin.y;

        m_ScrollRectTransform = m_ScrollRect.gameObject.GetComponent<RectTransform>();

        m_ScrollRect.gameObject.SetActive(false);   // By default, we don't need to show the scroll view.







       





    }






  private void ChangeScene()
    {
        
        SceneManager.LoadScene("ARScene");
       
    }




      
    


  





    protected void Update()
    {

    

      
       



    }



    public void SetAndCloseScrollView(string optionLabel)
    {
        m_InputField.text = optionLabel;
        m_ScrollRect.gameObject.SetActive(false);
    }

    /// <summary>
    /// Spawns a list of all the available options into the scene, deactivates them, and adds them to the pool
    /// </summary>
    /// <param name="options"></param>
    /// 


    private void ReturnString(string word)
    {

        opt_ = word;

    }

    private void SpawnClickableOptions(List<string> options)
    {
        ResetDictionaryAndCleanupSceneObjects();

        if (m_Options == null || m_Options.Count == 0)
        {
            Debug.LogError("Options lists is null or the list is == 0, please ensure it has something in it!");
            return;
        }

        for (int i = 0; i < m_Options.Count; i++)
        {
            GameObject obj = Instantiate(m_OptionPrefab.gameObject, m_OptionsParent);
            obj.transform.localScale = Vector3.one;

            m_OptionObjectSpawnedDict.Add(m_Options[i], obj);

            string opt = m_Options[i];
           


            obj.GetComponent<OptionButton>().Setup(m_Options[i], m_ComponentsHeight, () =>
            {

                if(SceneManager.GetActiveScene().name == "Main Scene_Help")
                {
                    findYourfriend_help.SetActive(false);
                    connectwithuser_help.SetActive(true);

                }
                
                ReturnString(opt);
                Debug.Log("Clicked option " + opt);
                SetAndCloseScrollView(opt);


            });
        }


    }

    /// <summary>
    /// Cleans up the scrollview
    /// </summary>
    private void ResetDictionaryAndCleanupSceneObjects()
    {
        if (m_OptionObjectSpawnedDict == null)
        {
            m_OptionObjectSpawnedDict = new Dictionary<string, GameObject>();
            return;
        }

        if (m_OptionObjectSpawnedDict.Count == 0)
            return;

        foreach (KeyValuePair<string, GameObject> options in m_OptionObjectSpawnedDict)
            Destroy(options.Value);

        m_OptionObjectSpawnedDict.Clear();
    }

    /// <summary>
    /// Hooked up to the OnValueChanged() event of the inputfield specified, we listen out for changes within the input field.
    /// When the input.text has changed, we search the options dictionary and attempt to find matches, and display them if any.
    /// </summary>
    public void OnValueChanged()
    {
        if (m_InputField.text == "")
        {
            m_ScrollRect.gameObject.SetActive(false);       // Disable the scrollview if the inputfield is empty
            return;
        }
        else
        {

            SpawnClickableOptions(m_Options);
        }


        List<string> optionsThatMatched = m_OptionObjectSpawnedDict.Keys.
           Where(optionKey => optionKey.ToLower().Contains(m_InputField.text.ToLower())).ToList();

        foreach (KeyValuePair<string, GameObject> keyValuePair in m_OptionObjectSpawnedDict)
        {
            if (optionsThatMatched.Contains(keyValuePair.Key))
                keyValuePair.Value.SetActive(true);
            else
                keyValuePair.Value.SetActive(false);
        }

        if (optionsThatMatched.Count == 0)
        {
            m_ScrollRect.gameObject.SetActive(false);        // Disable the scrollview if no options
            return;
        }


        // If options is > than the amount of options we can display
        if (optionsThatMatched.Count > m_OptionsOnDisplay)
        {
            // Then scale the height of the rect transform to only show the max amount of items we can show at one time
            m_ScrollRectTransform.offsetMin = new Vector2(
                       m_ScrollRect.GetComponent<RectTransform>().offsetMin.x,
                         m_OriginalOffsetMinY - (m_ComponentsHeight * m_OptionsOnDisplay));

        }
        else
        {
            // Else... just increase the height of the rect transform to display all of options that matched
            m_ScrollRectTransform.offsetMin = new Vector2(
                      m_ScrollRect.GetComponent<RectTransform>().offsetMin.x,
                        m_OriginalOffsetMinY - (m_ComponentsHeight * optionsThatMatched.Count));
        }

        m_ScrollRect.gameObject.SetActive(true);            // If we get here, we can assume that we want to display the options.
    }


    private void CreateInvitation(string inviter, string status, string time, string inviter_character)
    {
        sessions sessions = new sessions(inviter, status, time, inviter_character, null, null, null, null, null);
        string json = JsonUtility.ToJson(sessions);

                                         

        dict.TryGetValue(opt_, out opt_value);           //Gets the userId associated with the username

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vizzo-app.firebaseio.com/");

        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("sessions");
        key = reference.Push().Key;
        reference.Child(opt_value).Child(key).SetRawJsonValueAsync(json);


    }

    public string GetReceiverID()
    {
        return opt_value;

    }


    public string GetSessionID()
    {
        return key;

    }

    public string GetReceiverName()
    {
        return opt_;



    }



    public void DisplayButton()
    {

        int boxheight = 400;
        int boxwidth = 1000;
        int buttonheight = 150;
        int buttonwidth = 300;





       if (optDialog)
        {

           

            GUI.Box(new Rect((Screen.width - boxwidth) / 2, Screen.height / 15, boxwidth, boxheight), "Don't forget to select a friend");

         
            if (GUI.Button(new Rect((Screen.width - buttonwidth) / 2, Screen.height / 15 + buttonheight, buttonwidth, buttonheight), "Okay"))
            {
                optDialog = false;

            }

            


        }






        if (Dialog)
        {



            GUI.Box(new Rect((Screen.width - boxwidth) / 2, Screen.height / 15, boxwidth, boxheight), "Don't forget to select your character");

         
            if (GUI.Button(new Rect((Screen.width - buttonwidth) / 2, Screen.height / 15 + buttonheight, buttonwidth, buttonheight), "Okay"))
            {
                Dialog = false;

            }

    


        }




    }


    public void OnGUI()
    {
        DisplayButton();

    }

    public void Connect()
    {
        InstanceIDUpdate instanceIDUpdate = new InstanceIDUpdate();

        
        CharacterSelection characterSelection = new CharacterSelection();
        string selectedCharcater = characterSelection.SelectedCharacter();




        if (String.IsNullOrEmpty(selectedCharcater))
        {
            Dialog = true;

        }
        else
        {

            if (String.IsNullOrEmpty(opt_))
            {
                optDialog = true;
                Debug.Log(opt_);
            }
            else
            {
                
                string date = DateTime.Now.ToString("dd/MM/yyy hh:mm:ss tt", System.Globalization.DateTimeFormatInfo.InvariantInfo);

                CreateInvitation(instanceIDUpdate.GetUserId(), "Waiting for Response", date, selectedCharcater);

                connectbutton.SetActive(false);
                CharacterList.SetActive(false);
                ChangeScene();

                

             


            }



        }

        

            }



    public void LogOut()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignOut();
        SceneManager.LoadScene("Aunthentication");

    }










}

