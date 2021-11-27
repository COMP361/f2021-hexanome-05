public delegate void TestAPISuccess(string responseText);
public delegate void TestAPIFailure(string error);
public delegate void LoginSuccess(string responseText);
public delegate void LoginFailure(string error);

public interface ClientInterface
{
    event LoginSuccess LoginSuccessEvent;
    event LoginFailure LoginFailureEvent;
    
    void Login(string username, string password);
    
}

