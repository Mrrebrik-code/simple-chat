using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO3;
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


	}

	private void OnConnectToServer()
	{
		Debug.Log("Connection to server!");
		_isConnected = true;
		SubscribeSocketIOEvents();

	}

	private void OnDisconnectToServer()
	{
		Debug.Log("Disconnect to server!");
		_isConnected = false;
		_socketManager = null;
	}
}
