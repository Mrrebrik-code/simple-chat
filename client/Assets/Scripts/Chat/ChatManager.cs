using System;
using UnityEngine;

public static class ChatManager
{
	private static Chat _chat = null;

	public static void CreateChat(string nameChat, string passwordChat, Action<bool> callbackCreatedChat)
	{
		Chat.Create(nameChat, passwordChat, (chat, users) =>
		{
			Debug.Log("Chat create successful!");
			callbackCreatedChat?.Invoke(true);
		}, () =>
		{
			_chat = null;

			Debug.Log("Chat create failed!");
			callbackCreatedChat?.Invoke(false);
		});
	}

	public static void JoinChat(string nameChat, string passwordChat, Action<bool, User[]> callbackJoinedChat)
	{
		Chat.Join(nameChat, passwordChat, (chat, users) =>
		{
			_chat = chat;
			_chat.SetUsers(users);

			Debug.Log("Chat join successful!");
			callbackJoinedChat?.Invoke(true, users);
		}, () =>
		{
			_chat = null;

			Debug.Log("Chat join failed!");
			callbackJoinedChat?.Invoke(false, null);
		});
	}

	public static void LeaveChat()
	{
		if (_chat == null) return;

		_chat.Leave();
		_chat = null;
	}

	public static void SendMessageToChat(string messageText, User user)
	{
		var message = new Message(messageText, user);

		_chat?.SendMessage(message);
	}

	public static Chat GetCurrentChat()
	{
		return _chat;
	}

	public static void SubscribeLeaveAndJoin(this Chat chat, Action<User> callbackLeave, Action<User> callbackJoin, Action<Message> callbackMessage)
	{
		if(chat != null)
		{
			chat.onLeaveTargetUser += callbackLeave;
			chat.onJoinTargetUser += callbackJoin;
			chat.onMessageUser += callbackMessage;
		}
	}
}
