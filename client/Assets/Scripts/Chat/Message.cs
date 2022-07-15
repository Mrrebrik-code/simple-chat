using System;

[Serializable]
public class Message
{
	public string Text { get; private set; }
	public User User { get; private set; }

	public Message(string text, User user)
	{
		Text = text;
		User = user;
	}
}
