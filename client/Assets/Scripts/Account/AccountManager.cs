using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AccountManager
{
	private static Account _account;

	public static void Open()
	{
		Debug.Log("Open account!");
		_account = Account.Create();
	}

	public static void Close()
	{
		Debug.Log("Close account!");
		_account = null;
	}

	public static void RegisterUserFromAccount(string nickname, string password, Action<bool> callback)
	{
		if (CheckingDataNicknameAndPassword(nickname, password) == false) return;

		Debug.Log("Creating user to account...");

		if(_account == null)
		{
			Debug.LogError("You not connecting to server! Please connect!");
			callback?.Invoke(false);
			return;
		}

		_account.Register(nickname, password, (userId) =>
		{
			Debug.Log($"Successful create user! ID: {userId}");
			callback?.Invoke(true);
		}, () =>

		{
			Debug.Log($"Error create user");
			callback?.Invoke(false);
		});
	}

	public static User GetCurrentUser()
	{
		return _account.User;
	}

	public static void LoginUserToAccount(string nickname, string password, Action<bool> callback)
	{
		if (CheckingDataNicknameAndPassword(nickname, password) == false) return;

		Debug.Log("Login to user account...");

		if (_account == null)
		{
			Debug.LogError("You not connecting to server! Please connect!");
			callback?.Invoke(false);
			return;
		}

		_account.Login(nickname, password, (userId) =>
		{
			Debug.Log($"Successful login user! ID: {userId}");
			callback?.Invoke(true);
		}, () =>
		{
			Debug.Log($"Error login user");
			callback?.Invoke(false);
		});
	}

	private static bool CheckingDataNicknameAndPassword(string nickname, string password)
	{
		if (nickname.Length < 10 && string.IsNullOrEmpty(nickname) == true)
		{
			Debug.Log("Имя слишком длинное или вы его не написали!");
			return false;
		}

		if (password.Length <= 4)
		{
			Debug.Log("Пароль слишком короткий!");
			return false;
		}
		

		return true;
	}
}
