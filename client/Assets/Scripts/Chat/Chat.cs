using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat 
{
	public string Name { get; private set; }
	public string Password { get; private set; }

	public static void Create(string name, string password, Action<Chat> callbackSuccessful, Action callbackError)
	{
		var chat = CreateObjectChat(name, password);

		NetworkManager.Instance.CreateChatToServer(chat, callbackSuccessful, callbackError);
	}

	public static void Join(string name, string password, Action<Chat> callbackSuccessful, Action callbackError)
	{
		var chat = CreateObjectChat(name, password);


	}

	public void Leave()
	{

	}

	private static Chat CreateObjectChat(string name, string password)
	{
		var chat = new Chat();
		chat.Name = name;
		chat.Password = password;

		return chat;
	}
}
