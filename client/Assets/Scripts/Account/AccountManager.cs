using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AccountManager
{
	private static Account _account;

	public static void Init()
	{
		Debug.Log("Init account base!");
		_account = Account.Create();
	}

	public static void RegisterUserFromAccount(string nickname, string password, Action<bool> callback)
	{
		if (CheckingDataNicknameAndPassword(nickname, password) == false) return;

		Debug.Log("Creating user to account...");

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

	public static void LoginUserToAccount(string nickname, string password, Action<bool> callback)
	{
		if (CheckingDataNicknameAndPassword(nickname, password) == false) return;

		Debug.Log("Login to user account...");

		_account.Register(nickname, password, (userId) =>
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
