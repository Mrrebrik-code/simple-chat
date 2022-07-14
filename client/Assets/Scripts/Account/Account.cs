using System;

public class Account
{
	public User User { get; private set; }

	public static Account Create()
	{
		return new Account();
	}

	public void Login(string nickname, string password, Action<string> callbackSuccessful, Action callbackError)
	{
		User = new User(nickname, password, "default");

		NetworkManager.Instance.LoginUserToServer(User, callbackSuccessful, callbackError);
	}

	public void Register(string nickname, string password, Action<string> callbackSuccessful, Action callbackError)
	{
		User = new User(nickname, password, "default");

		NetworkManager.Instance.RegisterUserToServer(User, callbackSuccessful, callbackError);
	}
}
