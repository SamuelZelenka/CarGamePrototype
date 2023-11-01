using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
	public Action<int> OnCurrentTrackChanged;
	public Action<int> OnTrackCreated;
	public Action<string, bool> OnGameMessage;

	private void OnDisable()
	{
		OnCurrentTrackChanged = null;
		OnTrackCreated = null;
		OnGameMessage = null;
	}
}
