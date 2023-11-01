using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMessageUI : MonoBehaviour
{
	[SerializeField]
	private Color32 _defaultErrorColor = new Color32(0xCC, 0x42, 0x5E, 0XFF);

	[SerializeField]
	private Color32 _defaultMessageColor = new Color32(0XFF, 0XFF, 0XFF, 0XFF);

	[SerializeField]
	private float _duration;

	[SerializeField]
	private TMP_Text _messageText;
	private void SetErrorText(string message, bool isError)
	{
		StartCoroutine(ShowErrorMessage(message, isError));
	}

	private IEnumerator ShowErrorMessage(string message, bool isError)
	{
		var color = isError ? _defaultErrorColor : _defaultMessageColor;
		_messageText.color = color;
		_messageText.text = message;
		yield return new WaitForSecondsRealtime(_duration);
		_messageText.text = "";

	}

	private void OnEnable()
	{
		GameManager.GameEventManager.OnGameMessage += SetErrorText;
	}
}
