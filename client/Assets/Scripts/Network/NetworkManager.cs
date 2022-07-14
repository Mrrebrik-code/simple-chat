using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO3;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class NetworkManager : SingletonMono<NetworkManager>
{
	private SocketManager _socketManager = null;

	[SerializeField] private string _address = "http://localhost:52300";
	[SerializeField] private bool _isReconection = false;
	[SerializeField] private bool _isAutoConnect = true;
	private bool _isConnected = false;

	private Action<string> onRegisterSuccessful;
	private Action onRegisterError;

	private Action<string> onLoginSuccessful;
	private Action onLoginError;

	private Action<Chat, User[]> onChatCreateSuccessful;
	private Action onChatCreateError;

	private Action<Chat, User[]> onChatJoinSuccessful;
	private Action onChatJoinError;


	//Inovke to button - "Connect"
	public void ConnectedToServer()
	{
		if(_socketManager == null || _isConnected == false)
		{
			SocketOptions socketOptions = new SocketOptions()
			{
				AutoConnect = _isAutoConnect,
				Reconnection = _isReconection
			};

			_socketManager = new SocketManager(new System.Uri(_address), socketOptions);

			_socketManager.Socket.On(SocketIOEventTypes.Connect, OnConnectToServer);
			_socketManager.Socket.On(SocketIOEventTypes.Disconnect, OnDisconnectToServer);
		}
	}

	//Inovke to button - "Disconnect"
	public void DisconnectFromServer()
	{
		if(_socketManager != null && _isConnected == true)
		{
			_socketManager.Close();
			_isConnected = false;
			_socketManager = null;
		}
	}

	private void SubscribeSocketIOEvents()
	{
		if (_socketManager == null || _isConnected == false)
		{
			Debug.LogError("Socket event subscription failed due to null or false connection to SocketManager server!");
			return;
		}

		_socketManager.Socket.On<string>(OnIOEvent.LoginUser, OnLoginUserToServer);
		_socketManager.Socket.On<string>(OnIOEvent.RegisterUser, OnRegisterUserToServer);

		_socketManager.Socket.On<string>(OnIOEvent.CreateChat, OnCreateChatToServer); //Create chat
		_socketManager.Socket.On<string>(OnIOEvent.JoinChat, OnJoinChatToServer); //Join chat

		_socketManager.Socket.On<string>(OnIOEvent.LeaveChatTargetUser, OnLeaveTargetUserChatToServer); //Some leave chat
		_socketManager.Socket.On<string>(OnIOEvent.JoinChatTargetUser, OnJoinTargetUserChatToServer); //Some join chat

		_socketManager.Socket.On<string>(OnIOEvent.SendMessageToChat, OnMessageTargetUserChatToServer);
	}



	public void RegisterUserToServer(User user, Action<string> callbackSuccessful, Action callbackError)
	{
		if (_isConnected == false) return;

		onRegisterSuccessful += callbackSuccessful;
		onRegisterError += callbackError;

		var jsonSerializerSettings = new JsonSerializerSettings()
		{
			Formatting = Formatting.Indented
		};

		var json = JsonConvert.SerializeObject(user, jsonSerializerSettings);

		Debug.Log(json);

		_socketManager.Socket.Emit(EmitIOEvent.RegisterAccountUser, json);
	}

	public void LoginUserToServer(User user, Action<string> callbackSuccessful, Action callbackError)
	{
		if (_isConnected == false) return;

		onLoginSuccessful += callbackSuccessful;
		onLoginError += callbackError;

		var jsonSerializerSettings = new JsonSerializerSettings()
		{
			Formatting = Formatting.Indented
		};

		var json = JsonConvert.SerializeObject(user, jsonSerializerSettings);

		Debug.Log(json);

		_socketManager.Socket.Emit(EmitIOEvent.LoginAccountUser, json);
	}

	public void CreateChatToServer(Chat chat, Action<Chat, User[]> callbackSuccessful, Action callbackError)
	{
		if (_isConnected == false) return;

		onChatCreateSuccessful += callbackSuccessful;
		onChatCreateError += callbackError;

		var jsonSerializerSettings = new JsonSerializerSettings()
		{
			Formatting = Formatting.Indented
		};

		var json = JsonConvert.SerializeObject(chat, jsonSerializerSettings);

		Debug.Log(json);

		_socketManager.Socket.Emit(EmitIOEvent.CreateChat, json);
	}

	public void JoinChatFromServer(Chat chat, Action<Chat, User[]> callbackSuccessful, Action callbackError)
	{
		if (_isConnected == false) return;

		onChatJoinSuccessful += callbackSuccessful;
		onChatJoinError += callbackError;

		var jsonSerializerSettings = new JsonSerializerSettings()
		{
			Formatting = Formatting.Indented
		};

		var json = JsonConvert.SerializeObject(chat, jsonSerializerSettings);

		Debug.Log(json);


		_socketManager.Socket.Emit(EmitIOEvent.JoinChat, json);
	}

	public void LeaveChatFromServerCurrentUser()
	{
		if (_isConnected == false) return;

		_socketManager.Socket.Emit(EmitIOEvent.LeaveChatCurrentUser);
	}

	public void SendMessageToChatFromServer(Message message)
	{
		// TODO: sennding message to server
	}


	//Events On Scoket IO
	private void OnCreateChatToServer(string data)
	{
		Debug.Log("OnCreateChatToServer");
		Debug.Log(data);

		var chat = JsonConvert.DeserializeObject<Chat>(data);

		if (chat != null) onChatCreateSuccessful?.Invoke(chat, null);
		else onChatCreateError?.Invoke();

		onChatCreateSuccessful = null;
		onChatCreateError = null;
	}
	private void OnJoinChatToServer(string data)
	{
		Debug.Log("OnJoinChatToServer");
		Debug.Log(data);

		List<User> users = new List<User>();

		var chatData = JObject.Parse(data);

		if (chatData.ContainsKey("users"))
		{
			var JObjectUserT = JArray.Parse(chatData["users"].ToString());
			Debug.Log(JObjectUserT.ToString());

			foreach (var userData in JObjectUserT)
			{
				var user = new User(userData["name"].ToString(), userData["id"].ToString());
				users.Add(user);
			}
		}
		
		
		var chat = new Chat(chatData["name"].ToString(), chatData["password"].ToString());

		if (chat != null) onChatJoinSuccessful?.Invoke(chat, users.ToArray());
		else onChatJoinError?.Invoke();

		onChatJoinSuccessful = null;
		onChatJoinError = null;
	}

	private void OnLeaveTargetUserChatToServer(string data)
	{
		Debug.Log("OnLeaveTargetUserChatToServer");
		Debug.Log(data);

		var userData = JObject.Parse(data);

		var user = new User(userData["nickname"].ToString(), userData["id"].ToString());

		var chat = ChatManager.GetCurrentChat();

		if(chat != null)
		{
			chat.LeaveTargetUser(user);
		}
	}

	private void OnJoinTargetUserChatToServer(string data)
	{
		Debug.Log("OnJoinTargetUserChatToServer");
		Debug.Log(data);

		var userData = JObject.Parse(data);

		var user = new User(userData["nickname"].ToString(), userData["id"].ToString());

		var chat = ChatManager.GetCurrentChat();

		if (chat != null)
		{
			chat.JoinTargetUser(user);
		}
	}

	private void OnMessageTargetUserChatToServer(string data)
	{
		// TODO callback message from server target user to chat
	}

	private void OnRegisterUserToServer(string data)
	{
		Debug.Log("OnRegisterUserToServer");
		Debug.Log(data);

		var user = JsonConvert.DeserializeObject<User>(data);

		if(user != null) onRegisterSuccessful?.Invoke(user.Id);
		else onRegisterError?.Invoke();

		onRegisterSuccessful = null;
		onRegisterError = null;
	}

	private void OnLoginUserToServer(string data)
	{
		Debug.Log("OnLoginUserToServer");
		Debug.Log(data);

		var user = JsonConvert.DeserializeObject<User>(data);

		if (user != null) onLoginSuccessful?.Invoke(user.Id);
		else onLoginError?.Invoke();

		onLoginSuccessful = null;
		onLoginError = null;
	}

	private void OnConnectToServer()
	{
		Debug.Log("Connection to server!");
		_isConnected = true;
		SubscribeSocketIOEvents();

		AccountManager.Open();
	}

	private void OnDisconnectToServer()
	{
		Debug.Log("Disconnect to server!");
		_isConnected = false;
		_socketManager = null;

		AccountManager.Close();
	}
}
