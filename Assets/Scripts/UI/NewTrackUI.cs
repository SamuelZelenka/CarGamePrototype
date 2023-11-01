using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewTrackUI : TrackOptionUI
{
	[SerializeField]
	private TMP_Text _placeholderText;
	[SerializeField]
	private TMP_Text _inputText;
	[SerializeField]
	private StringCondition _condition;
	public override void Init(int index)
	{
		throw new System.NotImplementedException();
	}

	public void CreateNewTrack()
	{
		var trackName = _inputText.text;
		if (_condition.IsValid(trackName, out string message))
		{
			var newTrackIndex = GameManager.TrackManager.CreateNewTrack(trackName);
			GameManager.TrackManager.SetCurrentTrack(newTrackIndex);
			GameManager.GameEventManager.OnCurrentTrackChanged?.Invoke(newTrackIndex);
		}

		var isError = message != "";
		message = !isError ? $"Successfully created track {trackName}" : message;
		//GameManager.GameEventManager.OnGameMessage?.Invoke(message, isError);
		_inputText.text = "...";
	}

	public override void SetAlpha(float alpha)
	{
		base.SetAlpha(alpha);
		var color = _placeholderText.color;
		_placeholderText.color = new Color(color.r, color.g, color.b, alpha);
		color = _inputText.color;
		_placeholderText.color = new Color(color.r, color.g, color.b, alpha);
	}
}
