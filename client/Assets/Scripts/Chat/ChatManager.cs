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
		Chat.Create(name, password, (chat, users) =>
		{
			Debug.Log("Chat create successful!");
			callback?.Invoke(true);
		}, () =>
		{
			_chat = null;

			Debug.Log("Chat create failed!");
			callback?.Invoke(false);
		});
	}

	public static void JoinChat(string name, string password, Action<bool, User[]> callback)
	{
		Chat.Join(name, password, (chat, users) =>
		{
			_chat = chat;
			_chat.SetUsers(users);

			Debug.LogError(_chat.Name);
			Debug.LogError(_chat.Users.Length);

			Debug.Log("Chat join successful!");
			callback?.Invoke(true, users);
		}, () =>
		{
			_chat = null;

			Debug.Log("Chat join failed!");
			callback?.Invoke(false, null);
		});
	}

	public static void LeaveChat()
	{
		if (_chat == null) return;

		_chat.Leave();
		_chat = null;
	}

	public static Chat GetCurrentChat()
	{
		return _chat;
	}

	public static void SubscribeLeaveAndJoin(this Chat chat, Action<User> callbackLeave, Action<User> callbackJoin)
	{
		if(chat != null)
		{
			chat.onLeaveTargetUser += callbackLeave;
			chat.onJoinTargetUser += callbackJoin;
		}
	}



}
