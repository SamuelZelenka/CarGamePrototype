using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditActions : MonoBehaviour
{
	public void FlipTrackDirection()
	{
		GameManager.TrackManager.GetCurrentTrack().FlipTrackDirection();
		GameManager.GameEventManager.OnCurrentTrackChanged?.Invoke(GameManager.TrackManager.GetCurrentTrackIndex());
	}
}
