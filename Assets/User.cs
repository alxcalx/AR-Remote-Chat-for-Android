

public class User 
{
   
    public string memail;
    public string mfullname;
    public string username;
    public string instanceid;
  

    public User()
    {


    }

    public User(string mfullname, string memail, string username, string instanceid )
    {

        this.mfullname = mfullname;
        
        this.memail = memail;

        this.username = username;

        this.instanceid = instanceid;

     
    }


}