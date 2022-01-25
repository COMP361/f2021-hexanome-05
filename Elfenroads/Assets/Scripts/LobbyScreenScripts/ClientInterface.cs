using System.Threading.Tasks;

public delegate void LoginSuccess(string responseText);
public delegate void LoginFailure(string error);
public delegate void RoleSuccess(string responseText);
public delegate void RoleFailure(string error);
public delegate void RefreshSuccess(string responseText);
public delegate void RefreshFailure(string error);
public delegate Task CreateSuccess(string responseText);
public delegate void CreateFailure(string error);

public delegate Task JoinSuccess(string responseText); //Joining doesn't seem to return anything... But will at least try to use this to test whether or not it works.
public delegate void JoinFailure(string error);
public delegate void LaunchSuccess(string responseText);
public delegate void LaunchFailure(string error);
public delegate Task DeleteSuccess(string responseText);
public delegate void DeleteFailure(string error);

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
    event DeleteSuccess DeleteSuccessEvent;
    event DeleteFailure DeleteFailureEvent;

    void Login(string username, string password);
    
}

