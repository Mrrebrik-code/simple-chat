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

	[Space]

	[Header("Join chat:")]
	[SerializeField] private UIButton _joinChatButton;
	[SerializeField] private TMP_InputField _nameChatJoinInput;
	[SerializeField] private TMP_InputField _passwordChatJoinInput;

	[SerializeField] private UIButton _leaveChatButton;

	private void Start()
	{
		_registeAccountButton.Subscribe(RegisterAccount);
		_loginAccountButton.Subscribe(LoginAccount);
		_createChatButton.Subscribe(CreateChat);
		_joinChatButton.Subscribe(JoinChat);
		_leaveChatButton.Subscribe(LeaveChat);
	}

	private void LeaveChat()
	{
		ChatManager.LeaveChat();
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

	private void JoinChat()
	{
		Debug.Log("JoinChat UIButton");

		var name = _nameChatJoinInput.text;
		var password = _passwordChatJoinInput.text;

		ChatManager.JoinChat(name, password, (callback, users) =>
		{
			if (callback)
			{
				Debug.Log("**COMPLET CHAT JOIN");

				foreach (var user in users)
				{
					CreateUserToPanel(user);
				}
			}
			else
			{
				Debug.Log("**FAILED CHAT JOIN");
			}
		});
	}

	[SerializeField] private Transform _contentUserHolders;
	[SerializeField] private UserHolder _userHolderPrefab;

	private List<UserHolder> _userHolders = new List<UserHolder>();
	private void CreateUserToPanel(User user)
	{
		var userHolder = Instantiate(_userHolderPrefab, _contentUserHolders);

		userHolder.Init(user, _userHolders.Count + 1);
		userHolder.Subscribe(HandleUserHolder);

		_userHolders.Add(userHolder);
	}

	private void HandleUserHolder(TypeHandleUserHolder typeHandler)
	{
		switch (typeHandler)
		{
			case TypeHandleUserHolder.kick:
				Debug.Log("TypeHandleUserHolder.kick");
				break;
			case TypeHandleUserHolder.ban:
				Debug.Log("TypeHandleUserHolder.ban");
				break;
		}
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
