[System.Serializable]
public class User
{
	public string Id { get; private set; }
	public string Nickname { get; private set; }
	public string Password { get; private set; }


	public User(string nickname, string password, string id)
	{
		Nickname = nickname;
		Password = password;
		Id = id;
	}

	public User(string nickname, string id)
	{
		Id = id;
		Nickname = nickname;
	}

	public void SetId(string userId)
	{
		Id = userId;
	}
}

