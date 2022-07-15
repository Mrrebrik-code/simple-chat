using System;
using System.Collections.Generic;
using System.Linq;
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

	[Space]

	[Header("Leave chat:")]
	[SerializeField] private UIButton _leaveChatButton;

	[Space]

	[Header("Chat messanger:")]
	[SerializeField] private UIButton _sendMessageButton;
	[SerializeField] private TMP_InputField _messageChatInput;
	[SerializeField] private TMP_Text _messagesText;

	[Space]

	[Header("Users holder:")]
	[SerializeField] private Transform _contentUserHolders;
	[SerializeField] private UserHolder _userHolderPrefab;

	private List<UserHolder> _userHolders = new List<UserHolder>();

	private void Start()
	{
		_registeAccountButton.Subscribe(RegisterAccount);
		_loginAccountButton.Subscribe(LoginAccount);
		_createChatButton.Subscribe(CreateChat);
		_joinChatButton.Subscribe(JoinChat);
		_leaveChatButton.Subscribe(LeaveChat);
		_sendMessageButton.Subscribe(SendMessage);
	}

	private void SendMessage()
	{
		Debug.Log("SendMessage UIButton");

		var messageText = _messageChatInput.text;
		var user = AccountManager.GetCurrentUser();

		if(user != null)
		{
			MessageTargetUser(new Message(messageText, user));
			ChatManager.SendMessageToChat(messageText, user);
		}
	}

	private void LeaveChat()
	{
		Debug.Log("LeaveChat UIButton");

		ChatManager.LeaveChat();

		_userHolders.ForEach(userHolder => Destroy(userHolder.gameObject));

		_userHolders = new List<UserHolder>();
	}

	private void CreateChat()
	{
		Debug.Log("CreateChat UIButton");

		var nameChat = _nameChatInput.text;
		var passwordChat = _passwordChatInput.text;

		ChatManager.CreateChat(nameChat, passwordChat, (isCreateChatToServer) =>
		{
			if (isCreateChatToServer)
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

		var nameChat = _nameChatJoinInput.text;
		var passwordChat = _passwordChatJoinInput.text;

		ChatManager.JoinChat(nameChat, passwordChat, (isJoinToChat, users) =>
		{
			if (isJoinToChat)
			{
				Debug.Log("**COMPLET CHAT JOIN");

				users.ToList().ForEach(user => CreateUserToPanel(user));

				ChatManager.GetCurrentChat().SubscribeLeaveAndJoin(LeaveTargetUser, JoinTargetUser, MessageTargetUser);
			}
			else
			{
				Debug.Log("**FAILED CHAT JOIN");
			}
		});
	}

	private void MessageTargetUser(Message message)
	{
		_messagesText.text += $"[{message.User.Id}] {message.User.Nickname}: {message.Text}{Environment.NewLine}";
	}

	private void JoinTargetUser(User user)
	{
		CreateUserToPanel(user);
	}

	private void LeaveTargetUser(User user)
	{
		var userHolder = _userHolders.FirstOrDefault(x => x.User.Id == user.Id);
		userHolder.Remove(ref _userHolders);

		Destroy(userHolder.gameObject);
	}

	
	private void CreateUserToPanel(User user)
	{
		var userHolder = Instantiate(_userHolderPrefab, _contentUserHolders);

		userHolder.Init(user, _userHolders.Count + 1);
		userHolder.Subscribe(HandleUserHolder);

		userHolder.Add(ref _userHolders);
	}

	private void HandleUserHolder(TypeHandleUserHolder typeHandler)
	{
		switch (typeHandler)
		{
			case TypeHandleUserHolder.KICK:
				Debug.Log("TypeHandleUserHolder.kick");
				break;
			case TypeHandleUserHolder.BAN:
				Debug.Log("TypeHandleUserHolder.ban");
				break;
		}
	}

	private void RegisterAccount()
	{
		Debug.Log("RegisterAccount UIButton");

		var nickname = _nicknameInput.text;
		var password = _passwordInput.text;
		AccountManager.RegisterUserFromAccount(nickname, password, (isRegister) =>
		{
			if (isRegister)
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
		AccountManager.LoginUserToAccount(nickname, password, (isLogin) =>
		{
			if (isLogin)
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
