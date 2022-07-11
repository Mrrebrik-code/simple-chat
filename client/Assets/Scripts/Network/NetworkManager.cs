using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO3;
public class NetworkManager : SingletonMono<NetworkManager>
{
	private SocketManager _socketManager = null;

	[SerializeField] private string _address = "http://localhost:52300";
	[SerializeField] private bool _isReconection = false;

	private bool _isConnected = false;

	public void ConnectedToServer()
	{
		if(_socketManager == null && _isConnected == false)
		{
			SocketOptions options = new SocketOptions()
			{
				Reconnection = _isReconection
			};
			_socketManager = new SocketManager(new System.Uri(_address), options);


			_socketManager.Socket.On(SocketIOEventTypes.Connect, () =>
			{
				Debug.Log("Connection to server!");
				_isConnected = true;
			});
			_socketManager.Socket.On(SocketIOEventTypes.Disconnect, () =>
			{
				Debug.Log("Disconnect to server!");
				_isConnected = false;
				_socketManager = null;
			});
		}
	}

	public void DisconnectFromServer()
	{
		if(_socketManager != null && _isConnected == true)
		{
			_socketManager.Close();
			_socketManager = null;
		}
	}
}
