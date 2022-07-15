using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour
{
    private Button _button;
    private Action _onClick;
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Click);
    }

    private void Click()
	{
        _onClick?.Invoke();
    }

    public void Subscribe(Action callback)
	{
        _onClick += callback;
    }

    public void UnSubscribe(Action callback)
    {
        _onClick -= callback;
    }

    private void OnDestroy()
	{
        _button.onClick.RemoveListener(Click);
    }
}
