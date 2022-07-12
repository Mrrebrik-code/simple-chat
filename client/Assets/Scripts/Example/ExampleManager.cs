using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExampleManager : MonoBehaviour
{
	[SerializeField] private UIButton _registeAccountButton;
	[SerializeField] private TMP_InputField _nicknameInput;
	[SerializeField] private TMP_InputField _passwordInput;
	private void Start()
	{
		_registeAccountButton.Subscribe(RegisterAccount);
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
				Debug.LogError("COMPLET");
			}
			else
			{
				Debug.LogError("ERROR");
			}
		});
	}
}
