using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrackSelectUI : TrackOptionUI, IPointerDownHandler
{
	[ReadOnly]
	[SerializeField]
	private int _trackIndex;

	[SerializeField]
	private TMP_Text trackNameText;


	public override void Init(int trackIndex)
	{
		_trackIndex = trackIndex;
		trackNameText.text = GameManager.TrackManager.GetTrack(_trackIndex).TrackName;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		GameManager.TrackManager.SetCurrentTrack(_trackIndex);
		GameManager.GameEventManager.OnCurrentTrackChanged?.Invoke(_trackIndex);
	}

	public override void SetAlpha(float alpha)
	{
		base.SetAlpha(alpha);
		var nameColor = trackNameText.color;
		trackNameText.color = new Color(nameColor.r, nameColor.g, nameColor.b, alpha);
	}
}
