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

		_socketManager.Socket.On<string>(OnIOEvent.CreateChat, OnCreateChatToServer);
		_socketManager.Socket.On<string>(OnIOEvent.JoinChat, OnJoinChatToServer);
	}

	

	public void RegisterUserToServer(User user, Action<string> callbackSuccessful, Action callbackError)
	{
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
		_socketManager.Socket.Emit(EmitIOEvent.LeaveChatCurrentUser);
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
			Debug.Log("AASDDD");
			var JObjectUserT = JArray.Parse(chatData["users"].ToString());
			Debug.Log(JObjectUserT.ToString());
			//var JObjectUsers = JObject.Parse(chatData["users"].ToString());

			foreach (var userData in JObjectUserT)
			{
				var user = new User(userData["name"].ToString(), userData["id"].ToString());
				users.Add(user);

				/*foreach (var item in userData.Value)
				{
					var user = new User(item["name"].ToString(), item["id"].ToString());
					users.Add(user);
				}*/
			}
		}
		
		
		var chat = new Chat(chatData["name"].ToString(), chatData["password"].ToString());

		if (chat != null) onChatJoinSuccessful?.Invoke(chat, users.ToArray());
		else onChatJoinError?.Invoke();

		onChatJoinSuccessful = null;
		onChatJoinError = null;
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

		AccountManager.Init();
	}

	private void OnDisconnectToServer()
	{
		Debug.Log("Disconnect to server!");
		_isConnected = false;
		_socketManager = null;

		AccountManager.Close();
	}
}
