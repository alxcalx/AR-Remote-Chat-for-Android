

public class sessions 



{

    public string status;
    public string inviter;
    public string time;
    public string inviter_character;
    public string invitee_character;
    public string inviter_animation;
    public string invitee_animation;
    public string inviter_message;
    public string invitee_message;
    


    public sessions()
    {


    }

    public sessions(string inviter, string status, string time, string inviter_character, string invitee_character, string inviter_animation, string invitee_animation, string inviter_message, string invitee_message)
    {
         this.inviter = inviter;

         this.status = status;

         this.time = time;

         this.inviter_character = inviter_character;

         this.invitee_character = invitee_character;

         this.inviter_animation = inviter_animation;

         this.invitee_animation = invitee_animation;

         this.inviter_message = inviter_message;

         this.invitee_message = invitee_message;
    }


}