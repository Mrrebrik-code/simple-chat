using System;

[Serializable]
public class Chat 
{
	public string Name { get; private set; }
	public string Password { get; private set; }
	public User[] Users { get; private set; }

	public Action<User> onLeaveTargetUser { get; set; }
	public Action<User> onJoinTargetUser { get; set; }
	public Action<Message> onMessageUser { get; set; }

	public Chat(string name, string password)
	{
		Name = name;
		Password = password;
	}

	~Chat()
	{
		onLeaveTargetUser = null;
		onJoinTargetUser = null;
	}

	public static void Create(string nameChat, string passwordChat, Action<Chat, User[]> callbackSuccessfulCreated, Action callbackErrorCreated)
	{
		Chat chat = CreateObjectChat(nameChat, passwordChat);

		NetworkManager.Instance.CreateChatToServer(chat, callbackSuccessfulCreated, callbackErrorCreated);
	}

	public static void Join(string nameChat, string passwordChat, Action<Chat, User[]> callbackSuccessfulJoined, Action callbackErrorJoined)
	{
		Chat chat = CreateObjectChat(nameChat, passwordChat);

		NetworkManager.Instance.JoinChatFromServer(chat, callbackSuccessfulJoined, callbackErrorJoined);
	}

	public void Leave()
	{
		NetworkManager.Instance.LeaveChatFromServerCurrentUser();
	}

	public void SendMessage(Message message)
	{
		NetworkManager.Instance.SendMessageToChatFromServer(message);
	}

	public void LeaveTargetUser(User user)
	{
		onLeaveTargetUser?.Invoke(user);
	}

	public void JoinTargetUser(User user)
	{
		onJoinTargetUser?.Invoke(user);
	}

	public void MessageTargetUser(Message message)
	{
		onMessageUser?.Invoke(message);
	}

	private static Chat CreateObjectChat(string nameChat, string passwordChat)
	{
		Chat chat = new Chat(nameChat, passwordChat);

		return chat;
	}

	public void SetUsers(User[] users)
	{
		Users = users;
	}
}
