public delegate void LoginSuccess(string responseText);
public delegate void LoginFailure(string error);
public delegate void RoleSuccess(string responseText);
public delegate void RoleFailure(string error);
public delegate void RefreshSuccess(string responseText);
public delegate void RefreshFailure(string error);
public delegate void CreateSuccess(string responseText);
public delegate void CreateFailure(string error);

public delegate void JoinSuccess(string responseText); //Joining doesn't seem to return anything... But will at least try to use this to test whether or not it works.
public delegate void JoinFailure(string error);
public delegate void LaunchSuccess(string responseText);
public delegate void LaunchFailure(string error);

public interface ClientInterface
{
    event LoginSuccess LoginSuccessEvent;
    event LoginFailure LoginFailureEvent;
    event RoleSuccess RoleSuccessEvent;
    event RoleFailure RoleFailureEvent;
    event RefreshSuccess RefreshSuccessEvent;
    event RefreshFailure RefreshFailureEvent;
    event CreateSuccess CreateSuccessEvent;
    event CreateFailure CreateFailureEvent;
    event JoinSuccess JoinSuccessEvent;
    event JoinFailure JoinFailureEvent;
    event LaunchSuccess LaunchSuccessEvent;
    event LaunchFailure LaunchFailureEvent;
    
    void Login(string username, string password);
    
}

