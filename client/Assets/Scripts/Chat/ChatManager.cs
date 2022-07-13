using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatManager : SingletonMono<ChatManager>
{
	[SerializeField] private TMP_Text _statusChatText;

	private Chat _chat = null;

	public void CreateChat(string name, string password)
	{
		Chat.Create(name, password, (chat) =>
		{
			_chat = chat;

			Debug.Log("Chat create successful!");
		}, () =>
		{
			_chat = null;

			Debug.Log("Chat create failed!");
		});
	}

	public void JoinChat(string name, string password)
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

	public void LeaveChat()
	{
		_chat.Leave();
		_chat = null;
	}
}
