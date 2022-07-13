using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExampleManager : MonoBehaviour
{
	[Header("Register account:")]
	[SerializeField] private UIButton _registeAccountButton;
	[SerializeField] private TMP_InputField _nicknameInput;
	[SerializeField] private TMP_InputField _passwordInput;

	[Space]

	[Header("Login account:")]
	[SerializeField] private UIButton _loginAccountButton;
	[SerializeField] private TMP_InputField _nicknameInput2;
	[SerializeField] private TMP_InputField _passwordInput2;

	[Space]

	[Header("Create chat:")]
	[SerializeField] private UIButton _createChatButton;
	[SerializeField] private TMP_InputField _nameChatInput;
	[SerializeField] private TMP_InputField _passwordChatInput;

	private void Start()
	{
		_registeAccountButton.Subscribe(RegisterAccount);
		_loginAccountButton.Subscribe(LoginAccount);
		_createChatButton.Subscribe(CreateChat);
	}

	private void CreateChat()
	{
		Debug.Log("CreateChat UIButton");

		var name = _nameChatInput.text;
		var password = _passwordChatInput.text;

		ChatManager.CreateChat(name, password, (callback) =>
		{
			if (callback)
			{
				Debug.Log("**COMPLET CHAT CREATE");
			}
			else
			{
				Debug.Log("**FAILED CHAT CREATE");
			}
		});
	}

	private void RegisterAccount()
	{
		Debug.Log("RegisterAccount UIButton");

		var nickname = _nicknameInput.text;
		var password = _passwordInput.text;
		AccountManager.RegisterUserFromAccount(nickname, password, (callback) =>
		{
			if (callback)
			{
				Debug.LogError("COMPLET REGISTER");
			}
			else
			{
				Debug.LogError("ERROR REGISTER");
			}
		});
	}

	private void LoginAccount()
	{
		Debug.Log("LoginAccount UIButton");

		var nickname = _nicknameInput2.text;
		var password = _passwordInput2.text;
		AccountManager.LoginUserToAccount(nickname, password, (callback) =>
		{
			if (callback)
			{
				Debug.LogError("COMPLET LOGIN");
			}
			else
			{
				Debug.LogError("ERROR LOGIN");
			}
		});
	}
}
