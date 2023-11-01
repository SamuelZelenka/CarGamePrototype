using TMPro;
using UnityEngine;

public class SelectedTrackName : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _selectedTrackText;

	private void Start()
	{
		var trackIndex = GameManager.TrackManager.GetCurrentTrackIndex();
		if (trackIndex == -1)
			return;

		UpdateSelectedTrack(trackIndex);
		GameManager.GameEventManager.OnCurrentTrackChanged += UpdateSelectedTrack;
	}

	private void UpdateSelectedTrack(int trackIndex)
	{
		var trackData = GameManager.TrackManager.GetTrack(trackIndex);
		if (trackData == null)
			return;

		_selectedTrackText.text = trackData.TrackName;
	}
}
