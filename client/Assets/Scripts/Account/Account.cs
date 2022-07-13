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

		callbackSuccessful += OnHandleSuccesfulUser;
		callbackError += OnHandleFailedUser;

		NetworkManager.Instance.LoginUserToServer(User, callbackSuccessful, callbackError);
	}

	public void Register(string nickname, string password, Action<string> callbackSuccessful, Action callbackError)
	{
		User = new User(nickname, password, "default");

		callbackSuccessful += OnHandleSuccesfulUser;
		callbackError += OnHandleFailedUser;

		NetworkManager.Instance.RegisterUserToServer(User, callbackSuccessful, callbackError);
	}

	private void OnHandleSuccesfulUser(string userId)
	{
		User.SetId(userId);
	}

	private void OnHandleFailedUser()
	{
		User = null;
	}
}
