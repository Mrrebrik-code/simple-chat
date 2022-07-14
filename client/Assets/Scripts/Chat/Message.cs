using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
