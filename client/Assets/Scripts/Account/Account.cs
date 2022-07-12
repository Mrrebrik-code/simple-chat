public class Account
{
	public User User { get; private set; }

	public static Account Create()
	{
		return new Account();
	}

	public void Login()
	{

	}

	public void Register(string nickname, string password)
	{
	}
}
