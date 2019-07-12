using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrabUsernames : MonoBehaviour {

 
    Dictionary<string, string> dict = new Dictionary<string, string>();

    public static List<string> m_Options;


    // Use this for initialization
    void Awake () {

        StartCoroutine(ExecuteAterTime(2));

        

    }

    private void OnDestroy()
    {
        StopCoroutine(ExecuteAterTime(0));
    }

    public List<string> GetUserList()
    {
        return m_Options;

    }




    IEnumerator ExecuteAterTime(float time)
    {

        yield return new WaitForSeconds(time);

        FirebaseDatabase.DefaultInstance
 .GetReference("users")
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


    }
}
