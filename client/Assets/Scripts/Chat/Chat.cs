using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat 
{
	public string Name { get; private set; }
	public string Password { get; private set; }

	public User[] Users { get; private set; }

	public Action<User> onLeaveTargetUser { get; set; }
	public Action<User> onJoinTargetUser { get; set; }
	public Action<Message> onMessageUser { get; set; }

	public Chat(string name, string password)
	{
		Name = name;
		Password = password;
	}

	~Chat()
	{
		onLeaveTargetUser = null;
		onJoinTargetUser = null;
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
		NetworkManager.Instance.LeaveChatFromServerCurrentUser();
	}

	public void SendMessage(Message message)
	{
		NetworkManager.Instance.SendMessageToChatFromServer(message);
	}

	public void LeaveTargetUser(User user)
	{
		onLeaveTargetUser?.Invoke(user);
	}

	public void JoinTargetUser(User user)
	{
		onJoinTargetUser?.Invoke(user);
	}

	public void MessageTargetUser(Message message)
	{
		onMessageUser?.Invoke(message);
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
