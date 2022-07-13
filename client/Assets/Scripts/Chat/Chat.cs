using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat 
{
	public string Name { get; private set; }
	public string Password { get; private set; }

	public User[] Users { get; private set; }

	public Chat(string name, string password)
	{
		Name = name;
		Password = password;
	}

	public static void Create(string name, string password, Action<Chat, User[]> callbackSuccessful, Action callbackError)
	{
		var chat = CreateObjectChat(name, password);

		NetworkManager.Instance.CreateChatToServer(chat, callbackSuccessful, callbackError);
	}

	public static void Join(string name, string password, Action<Chat, User[]> callbackSuccessful, Action callbackError)
	{
		var chat = CreateObjectChat(name, password);

		NetworkManager.Instance.JoinChatFromServer(chat, callbackSuccessful, callbackError);
	}

	public void Leave()
	{

	}

	private static Chat CreateObjectChat(string name, string password)
	{
		var chat = new Chat(name, password);

		return chat;
	}

	public void SetUsers(User[] users)
	{
		Users = users;
	}
}
