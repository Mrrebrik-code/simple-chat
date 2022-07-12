using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoggerManager : SingletonMono<LoggerManager>
{
	[SerializeField] private TMP_Text _logsText;

	public override void Awake()
	{
		base.Awake();
		Application.logMessageReceived += HandleLogUnity;
	}

	private void HandleLogUnity(string condition, string stackTrace, LogType type)
	{
		AddLog(condition);
	}
	public void AddLog(string log)
	{
		_logsText.text += $"{log}{Environment.NewLine}";
	}
}
