using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditActions : MonoBehaviour
{
	[SerializeField]
	private RunTimePathEditor _runtimeEditor;
	public void FlipTrackDirection()
	{
		_runtimeEditor.FlipTrackDirection();
		GameManager.GameEventManager.OnCurrentTrackChanged?.Invoke(GameManager.TrackManager.GetCurrentTrackIndex());
	}
}
