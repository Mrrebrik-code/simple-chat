using System;
using TMPro;
using UnityEngine;

public class UserHolder : MonoBehaviour
{
	[SerializeField] private TMP_Text _nicknameUserText;
	[SerializeField] private TMP_Text _idUserText;
	[SerializeField] private TMP_Text _indexUserText;

	[SerializeField] private UIButton _kickUserButton;
	[SerializeField] private UIButton _banUserButton;

	public User User { get; private set; }

	private Action<TypeHandleUserHolder> _callback = null;

	public void Init(User user, int index)
	{
		User = user;

		_indexUserText.text = index.ToString();
		_idUserText.text = user.Id.ToString();
		_nicknameUserText.text = user.Nickname;

		_kickUserButton.Subscribe(HandleButtonKick);
		_banUserButton.Subscribe(HandleButtonBan);
	}


	public void Subscribe(Action<TypeHandleUserHolder> callback)
	{
		_callback += callback;
	}

	public void UnSubscribe(Action<TypeHandleUserHolder> callback)
	{
		_callback -= callback;
	}

	private void HandleButtonKick()
	{
		Debug.Log("KICK");
		_callback?.Invoke(TypeHandleUserHolder.KICK);
	}

	private void HandleButtonBan()
	{
		Debug.Log("BAN");
		_callback?.Invoke(TypeHandleUserHolder.BAN);
	}
}
