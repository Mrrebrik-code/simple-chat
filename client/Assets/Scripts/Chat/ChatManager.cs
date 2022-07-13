using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class ChatManager
{
	private static Chat _chat = null;

	public static void CreateChat(string name, string password, Action<bool> callback)
	{
		Chat.Create(name, password, (chat) =>
		{
			_chat = chat;

			Debug.Log("Chat create successful!");
			callback?.Invoke(true);
		}, () =>
		{
			_chat = null;

			Debug.Log("Chat create failed!");
			callback?.Invoke(false);
		});
	}

	public static void JoinChat(string name, string password)
	{
		Chat.Join(name, password, (chat) =>
		{
			_chat = chat;

			Debug.Log("Chat join successful!");
		}, () =>
		{
			_chat = null;

			Debug.Log("Chat join failed!");
		});
	}

	public static void LeaveChat()
	{
		_chat.Leave();
		_chat = null;
	}
}
