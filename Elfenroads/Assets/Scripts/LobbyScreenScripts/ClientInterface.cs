public delegate void LoginSuccess(string responseText);
public delegate void LoginFailure(string error);
public delegate void RoleSuccess(string responseText);
public delegate void RoleFailure(string error);
public delegate void RegisterSuccess(string responseText);
public delegate void RegisterFailure(string error);
public delegate void CreateSuccess(string responseText);
public delegate void CreateFailure(string error);

public interface ClientInterface
{
    event LoginSuccess LoginSuccessEvent;
    event LoginFailure LoginFailureEvent;
    event RoleSuccess RoleSuccessEvent;
    event RoleFailure RoleFailureEvent;
    event RegisterSuccess RegisterSuccessEvent;
    event RegisterFailure RegisterFailureEvent;
    event CreateSuccess CreateSuccessEvent;
    event CreateFailure CreateFailureEvent;
    
    void Login(string username, string password);
    
}

