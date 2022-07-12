using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO3;
using System;
using Newtonsoft.Json;

public class NetworkManager : SingletonMono<NetworkManager>
{
	private SocketManager _socketManager = null;

	[SerializeField] private string _address = "http://localhost:52300";
	[SerializeField] private bool _isReconection = false;
	[SerializeField] private bool _isAutoConnect = true;
	private bool _isConnected = false;

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
	}

	public void RegisterUserToServer(User user, Action<string> callback)
	{
		var jsonSerializerSettings = new JsonSerializerSettings()
		{
			Formatting = Formatting.Indented
		};

		var json = JsonConvert.SerializeObject(user, jsonSerializerSettings);

		Debug.Log(json);

		_socketManager.Socket.Emit(EmitIOEvent.RegisterAccountUser, json);
	}

	private void OnLoginUserToServer(string data)
	{
		Debug.Log("OnLoginUserToServer");
		Debug.Log(data);
	}

	private void OnRegisterUserToServer(string data)
	{
		Debug.Log("OnRegisterUserToServer");
		Debug.Log(data);
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
	}
}
